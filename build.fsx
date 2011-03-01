#I "tools/FAKE"
#r "FakeLib.dll"

open Fake
open Fake.Git
open System.Linq
open System.Text.RegularExpressions

(* properties *)
let authors = ["Steffen Forkmann"]
let projectName = "FSharp.Testing"
let description = "Some extensions which help to make F# code testable from C# test projects."
let copyright = "Copyright - FSharp.Testing 2011"
let version = if isLocalBuild then "0.0.0.1" else buildVersion

(* Directories *)
let buildDir = @".\Build\"
let docsDir = buildDir + @"Documentation\"
let testOutputDir = buildDir + @"Specs\"
let nugetDir = buildDir + @"NuGet\" 
let testDir = buildDir
let deployDir = @".\Release\"
let nugetDocsDir = nugetDir + @"docs\"
let nugetLibDir = nugetDir + @"lib\"

(* files *)
let appReferences = !! @".\Source\**\*.csproj"

(* Targets *)
Target "Clean" (fun _ -> CleanDirs [buildDir; testDir; deployDir; docsDir; testOutputDir] )

Target "BuildApp" (fun _ -> 
    AssemblyInfo
      (fun p -> 
        {p with
            CodeLanguage = CSharp;
            AssemblyVersion = version;
            AssemblyTitle = projectName;
            AssemblyDescription = description;
            AssemblyCopyright = copyright;
            Guid = "81DEA79A-B464-411C-B861-D2716DBD3B30";
            OutputFileName = @".\Source\GlobalAssemblyInfo.cs"})                      

    appReferences
        |> MSBuildRelease buildDir "Build"
        |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
    ActivateFinalTarget "DeployTestResults"
    !+ (testDir + "/*.Specs.dll")
      ++ (testDir + "/*.Examples.dll")
        |> Scan
        |> MSpec (fun p -> 
                    {p with 
                        HtmlOutputDir = testOutputDir})
)

FinalTarget "DeployTestResults" (fun () ->
    !+ (testOutputDir + "\**\*.*") 
      |> Scan
      |> Zip testOutputDir (sprintf "%sMSpecResults.zip" deployDir)
)

Target "GenerateDocumentation" (fun _ ->
    !+ (buildDir + "FSharp.Testing.dll")      
      |> Scan
      |> Docu (fun p ->
          {p with
              ToolPath = "./tools/docu/docu.exe"
              TemplatesPath = "./tools/docu/templates"
              OutputPath = docsDir })
)

Target "ZipDocumentation" (fun _ ->    
    !! (docsDir + "/**/*.*")
      |> Zip docsDir (deployDir + sprintf "Documentation-%s.zip" version)
)

Target "BuildZip" (fun _ -> 
    !+ (buildDir + "/**/*.*")     
      -- "*.zip"
      -- "**/*.Specs.*"
        |> Scan
        |> Zip buildDir (deployDir + sprintf "%s-%s.zip" projectName version)
)

Target "BuildNuGet" (fun _ ->
    CleanDirs [nugetDir; nugetLibDir; nugetDocsDir]
        
    XCopy docsDir nugetDocsDir
    [buildDir + "FSharp.Core.dll"
     buildDir + "FSharp.Testing.dll"]
        |> CopyTo nugetLibDir

    NuGet (fun p -> 
        {p with               
            Authors = authors
            Project = projectName
            Version = version                        
            OutputPath = nugetDir
            Description = description
            AccessKey = getBuildParamOrDefault "nugetkey" ""
            Publish = hasBuildParam "nugetkey" })
        "fsharp.testing.nuspec"

    [nugetDir + sprintf "FSharp.Testing.%s.nupkg" version]
        |> CopyTo deployDir
)

Target "Default" DoNothing
Target "Deploy" DoNothing

// Dependencies
"BuildApp" <== ["Clean"]
"Test" <== ["BuildApp"]
"BuildZip" <== ["Test"]
"ZipDocumentation" <== ["GenerateDocumentation"]
"Deploy" <== ["BuildZip"; "ZipDocumentation"; "BuildNuGet"]
"Default" <== ["Deploy"]

// start build
Run <| getBuildParamOrDefault "target" "Default"