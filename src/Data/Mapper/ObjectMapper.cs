using System;
using System.Linq;
using System.Reflection;

namespace PipServices.Commons.Data.Mapper
{
    public sealed class ObjectMapper : IObjectMapper
    {
        private readonly IObjectMapperStrategy _strategy;

        internal ObjectMapper(IObjectMapperStrategy strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            _strategy = strategy;
        }

        public TT Transfer<TS, TT>(TS source)
            where TS : class
            where TT : class
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            var target = Activator.CreateInstance<TT>();

            var propertyInfosSource = source.GetType().GetRuntimeProperties().ToArray();
            var propertyInfosTarget = target.GetType().GetRuntimeProperties().ToArray();

            foreach (var propertyInfoSource in propertyInfosSource)
            {
                foreach (var propertyInfoTarget in propertyInfosTarget)
                {
                    if (propertyInfoSource.Name != propertyInfoTarget.Name)
                        continue;

                    _strategy.Transfer(this, source, target, propertyInfoSource, propertyInfoTarget);

                    break;
                }
            }

            return target;
        }

        public static TT MapTo<TT>(object source)
            where TT : class
        {
            if (source == null)
                return null;

            var mapper = new ObjectMapper(new ObjectMapperStrategy());

            var sourceType = source.GetType();

            var methodInfo = mapper.GetType().GetTypeInfo().GetMethod(nameof(mapper.Transfer));
            var genericMethodInfo = methodInfo.MakeGenericMethod(sourceType, typeof(TT));
            var result = genericMethodInfo.Invoke(mapper, new[] { source });

            return (TT)result;
        }
    }
}
