using System;
using System.Reflection;

namespace FSharp.Testing
{
    public class BackingFields
    {
        /// <summary>
        ///   Gets the backing field for the given property.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "propertyName">Name of the property.</param>
        /// <returns></returns>
        public static FieldInfo GetBackingField<T>(string propertyName)
        {
            return GetBackingField(typeof (T), propertyName);
        }

        /// <summary>
        /// Gets the backing field for a given property name.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static FieldInfo GetBackingField(Type type, string propertyName)
        {
            // TODO: Apply Open-Closed-Principle
            var fieldName = propertyName + "@";
            var field = GetPrivateFieldInfo(type, fieldName);

            if (field == null)
            {
                fieldName = String.Format("<{0}>k__BackingField", propertyName);
                field = GetPrivateFieldInfo(type, fieldName);
            }

            if (field == null)
            {
                fieldName = propertyName + "Field";
                field = GetPrivateFieldInfo(type, fieldName);
            }
            if (field == null)
                throw new Exception(String.Format("Backing field for property {0} could not be found.", propertyName));
            return field;
        }

        static FieldInfo GetPrivateFieldInfo(Type type, string fieldName)
        {
            return type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}