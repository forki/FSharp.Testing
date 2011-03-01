using Machine.Specifications;

namespace FSharp.Testing.Specs
{
    public class when_setting_a_value_on_an_automatic_property_with_no_visible_setter
    {
        static MyRecord _myRecord;
        Establish context = () => _myRecord = new MyRecord(0);

        Because of = () => _myRecord.Set(x => x.Property1).To(10);

        It should_reflect_the_new_value = () => _myRecord.Property1.ShouldEqual(10);
    }

    class MyRecord
    {
        public MyRecord(int property1)
        {
            Property1 = property1;
        }

        public int Property1 { get; private set; }
    }
}