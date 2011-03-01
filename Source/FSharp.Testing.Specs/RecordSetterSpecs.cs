using FSharp.CodeSamples;
using Machine.Specifications;

namespace FSharp.Testing.Specs
{
    public class when_setting_a_value_on_an_automatic_property_with_no_visible_setter_in_the_record
    {
        static Records.MyRecord _myClass;
        Establish context = () => _myClass = new Records.MyRecord(0, "");

        Because of = () => _myClass.Set(x => x.Property1).To(10);

        It should_reflect_the_new_value = () => _myClass.Property1.ShouldEqual(10);
    }

    public class when_retrieving_the_property_name_of_a_property_in_a_record
    {
        static string _name;

        Because of = () => _name = FSharpRecordExtensions.GetPropertyName<Records.MyRecord, int>(x => x.Property1);

        It should_yield_Property1 = () => _name.ShouldEqual("Property1");
    }
}