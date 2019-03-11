using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using PipServices3.Commons.Convert;

namespace PipServices3.Commons.Data.Mapper
{
    internal sealed class ObjectMapperStrategy : IObjectMapperStrategy
    {
        public void Transfer<TS, TT>(IObjectMapper mapper, TS objectSource, TT objectTarget,
            PropertyInfo propertyInfoSource, PropertyInfo propertyInfoTarget)
            where TS : class
            where TT : class
        {
            var propertyValueSource = propertyInfoSource.GetValue(objectSource);
            var propertyValueTarget = propertyInfoTarget.GetValue(objectTarget);

            if (propertyValueSource == null)
                return;

            var valueSourceType = propertyValueSource.GetType();
            var valueSourceTypeInfo = valueSourceType.GetTypeInfo();

            var valueTargetType = propertyInfoTarget.PropertyType;
            var valueTargetTypeInfo = valueTargetType.GetTypeInfo();

            if (valueSourceTypeInfo.IsClass && valueSourceType != typeof(string) && !valueSourceTypeInfo.IsArray &&
                !valueSourceTypeInfo.ImplementedInterfaces.Contains(typeof(IEnumerable)))
            {
                var methodInfo = mapper.GetType().GetTypeInfo().GetMethod(nameof(mapper.Transfer));
                var genericMethodInfo = methodInfo.MakeGenericMethod(valueSourceType, valueTargetType);
                var result = genericMethodInfo.Invoke(mapper, new[] { propertyValueSource });

                propertyInfoTarget.SetValue(objectTarget, result);

                return;
            }

            if (valueSourceType != typeof(string) &&
                (valueSourceTypeInfo.IsArray || valueSourceTypeInfo.ImplementedInterfaces.Contains(typeof(IEnumerable))))
            {
                    var source = (IEnumerable) propertyValueSource;

                    object firstEntry = null;
                    foreach (var item in source)
                    {
                        firstEntry = item;
                        break;
                    }

                    var entrySourceType = firstEntry?.GetType();

                    if (entrySourceType != null)
                    {
                        var entrySourceTypeInfo = entrySourceType.GetTypeInfo();
                        var entryTargetType = propertyValueTarget.GetType().GetTypeInfo().GetGenericArguments()[0];

                        if (entrySourceTypeInfo.IsClass)
                        {
                            var methodParameters = new[]
                            {
                                entryTargetType
                            };

                            var method = propertyValueTarget.GetType().GetRuntimeMethod("Add", methodParameters);

                            foreach (var entrySource in source)
                            {
                                if (entrySource == null || method == null)
                                    continue;

                                if (TypeConverter.IsPrimitiveType(entrySource))
                                {
                                    var parameters = new[]
                                    {
                                        entrySource
                                    };

                                    method.Invoke(propertyValueTarget, parameters);
                                }
                                else
                                {
                                    var methodInfo = mapper.GetType().GetTypeInfo().GetMethod(nameof(mapper.Transfer));
                                    var genericMethodInfo =
                                        methodInfo.MakeGenericMethod(entrySourceType, entryTargetType);
                                    var entryTarget = genericMethodInfo.Invoke(mapper, new[] {entrySource});

                                    var parameters = new[]
                                    {
                                        entryTarget
                                    };

                                    method.Invoke(propertyValueTarget, parameters);
                                }
                            }
                        }
                        else
                        {
                            var methodParameters = new List<Type>();
                            
                            if (valueSourceTypeInfo.ImplementedInterfaces.Contains(typeof(IDictionary)))
                            {
                                Type[] arguments = propertyValueSource.GetType().GetGenericArguments();
                                
                                methodParameters.Add(arguments[0]);
                                methodParameters.Add(arguments[1]);
                            }
                            else
                            {
                                methodParameters.Add(entrySourceType);
                            }

                            var method = propertyValueTarget.GetType().GetRuntimeMethod("Add", methodParameters.ToArray());

                            foreach (dynamic entrySource in source)
                            {
                                var parameters = new List<object>();
                                
                                if (valueSourceTypeInfo.ImplementedInterfaces.Contains(typeof(IDictionary)))
                                {
                                    parameters.Add(entrySource.Key);
                                    parameters.Add(entrySource.Value);
                                }
                                else
                                {
                                    parameters.Add(entrySource);
                                }

                                method.Invoke(propertyValueTarget, parameters.ToArray());
                            }
                        }

                        return;
                    }
                    else
                    {
                        return;
                    }
            }

            propertyInfoTarget.SetValue(objectTarget, propertyValueSource);
        }
    }
}