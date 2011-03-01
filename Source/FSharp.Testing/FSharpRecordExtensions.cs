using System;
using System.Linq;
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
        ///   Copies the record.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "record">The record.</param>
        /// <returns></returns>
        public static T CopyRecord<T>(this T record)
        {
            return CreateModifiedCopy<T, object>(record, null, null);
        }

        /// <summary>
        ///   Creates a modified copy of the original object.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <typeparam name = "TProperty">The type of the property.</typeparam>
        /// <param name = "record">The record.</param>
        /// <param name = "propertyName">Name of the property.</param>
        /// <param name = "value">The value.</param>
        /// <returns></returns>
        static T CreateModifiedCopy<T, TProperty>(T record, string propertyName, TProperty value)
        {
            var originalValues =
                FSharpType.GetRecordFields(typeof (T), null)
                    .Select(p => Tuple.Create(p, FSharpValue.GetRecordField(record, p)));

            var values =
                originalValues
                    .Select(t => t.Item1.Name == propertyName ? value : t.Item2)
                    .ToArray();

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
            return new TargetInformation<T>(target, expression.GetPropertyName());
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
            return CreateModifiedCopy(targetInformation.Target, targetInformation.PropertyName, value);
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