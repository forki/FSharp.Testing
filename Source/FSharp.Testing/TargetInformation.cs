namespace FSharp.Testing
{
    public class TargetInformation<T>
    {
        public TargetInformation(T target, string propertyName)
        {
            Target = target;
            PropertyName = propertyName;
        }

        public T Target { get; private set; }
        public string PropertyName { get; private set; }
    }
}