using System.Collections.Generic;
using System.Text;

namespace PipServices.Commons.Data
{
    /// <summary>
    /// Projection parameters requried to retrieve custom resultsets from database. 
    /// The format is "Field1, Field2, InnerObject(Field1, Field2)"
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{System.String}" />
    public class ProjectionParams : List<string>
    { 
        public ProjectionParams()
        {
        }

        public ProjectionParams(string[] values)
        {
            if (values != null)
            {
                AddRange(values);
            }
        }

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

        public static ProjectionParams FromValue(object value)
        {
            if (value is ProjectionParams)
            {
                return (ProjectionParams)value;
            }

            var array = value != null ? AnyValueArray.FromValue(value) : new AnyValueArray();
            return new ProjectionParams(array);
        }

        public static ProjectionParams Parse(params string[] values)
        {
            var result = new ProjectionParams();

            foreach (var value in values)
            {
                ParseValue("", result, value);
            }

            return result;
        }

        private static void ParseValue(string prefix, ProjectionParams result, string value)
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
                switch (value[index])
                {
                    case '(':
                        if (openBracket == 0)
                        {
                            openBracketIndex = index;
                        }

                        openBracket++;
                        break;
                    case ')':
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
                                ParseValue(prefix, result, subValue);

                                subValue = value.Substring(closeBracketIndex + 1);
                                ParseValue(previousPrefix, result, subValue);
                                breakCycleRequired = true;
                            }
                        }
                        break;
                    case ',':
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
                                ParseValue(prefix, result, subValue);
                                breakCycleRequired = true;
                            }
                        }
                        break;
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