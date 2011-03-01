using Machine.Specifications;

namespace FSharp.Testing.Specs
{
    class MyClass
    {
        public MyClass(int property1, string myStringProperty)
        {
            Property1 = property1;
            MyStringProperty = myStringProperty;
        }

        public int Property1 { get; private set; }
        public string MyStringProperty { get; private set; }
    }

    public class when_setting_a_value_on_an_automatic_property_with_no_visible_setter_in_the_class
    {
        static MyClass _myClass;
        Establish context = () => _myClass = new MyClass(0, "");

        Because of = () => _myClass.Set(x => x.Property1).To(10);

        It should_reflect_the_new_value = () => _myClass.Property1.ShouldEqual(10);
    }

    public class when_retrieving_the_property_name_of_a_property_in_a_class
    {
        static string _name;

        Because of = () => _name = FSharpRecordExtensions.GetPropertyName<MyClass, string>(x => x.MyStringProperty);

        It should_yield_Property1 = () => _name.ShouldEqual("MyStringProperty");
    }
}