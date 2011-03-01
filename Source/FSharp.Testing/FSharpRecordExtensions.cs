using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FSharp.Testing
{
    ///<summary>
    ///  Extensions for FSharp Records
    ///</summary>
    public static class FSharpRecordExtensions
    {
        /// <summary>
        ///   Gets the name of the property.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <typeparam name = "TProperty">The type of the property.</typeparam>
        /// <param name = "expression">The expression.</param>
        /// <returns></returns>
        public static string GetPropertyName<T, TProperty>(this Expression<Func<T, TProperty>> expression)
        {
            return ((MemberExpression) expression.Body).Member.Name;
        }

        /// <summary>
        ///   Extracts the property name and creates a tuple with target object and backing field reference.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <typeparam name = "TProperty"></typeparam>
        /// <param name = "target">The target.</param>
        /// <param name = "expression">The expression.</param>
        /// <returns></returns>
        public static Tuple<T, FieldInfo> Set<T, TProperty>(this T target, Expression<Func<T, TProperty>> expression)
        {
            var propertyName = expression.GetPropertyName();
            var field = GetBackingField<T>(propertyName);

            return Tuple.Create(target, field);
        }

        /// <summary>
        ///   Gets the backing field for the given property.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "propertyName">Name of the property.</param>
        /// <returns></returns>
        public static FieldInfo GetBackingField<T>(string propertyName)
        {
            var fieldName = propertyName + "@";
            var field = GetPrivateFieldInfo<T>(fieldName);

            if (field == null)
            {
                fieldName = string.Format("<{0}>k__BackingField", propertyName);
                field = GetPrivateFieldInfo<T>(fieldName);
            }
            if (field == null)
                throw new Exception(string.Format("Backing field for property {0} could not be found.", propertyName));
            return field;
        }

        static FieldInfo GetPrivateFieldInfo<T>(string fieldName)
        {
            return typeof (T).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        }

        /// <summary>
        ///   Takes a tuple with target object and backing field reference and sets the given value on it.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <typeparam name = "TProperty"></typeparam>
        /// <param name = "tuple">The tuple.</param>
        /// <param name = "value">The value.</param>
        /// <returns></returns>
        public static T To<T, TProperty>(this Tuple<T, FieldInfo> tuple, TProperty value)
        {
            tuple.Item2.SetValue(tuple.Item1, value);
            return tuple.Item1;
        }

        /// <summary>
        ///   Extracts the property name and sets the given value on it.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <typeparam name = "TProperty"></typeparam>
        /// <param name = "target">The target.</param>
        /// <param name = "expression">The expression.</param>
        /// <param name = "value">The value.</param>
        /// <returns></returns>
        public static T With<T, TProperty>(this T target, Expression<Func<T, TProperty>> expression, TProperty value)
        {
            return target.Set(expression).To(value);
        }
    }
}