# What is FSharp.Testing ?

FSharp.Testing is a little framework which allows to test F# code from C#.

## How to contribute code

* Login in github (you need an account)
* Fork the main repository from [Github](https://github.com/forki/FSharp.Testing)
* Push your changes to your fork
* Send me a pull request

# How to get started

## Building FSharp.Testing from source

Just download the repository from github and run the build.cmd. The build of FSharp.Testing only requires the .NET Framework 4.0 to be installed on your machine. Everything else should work out-of-the-box. If not, please take the time to add an issue to this project. After a succesful build you find all the assemblies in a zip file under the "Release" folder.

## Getting FSharp.Testing via the NuGet package manager

If you've got NuGet installed on your machine it gets even easier:

        install-package FSharp.Testing

## How to use it

### Working with F# records in C# testing code

You can easily create slightly modified F# records with the following "Set/To"-syntax:

	var myNewRecord =
	  myDefaultRecord
		.Set(p => p.Property1).To(true)
		.Set(p => p.Property2).To("test");

or with the following "With"-syntax:

	var myNewRecord =
	  myDefaultRecord
		.With(p => p.Property1, true)
		.With(p => p.Property2, "test");

Please note that this methods will not mutate your F# records. Instead they will create new record copies just as you would expect for immutable F# records.