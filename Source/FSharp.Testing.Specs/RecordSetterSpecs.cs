using FSharp.CodeSamples;
using Machine.Specifications;

namespace FSharp.Testing.Specs
{
    public class when_setting_a_value_on_an_automatic_property_with_no_visible_setter_in_the_class
    {
        static MyClass _myClass;
        Establish context = () => _myClass = new MyClass(0);

        Because of = () => _myClass.Set(x => x.Property1).To(10);

        It should_reflect_the_new_value = () => _myClass.Property1.ShouldEqual(10);
    }

    class MyClass
    {
        public MyClass(int property1)
        {
            Property1 = property1;
        }

        public int Property1 { get; private set; }
    }

    public class when_setting_a_value_on_an_automatic_property_with_no_visible_setter_in_the_record
    {
        static Records.MyRecord _myClass;
        Establish context = () => _myClass = new Records.MyRecord(0,"");

        Because of = () => _myClass.Set(x => x.Property1).To(10);

        It should_reflect_the_new_value = () => _myClass.Property1.ShouldEqual(10);
    }
}