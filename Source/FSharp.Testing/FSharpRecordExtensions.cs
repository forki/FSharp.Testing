using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.FSharp.Core;
using Microsoft.FSharp.Reflection;

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
        /// Copies the record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record">The record.</param>
        /// <returns></returns>
        public static T CopyRecord<T>(this T record)
        {
            var values = FSharpValue.GetRecordFields(record, FSharpOption<BindingFlags>.None);
            return (T) FSharpValue.MakeRecord(typeof (T), values, FSharpOption<BindingFlags>.None);
        }

        /// <summary>
        ///   Extracts the property name and creates a targetInformation with target object and backing field reference.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <typeparam name = "TProperty"></typeparam>
        /// <param name = "target">The target.</param>
        /// <param name = "expression">The expression.</param>
        /// <returns></returns>
        public static TargetInformation<T> Set<T, TProperty>(this T target, Expression<Func<T, TProperty>> expression)
        {
            var propertyName = expression.GetPropertyName();
            var field = GetBackingField<T>(propertyName);

            return new TargetInformation<T>(target, field);
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
        ///   Takes a targetInformation with target object and backing field reference and sets the given value on it.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <typeparam name = "TProperty"></typeparam>
        /// <param name = "targetInformation">The targetInformation.</param>
        /// <param name = "value">The value.</param>
        /// <returns></returns>
        public static T To<T, TProperty>(this TargetInformation<T> targetInformation, TProperty value)
        {
            targetInformation.BackingField.SetValue(targetInformation.Target, value);
            return targetInformation.Target;
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

        /// <summary>
        ///   Gets the record fields.
        /// </summary>
        /// <param name = "type">The record type.</param>
        /// <returns></returns>
        public static PropertyInfo[] GetRecordFields(this Type type)
        {
            return FSharpType.GetRecordFields(type, null);
        }
    }
}