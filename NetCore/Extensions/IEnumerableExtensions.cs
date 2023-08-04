using System.Reflection;

namespace NetCore
{
    /// <summary>
    /// extension methods for IEnumerable
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Enumerable run foreach
        /// </summary>
        /// <param name="enumeration">Enumerable object</param>
        /// <param name="action">execute function</param>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        /// <summary>
        /// Enumerable run Except with Generic
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static IEnumerable<T> ExceptList<T>(this IEnumerable<T> list, IEnumerable<T> second)
        {
            return list.Except(second, new CommonComparer<T>());
        }

        /// <summary>
        /// Enumerable run Except
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="list"></param>
        /// <param name="second"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Except<T, TValue>(this IEnumerable<T> list, IEnumerable<T> second, Func<T, TValue> selector)
        {
            return list.Except(second, new CommonSelectorComparer<T, TValue>(selector));
        }


        public class CommonComparer<T> : IEqualityComparer<T>
        {

            private PropertyInfo[] properties = null;

            public CommonComparer()
            {
            }

            public bool Equals(T x, T y)
            {
                if (properties == null)
                    properties = x.GetType().GetProperties();
                return properties.Aggregate(true, (result, item) =>
                {
                    return result && item.GetValue(x).Equals(item.GetValue(y));
                });
            }

            public int GetHashCode(T obj)
            {
                if (properties == null)
                    properties = obj.GetType().GetProperties();
                return properties.Aggregate(0, (result, item) =>
                {
                    return result ^ item.GetValue(obj).GetHashCode();
                });
            }
        }


        class CommonSelectorComparer<T, TValue> : IEqualityComparer<T>
        {
            public Func<T, TValue> selector;

            public CommonSelectorComparer(Func<T, TValue> selector)
            {
                this.selector = selector;
            }

            public CommonSelectorComparer()
            {
            }

            public bool Equals(T x, T y)
            {
                var a = selector(x);
                var b = selector(y);
                return a.Equals(b);
            }

            public int GetHashCode(T obj)
            {
                var v = this.selector(obj);
                return v.GetHashCode();
            }
        }
    }
}

