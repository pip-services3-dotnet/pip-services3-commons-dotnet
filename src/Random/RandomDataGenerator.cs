using PipServices3.Commons.Convert;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PipServices3.Commons.Random
{
    public abstract class RandomDataGenerator<T>
    {
        public abstract T Create(DataReferences references = null);
        public abstract T Update(T item, DataReferences references = null);
        public abstract T Delete(T item, DataReferences references = null);

        public List<PropertyInfo> enums;

        public RandomDataGenerator()
        {
            enums = typeof(T).GetProperties().Where(x => typeof(Enum).IsAssignableFrom(x.PropertyType)).ToList();
        }

        public virtual T Clone(T item)
        {
            var temp = JsonConverter.ToJson(item);
            return JsonConverter.FromJson<T>(temp);
        }

        public virtual T Distort(T item)
        {
            if (enums.Count > 0)
            {
                if (RandomBoolean.Chance(1, 5))
                {
                    PropertyInfo prop = enums[RandomInteger.NextInteger(enums.Count)];
                    Array values = prop.PropertyType.GetEnumValues();
                    prop.SetValue(item, values.GetValue(RandomInteger.NextInteger(values.Length)));
                }
            }

            return item;
        }

        public List<T> CreateList(int minSize, int maxSize = 0, DataReferences references = null)
        {
            maxSize = Math.Max(minSize, maxSize);
            int size = RandomInteger.NextInteger(minSize, maxSize);
            List<T> items = new List<T>();

            for (int index = 0; index < size; index++)
            {
                items.Add(Create(references));
            }

            return items;
        }

        public List<T> AppendList(List<T> items, int minSize, int maxSize = 0, DataReferences references = null)
        {
            maxSize = Math.Max(minSize, maxSize);
            int size = RandomInteger.NextInteger(minSize, maxSize);

            for (int index = 0; index < size; index++)
            {
                var item = Create();
                items.Add(item);
            }

            return items;
        }

        public List<T> UpdateList(List<T> items, int minSize, int maxSize = 0, DataReferences references = null)
        {
            maxSize = Math.Max(minSize, maxSize);
            int size = RandomInteger.NextInteger(minSize, maxSize);

            for (int index = 0; items.Count > 0 && index < size; index++)
            {
                var item = items[RandomInteger.NextInteger(items.Count)];
                Update(item, references);
            }

            return items;
        }

        public List<T> ReduceList(List<T> items, int minSize, int maxSize = 0, DataReferences references = null)
        {
            maxSize = Math.Max(minSize, maxSize);
            int size = RandomInteger.NextInteger(minSize, maxSize);

            for (int index = 0; items.Count > 0 && index < size; index++)
            {
                var pos = RandomInteger.NextInteger(items.Count);
                var item = Delete(items[pos], references);
                if (item == null) items.RemoveAt(pos);
            }

            return items;
        }

        public List<T> ChangeList(List<T> items, int minSize, int maxSize = 0, DataReferences references = null)
        {
            maxSize = Math.Max(minSize, maxSize);
            int size = RandomInteger.NextInteger(minSize, maxSize);

            for (int index = 0; index < size; index++)
            {
                int choice = RandomInteger.NextInteger(5);
                if (choice == 1 && items.Count > 0)
                {
                    var pos = RandomInteger.NextInteger(items.Count);
                    var item = Delete(items[pos], references);
                    if (item == null) items.RemoveAt(pos);
                }
                else if (choice == 3)
                {
                    var item = Create();
                    items.Add(item);
                }
                else if (items.Count > 0)
                {
                    var item = items[RandomInteger.NextInteger(items.Count)];
                    Update(item, references);
                }
            }

            return items;
        }

        public List<T> DistortList(List<T> items, int minSize, int maxSize = 0, float probability = 1, DataReferences references = null)
        {
            maxSize = Math.Max(minSize, maxSize);
            int size = RandomInteger.NextInteger(minSize, maxSize);

            for (int index = 0; items.Count > 0 && index < size; index++)
            {
                if (RandomBoolean.Chance(probability * 100, 100))
                {
                    var itemIndex = RandomInteger.NextInteger(items.Count);
                    items[itemIndex] = Distort(items[itemIndex]);
                }
            }

            return items;
        }

    }
}
