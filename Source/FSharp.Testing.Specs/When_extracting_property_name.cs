using Machine.Specifications;

namespace FSharp.Testing.Specs
{
    public class When_extracting_property_name
    {
        static MyRecord _myRecord;
        Establish context = () => _myRecord = new MyRecord(0);

        Because of = () => _myRecord.Set(x => x.Property1).To(10);

        It should_have_right_value_in_property = () => _myRecord.Property1.ShouldEqual(10);
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