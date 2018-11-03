using System.Dynamic;
using System.Collections.Generic;

namespace PipServices3.Commons.Data
{
    public static class ProjectionExtensions
    {
        public static object FromObject(this ProjectionParams projectionParams, ExpandoObject @object)
        {
            var result = new ExpandoObject();
            var resultMap = (IDictionary<string, object>)result;

            var objectMap = (IDictionary<string, object>)@object;

            foreach (var keyValuePair in objectMap)
            {
                resultMap.Add(keyValuePair);
            }

            RemoveKeysFromExpandoObject(result, projectionParams);

            ProcessProjectionParams(result, projectionParams);

            return result;
        }

        public static object FromObject(this ProjectionParams projectionParams, object @object)
        {
            if (@object is ExpandoObject)
            {
                return projectionParams.FromObject(@object as ExpandoObject);
            }

            return @object;
        }

        private static void ProcessProjectionParams(ExpandoObject expandoObject, ProjectionParams projectionParams, int depth = 0, string parentProjection = "")
        {
            var expandoObjectMap = (IDictionary<string, object>)expandoObject;

            foreach (var projection in projectionParams)
            {
                var propertyNames = projection.Split('.');

                if (propertyNames.Length == 1)
                {
                    continue;
                }

                var propertyName = propertyNames[0];

                if (!expandoObjectMap.ContainsKey(propertyName))
                {
                    continue;
                }

                var propertyValue = expandoObjectMap[propertyName];
                var subPropertyName = projection.Substring(propertyName.Length + 1);
                var subProjectionParams = ProjectionParams.FromValues(subPropertyName);

                if (propertyValue is ExpandoObject)
                {
                    RemoveKeysFromExpandoObject(propertyValue as ExpandoObject, projectionParams, depth + 1, propertyName);

                    ProcessProjectionParams(propertyValue as ExpandoObject, subProjectionParams, depth + 1, propertyName);
                }
                else if (propertyValue is List<object>)
                {
                    foreach (var value in propertyValue as List<object>)
                    {
                        if (value is ExpandoObject)
                        {
                            RemoveKeysFromExpandoObject(value as ExpandoObject, projectionParams, depth + 1, propertyName);

                            ProcessProjectionParams(value as ExpandoObject, subProjectionParams, depth + 1, propertyName);
                        }
                    }
                }
            }
        }

        private static void RemoveKeysFromExpandoObject(IDictionary<string, object> expandoObjectMap, ProjectionParams projectionParams, int depth = 0, string parentProjection = "")
        {
            var projectionKeys = GetProjectionKeys(projectionParams, depth, parentProjection);
            var expandoObjectMapKeys = new List<string>(expandoObjectMap.Keys);

            foreach (var key in expandoObjectMapKeys)
            {
                if (!projectionKeys.Contains(key))
                {
                    expandoObjectMap.Remove(key);
                }
            }
        }

        private static IList<string> GetProjectionKeys(ProjectionParams projectionParams, int depth, string parentProjection)
        {
            var result = new List<string>();

            foreach (var projection in projectionParams)
            {
                var propertyNames = projection.Split('.');

                if (propertyNames.Length <= depth)
                {
                    continue;
                }

                var propertyName = propertyNames[depth];

                if (string.IsNullOrWhiteSpace(parentProjection) || parentProjection.Equals(propertyNames[depth - 1]))
                {
                    if (!result.Contains(propertyName))
                    {
                        result.Add(propertyName);
                    }
                }
            }

            return result;
        }
    }

}