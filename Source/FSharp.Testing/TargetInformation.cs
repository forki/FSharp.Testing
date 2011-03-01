using System.Reflection;

namespace FSharp.Testing
{
    public class TargetInformation<T>
    {
        public TargetInformation(T target, FieldInfo backingField)
        {
            Target = target;
            BackingField = backingField;
        }

        public T Target { get; private set; }
        public FieldInfo BackingField { get; private set; }
    }
}