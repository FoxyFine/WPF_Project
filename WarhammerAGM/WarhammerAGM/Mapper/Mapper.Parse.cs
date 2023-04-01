using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Mapping
{
    public static partial class Mapper
    {
        public static T Parse<T>(this string line, params string[] propertyNames)
            where T : new()
            => Parse<T>(line, '\t', propertyNames);

        public static T Parse<T>(this string line, char fieldSeparator, params string[] propertyNames)
            where T : new()
        {
            Type type = typeof(T);
            var propertyDatas = GetSetDatas(type);

            string[] props = line.Split(fieldSeparator, StringSplitOptions.TrimEntries);

            T item = new();

            for (int j = 0; j < props.Length; j++)
            {
                var name = propertyNames[j];
                var data = propertyDatas[name];
                var prop = props[j];
                data.SetValue(item, prop);
            }

            return item;
        }

        public static IEnumerable<T> LinesToItems<T>(this IEnumerable<string> lines, params string[] propertyNames)
            where T : new()
        {
            foreach (var line in lines)
            {
                yield return Parse<T>(line, propertyNames);
            }
        }

        public static IEnumerable<T> LinesToItems<T>(this IEnumerable<string> lines, char fieldSeparator, params string[] propertyNames)
            where T : new()
        {
            foreach (var line in lines)
            {
                yield return Parse<T>(line, fieldSeparator, propertyNames);
            }
        }

        public static T[] ParseToArray<T>(this string text, char fieldSeparator, char[] lineSeparators, params string[] propertyNames)
           where T : new()
        {
            var lines = text.Split(lineSeparators, StringSplitOptions.RemoveEmptyEntries);
            return LinesToItems<T>(lines, fieldSeparator, propertyNames).ToArray();
        }

        public static T[] ParseToArray<T>(this string text, char[] lineSeparators, params string[] propertyNames)
           where T : new()
        {
            var lines = text.Split(lineSeparators, StringSplitOptions.RemoveEmptyEntries);
            return LinesToItems<T>(lines, propertyNames).ToArray();
        }

        public static T[] ParseToArray<T>(this string text, char fleldSeparator, params string[] propertyNames)
            where T : new()
            => ParseToArray<T>(text, fleldSeparator, "\r\n".ToCharArray(), propertyNames);

        public static T[] ParseToArray<T>(this string text, params string[] propertyNames)
            where T : new()
            => ParseToArray<T>(text, "\r\n".ToCharArray(), propertyNames);

        private static IReadOnlyDictionary<string, PropertySetData> GetSetDatas(Type type)
        {
            if (!typeSetters.TryGetValue(type, out ReadOnlyDictionary<string, PropertySetData>? setDatas))
            {
                PropertyInfo[] propertiesInfo = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                Dictionary<string, PropertySetData> _converterSetters = new(propertiesInfo.Length);
                setDatas = new ReadOnlyDictionary<string, PropertySetData>(_converterSetters);

                var propertyDescriptors = TypeDescriptor.GetProperties(type);
                foreach (var property in propertiesInfo)
                {
                    PropertyDescriptor? propertyDescriptor = propertyDescriptors[property.Name];
                    if (property.CanWrite)
                    {
                        _converterSetters.Add(property.Name, new PropertySetData(property.Name, propertyDescriptors[property.Name]!, property.SetValue));
                    }
                }
                typeSetters.Add(type, setDatas);
            }
            return setDatas;
        }

        private readonly struct PropertySetData
        {
            public readonly string name;
            public readonly PropertyDescriptor propertyDescriptor;
            public readonly Action<object, object?> setter;

            public PropertySetData(string name, PropertyDescriptor propertyDescriptor, Action<object, object?> setter)
            {
                this.name = name;
                this.propertyDescriptor = propertyDescriptor;
                this.setter = setter;
            }

            private static readonly Action<PropertyDescriptor, object, EventArgs> onValueChanged =
                typeof(PropertyDescriptor).GetMethod("OnValueChanged", BindingFlags.NonPublic | BindingFlags.Instance)?
                .CreateDelegate<Action<PropertyDescriptor, object, EventArgs>>()
                ?? throw new Exception("Куда-то пропал метод OnValueChanged.");

            public void SetValue(object item, string value)
            {
                setter(item, propertyDescriptor.Converter.ConvertFromInvariantString(value));
                onValueChanged(propertyDescriptor, item, EventArgs.Empty);
            }
            public void SetValue(object item, object? value)
            {
                if (value is string text)
                {
                    SetValue(item, text);
                    return;
                }

                if (value is null || propertyDescriptor.PropertyType.IsAssignableFrom(value.GetType()))
                {
                    setter(item, value);
                }
                else
                {
                    setter(item, propertyDescriptor.Converter.ConvertFrom(value));
                }
                onValueChanged(propertyDescriptor, item, EventArgs.Empty);
            }
        }

        private static readonly Dictionary<Type, ReadOnlyDictionary<string, PropertySetData>> typeSetters = new();
    }
}
