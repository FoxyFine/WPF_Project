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
        public static void CopyFrom<TTarget>(this TTarget target, in object source, params string[] propertyNames)
            => CopyFrom(target ?? throw new ArgumentNullException(nameof(target)),
                        source ?? throw new ArgumentNullException(nameof(source)),
                        GetСopyableProperties(target.GetType(), source.GetType()),
                        propertyNames);
        public static void CopyFrom<TTarget>(this TTarget target, in object source, string[] propertyNamesArray, params string[] propertyNames)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (propertyNamesArray is null)
                throw new ArgumentNullException(nameof(propertyNamesArray));

            propertyNames ??= Array.Empty<string>();
            if (propertyNamesArray.Length == 0 && propertyNames.Length == 0)
                throw new ArgumentNullException(nameof(propertyNames), "Должно быть задано хоть одно свойство.");

            Type targetType = target.GetType();
            Type sourceType = source.GetType();
            var setDatas = GetSetDatas(targetType);
            var getDatas = GetGetDatas(sourceType);



            foreach (var name in propertyNamesArray.Concat(propertyNames).Distinct())
            {
                var setData = setDatas[name];
                var getData = getDatas[name];

                var value = getData.GetValue(source, setData.propertyDescriptor.PropertyType);
                setData.SetValue(target, value);
            }
        }
        public static TTarget Create<TTarget>(this object source, params string[] propertyNames)
               where TTarget : new()
        {
            TTarget item = new();
            item.CopyFrom(source, propertyNames);
            return item;
        }
        public static TTarget Create<TTarget, TSource>(this TSource source, string[] propertyNamesArray, params string[] propertyNames)
               where TTarget : new()
        {
            TTarget item = new();
            item.CopyFrom(source, propertyNamesArray, propertyNames);
            return item;
        }

        public static string[] GetСopyableProperties(Type targetType, Type sourceType, bool isValueTypeOnly = true)
        {
            var setDatas = GetSetDatas(targetType);
            var getDatas = GetGetDatas(sourceType);

            var names = new List<string>(setDatas.Count);
            foreach (var setData in setDatas.Values)
            {
                if (!getDatas.TryGetValue(setData.name, out var getData))
                    continue;
                var targetPropertyType = setData.propertyDescriptor.PropertyType;
                if (!(targetPropertyType.IsValueType || targetPropertyType == typeof(string)) && isValueTypeOnly)
                {
                    continue;
                }
                names.Add(setData.name);
            }
            return names.ToArray();
        }

        private static IReadOnlyDictionary<string, PropertyGetData> GetGetDatas(Type type)
        {
            if (!typeGetters.TryGetValue(type, out var getDatas))
            {
                PropertyInfo[] propertiesInfo = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                Dictionary<string, PropertyGetData> _getDatas = new(propertiesInfo.Length);
                getDatas = new ReadOnlyDictionary<string, PropertyGetData>(_getDatas);

                var propertyDescriptors = TypeDescriptor.GetProperties(type);
                foreach (var property in propertiesInfo)
                {
                    PropertyDescriptor? propertyDescriptor = propertyDescriptors[property.Name];
                    _getDatas.Add(property.Name, new PropertyGetData(property.Name, propertyDescriptors[property.Name]?.Converter, property.GetValue));
                }
                typeGetters.Add(type, getDatas);
                _getDatas.TrimExcess();
            }
            return getDatas;

        }

        private readonly struct PropertyGetData
        {
            public readonly string name;
            public readonly TypeConverter? converter;
            public readonly Func<object, object?> getter;

            public PropertyGetData(string name, TypeConverter? converter, Func<object, object?> getter)
            {
                this.name = name;
                this.converter = converter;
                this.getter = getter;
            }
            public object? GetValue(object item, Type destinationType)
            {
                var get = getter(item);
                if (get == null) return null;
                var getType = get.GetType();
                if (destinationType.IsAssignableFrom(getType))
                    return get;
                if (converter is not null && converter.CanConvertTo(destinationType))
                {
                    return converter.ConvertTo(get, destinationType);
                }
                return get;
            }
        }

        //    private static readonly Dictionary<Type, ReadOnlyDictionary<string, PropertyData>> typeSetters = new();
        private static readonly Dictionary<Type, ReadOnlyDictionary<string, PropertyGetData>> typeGetters
            = new();
    }
}
