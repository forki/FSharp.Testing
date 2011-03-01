using System.Linq;
using System.Reflection;
using FSharp.CodeSamples;
using Machine.Specifications;

namespace FSharp.Testing.Specs
{
    public class when_retrieving_record_fields
    {
        static PropertyInfo[] _fields;
        Because of = () => _fields = typeof (Records.MyRecord).GetRecordFields();
        It should_have_MyProperty2_field = () => _fields[0].Name.ShouldEqual("Property1");
        It should_have_Property1_field = () => _fields[0].Name.ShouldEqual("Property1");
        It should_have_two_fields = () => _fields.Count().ShouldEqual(2);
    }

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

    public class when_setting_a_value_on_an_automatic_property_with_no_visible_setter_in_the_record
    {
        static Records.MyRecord _myRecord;
        Establish context = () => _myRecord = new Records.MyRecord(0, "");

        Because of = () => _myRecord.Set(x => x.Property1).To(10);

        It should_reflect_the_new_value = () => _myRecord.Property1.ShouldEqual(10);
    }

    public class when_retrieving_the_property_name_of_a_property_in_a_record
    {
        static string _name;

        Because of = () => _name = FSharpRecordExtensions.GetPropertyName<Records.MyRecord, int>(x => x.Property1);

        It should_yield_Property1 = () => _name.ShouldEqual("Property1");
    }
}