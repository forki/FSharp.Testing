using FSharp.CodeSamples;
using Machine.Specifications;

namespace FSharp.Testing.Specs
{
    public class when_copying_a_record
    {
        static Records.MyRecord _myRecord;
        static Records.MyRecord _myOldRecord;
        Establish context = () => _myOldRecord = new Records.MyRecord(0, "");
        Because of = () => _myRecord = _myOldRecord.CopyRecord();

        It should_have_the_same_MyProperty2_value = () => _myRecord.MyProperty2.ShouldEqual(_myOldRecord.MyProperty2);
        It should_have_the_same_Property1_value = () => _myRecord.Property1.ShouldEqual(_myOldRecord.Property1);
        It should_not_be_the_same_record = () => _myRecord.ShouldNotBeTheSameAs(_myOldRecord);
    }

    public class when_creating_a_slightly_modified_fsharp_record
    {
        static Records.MyRecord _oldRecord;
        static Records.MyRecord _newRecord;
        Establish context = () => _oldRecord = new Records.MyRecord(0, "");

        Because of = () => _newRecord = _oldRecord.Set(x => x.Property1).To(10);

        It should_reflect_the_new_value = () => _newRecord.Property1.ShouldEqual(10);        
        It should_not_change_the_old_record = () => _oldRecord.Property1.ShouldEqual(0);        
    }

    public class when_retrieving_the_property_name_of_a_property_in_a_record
    {
        static string _name;

        Because of = () => _name = FSharpRecordExtensions.GetPropertyName<Records.MyRecord, int>(x => x.Property1);

        It should_yield_Property1 = () => _name.ShouldEqual("Property1");
    }
}