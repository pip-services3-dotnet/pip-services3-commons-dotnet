using System.Collections.Generic;
using System.Text;

namespace PipServices3.Commons.Data
{
    /// <summary>
    /// Defines projection parameters with list if fields to include into query results.
    /// 
    /// The parameters support two formats: dot format and nested format.
    /// 
    /// The dot format is the standard way to define included fields and subfields using
    /// dot object notation: <c>"field1,field2.field21,field2.field22.field221"</c>
    /// 
    /// As alternative the nested format offers a more compact representation:
    /// <c>"field1,field2(field21,field22(field221))"</c>
    /// </summary>
    /// <example>
    /// <code>
    /// var filter = FilterParams.FromTuples("type", "Type1");
    /// var paging = new PagingParams(0, 100);
    /// var projection = ProjectionParams.fromString("field1,field2(field21,field22)")
    /// 
    /// myDataClient.GetDataByFilter(filter, paging, projection);
    /// </code>
    /// </example>
    public class ProjectionParams : List<string>
    {
        private static char DefaultDelimiter = ',';

        /// <summary>
        /// Creates a new instance of the projection parameters.
        /// </summary>
        public ProjectionParams()
        {
        }

        /// <summary>
        /// Creates a new instance of the projection parameters and assigns its value.
        /// </summary>
        /// <param name="values">(optional) values to initialize this object.</param>
        public ProjectionParams(string[] values)
        {
            if (values != null)
            {
                AddRange(values);
            }
        }

        /// <summary>
        /// Creates a new instance of the projection parameters and assigns its value.
        /// </summary>
        /// <param name="values">(optional) values to initialize this object.</param>
        public ProjectionParams(AnyValueArray array)
        {
            if (array == null)
            {
                return;
            }

            for (int index = 0; index < array.Count; index++)
            {
                var value = array.GetAsString(index);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Add(value);
                }
            }
        }

        /// <summary>
        /// Converts specified value into ProjectionParams.
        /// </summary>
        /// <param name="value">value to be converted</param>
        /// <returns>a newly created ProjectionParams.</returns>
        /// See <see cref="AnyValueArray.FromValue(object)"/>
        public static ProjectionParams FromValue(object value)
        {
            if (value is ProjectionParams)
            {
                return (ProjectionParams)value;
            }

            var array = value != null ? AnyValueArray.FromValue(value) : new AnyValueArray();
            return new ProjectionParams(array);
        }

        /// <summary>
        /// Parses comma-separated list of projection fields.
        /// </summary>
        /// <param name="values">one or more comma-separated lists of projection fields</param>
        /// <returns>a newly created ProjectionParams.</returns>
        public static ProjectionParams FromValues(params string[] values)
        {
            return FromValues(DefaultDelimiter, values);
        }

        /// <summary>
        /// Parses comma-separated list of projection fields.
        /// </summary>
        /// <param name="delimiter">a certain type of delimiter</param>
        /// <param name="values">one or more comma-separated lists of projection fields</param>
        /// <returns>a newly created ProjectionParams.</returns>
        public static ProjectionParams FromValues(char delimiter, params string[] values)
        {
            return new ProjectionParams(Parse(delimiter, values));
        }

        /// <summary>
        /// Gets a string representation of the object.
        /// The result is a comma-separated list of projection fields
        /// "field1,field2.field21,field2.field22.field221"
        /// </summary>
        /// <returns>a string representation of the object.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var index = 0; index < Count; index++)
            {
                if (index > 0)
                {
                    builder.Append(',');
                }

                builder.Append(base[index]);
            }
            return builder.ToString();
        }

        private static string[] Parse(char delimiter, string[] values)
        {
            var result = new List<string>();
            var prefix = string.Empty;

            foreach (var value in values)
            {
                ParseValue(prefix, result, value.Trim(), delimiter);
            }

            return result.ToArray();
        }

        private static void ParseValue(string prefix, List<string> result, string value, char delimiter)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.Trim();
            }

            var openBracket = 0;
            var openBracketIndex = -1;
            var closeBracketIndex = -1;
            var commaIndex = -1;

            var breakCycleRequired = false;
            for (var index = 0; index < value.Length; index++)
            {
                var valueChar = value[index];

                if (valueChar.Equals('('))
                {
                    if (openBracket == 0)
                    {
                        openBracketIndex = index;
                    }

                    openBracket++;
                }
                else if (valueChar.Equals(')'))
                {
                    openBracket--;

                    if (openBracket == 0)
                    {
                        closeBracketIndex = index;

                        if (openBracketIndex >= 0 && closeBracketIndex > 0)
                        {
                            var previousPrefix = prefix;

                            if (!string.IsNullOrWhiteSpace(prefix))
                            {
                                prefix = $"{prefix}.{value.Substring(0, openBracketIndex)}";
                            }
                            else
                            {
                                prefix = $"{value.Substring(0, openBracketIndex)}";
                            }

                            var subValue = value.Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1);
                            ParseValue(prefix, result, subValue, delimiter);

                            subValue = value.Substring(closeBracketIndex + 1);
                            ParseValue(previousPrefix, result, subValue, delimiter);
                            breakCycleRequired = true;
                        }
                    }
                }
                else if (valueChar.Equals(delimiter))
                {
                    if (openBracket == 0)
                    {
                        commaIndex = index;

                        var subValue = value.Substring(0, commaIndex);

                        if (!string.IsNullOrWhiteSpace(subValue))
                        {
                            if (!string.IsNullOrWhiteSpace(prefix))
                            {
                                result.Add($"{prefix}.{subValue}");
                            }
                            else
                            {
                                result.Add(subValue);
                            }
                        }

                        subValue = value.Substring(commaIndex + 1);

                        if (!string.IsNullOrWhiteSpace(subValue))
                        {
                            ParseValue(prefix, result, subValue, delimiter);
                            breakCycleRequired = true;
                        }
                    }
                }

                if (breakCycleRequired)
                {
                    break;
                }
            }

            if (!string.IsNullOrWhiteSpace(value) && openBracketIndex == -1 && commaIndex == -1)
            {
                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    result.Add($"{prefix}.{value}");
                }
                else
                {
                    result.Add(value);
                }
            }
        }
    }
}