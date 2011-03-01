using System.Reflection;

namespace FSharp.Testing
{
    public class TargetInformation<T>
    {
        public TargetInformation(T target, PropertyInfo property)
        {
            Target = target;
            Property = property;
        }

        public T Target { get; private set; }
        public PropertyInfo Property { get; private set; }
    }
}