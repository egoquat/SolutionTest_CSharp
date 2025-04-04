﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;


namespace Common
{
    public static class IContainerHelper
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> container)
        {
            return null == container || 0 >= container.Count();
        }

        public static int CountOrDefault<T>(this IEnumerable<T> container)
        {
            if (true == container.IsNullOrEmpty())
                return 0;

            return container.Count();
        }

        public static bool IsRangeIn<T>(this IEnumerable<T> container, int idx)
        {
            if (true == container.IsNullOrEmpty())
                return false;

            return 0 <= idx && idx < container.Count();
        }

        public static T GetValueSafe<T>(this IEnumerable<T> container, int idx)
        {
            if (true == container.IsNullOrEmpty())
                return default(T);

            return container.ElementAtOrDefault(idx);
        }

        public static bool IsContainsSafe<T>(this IEnumerable<T> container, T t)
        {
            if (container.IsNullOrEmpty())
                return false;
            return container.Contains(t);
        }

        public static void Each<T>(this IEnumerable<T> container, Action<T> action)
        {
            if (true == container.IsNullOrEmpty())
                return;

            for(int i = 0; i < container.Count(); ++ i)
            {
                T t = container.ElementAt(i);
                if (null != t)
                { action(t); }
            }
        }

        public static T[] ToArrayOrDefault<T>(this IEnumerable<T> container)
        {
            if (container.IsNullOrEmpty())
                return null;

            return container.ToArray();
        }

        public static List<T> ToListOrDefault<T>(this IEnumerable<T> container)
        {
            if( container.IsNullOrEmpty() )
                return null;

            return container.ToList();
        }

        public static V GetOrDefault<T, V>(this IDictionary<T, V> container, T key)
        {
            if (container.IsNullOrEmpty() || false == container.ContainsKey(key))
                return default(V);

            return container[key];
        }

        public static bool IsNullOrEmpty<T, V>(this IDictionary<T, V> container)
        {
            return null == container || 0 >= container.Count();
        }

        public static bool IsRangeIn<T, V>(this IDictionary<T, V> container, int idx)
        {
            if (true == container.IsNullOrEmpty())
                return false;

            return 0 <= idx && idx < container.Count();
        }

        public static bool AddNotOverlap<T, V>(this IDictionary<T, V> container, T t, V v)
        {
            if (null == container || (null != container && true == container.ContainsKey(t)))
                return false;

            container.Add(t, v);
            return true;
        }

        public static bool AddOrUpdate<T, V>(this IDictionary<T, V> container, T t, V v)
        {
            if (null == container)
            {
                return false;
            }

            if (true == container.ContainsKey(t))
            {
                container[t] = v;
            }
            else
            {
                container.Add(t, v);
                return true;
            }
            return false;
        }

        public static bool ContainsKeySafe<T, V>(this IDictionary<T, V> container, T key)
        {
            if (true == container.IsNullOrEmpty())
                return false;

            if (null == key)
                return false;

            return container.ContainsKey(key);
        }

        public static bool ContainsValueSafe<T, V>(this IDictionary<T, V> container, V value)
        {
            if (true == container.IsNullOrEmpty())
                return false;

            if (null == value)
                return false;

            return container.Values.Contains(value);
        }

        public static V GetValueSafe<T, V>(this IDictionary<T, V> container, T key)
        {
            if (true == container.IsNullOrEmpty() || null == key || false == container.ContainsKey(key))
                return default(V);

            return container[key];
        }

        public static T GetKeySafe<T, V>(this IDictionary<T, V> container, V value)
        {
            if (true == container.IsNullOrEmpty() || null == value || false == container.ContainsValueSafe(value))
                return default(T);

            T k = default(T);
            foreach(var kvp in container)
            {
                if (kvp.Value.Equals(value))
                {
                    k = kvp.Key;
                    break;
                }
            }

            return k;
        }

        public static void RemoveAll<T, V>(this IDictionary<T, V> container, Func<KeyValuePair<T, V>, bool> condition)
        {
            if (true == container.IsNullOrEmpty())
                return;
        
            IEnumerable<KeyValuePair<T, V>> removes = container.Where(k => { return condition(k); }).Select(k => k);
            if (true == removes.IsNullOrEmpty())
                return;

            //removes.Each(r=> { container.Remove(r); });
            for(int i = 0; i < removes.Count(); ++i)
            {
                container.Remove(removes.ElementAt(i));
            }
        }

        public static void RemoveFromValue<T, V>(this IDictionary<T, V> container, V value)
        {
            if (true == container.IsNullOrEmpty())
                return;

            KeyValuePair<T, V>? kvp = null;
            foreach(var k in container)
            {
                if (k.Value.Equals(value))
                {
                    kvp = k;
                    break;
                }
            }

            if (kvp.HasValue)
                container.Remove(kvp.Value.Key);
        }

        public static int IndexOf<TKey>(this IEnumerable<TKey> container, TKey key, int defaultIndex = -1)
        {
            if (true == container.IsNullOrEmpty())
                return defaultIndex;
        
            int sequenceIndex = -1;
            int seq = 0;
            foreach(TKey t in container)
            {
                if (t.Equals(key))
                {
                    sequenceIndex = seq;
                    break;
                }
                seq++;
            }

            if (-1 != sequenceIndex)
                return sequenceIndex;
            else
                return defaultIndex;
        }

        public static bool AddNotOverlap<T>(this IList<T> container, T t)
        {
            if (null == container)
            {
                return false;
            }
            if (false == container.Contains(t))
            {
                container.Add(t);
                return true;
            }
            return false;
        }

        public static bool AddOrUpdate<T>(this IList<T> container, T t)
        {
            if( null == container )
            {
                return false;
            }
            if (true == container.Any(item=>item.Equals(t)))
            {
                container.RemoveAll(item=>item.Equals(t));
            }
            container.Add(t);
            return true;
        }

        public static bool InsertOrAdd<T>(this IList<T> container, int index, T t)
        {
            if (null == container || -1 >= index)
            {
                GLog.LogError("(null == container || -1 >= index)/InsertOrAdd");
                return false;
            }
            if (index >= container.Count)
            {
                container.Add(t);
            }
            else
            {
                container.Insert(index, t);
            }
            return true;
        }

        public static bool AddRangeSafe<T>(this List<T> container, IEnumerable<T> t)
        {
            if( null == container || t.IsNullOrEmpty())
            {
                return false;
            }
            
            container.AddRange(t);
            return true;
        }

        public static bool AddRangeNotOverlap<T>(this List<T> container, IEnumerable<T> otherContainer)
        {
            if( null == container || otherContainer.IsNullOrEmpty() )
            {
                return false;
            }

            bool isAddedAny = false;
            for(int i = 0; i < otherContainer.Count(); i++)
            {
                T t = otherContainer.ElementAt(i);
                if (container.Contains(t))
                    continue;
                container.Add(t);
                isAddedAny = true;
            }

            return isAddedAny;
        }

        public static void RemoveAll<T>(this IList<T> container, Func<T, bool> condition)
        {
            if (true == container.IsNullOrEmpty())
            {
                return;
            }
            IEnumerable<T> removeList = container.Where(t => condition(t));
            if (true == removeList.IsNullOrEmpty())
                return;
            removeList.Each(r=> container.Remove(r) );
        }

        public static void RemoveAll<T>(this IList<T> container, IEnumerable<T> otherContainer)
        {
            if (true == container.IsNullOrEmpty() || otherContainer.IsNullOrEmpty())
            {
                return;
            }
            container.RemoveAll(i=> { return otherContainer.Contains(i); });
        }

        public static void EachEnum<T>(Action<T> actionIteration)
        {
            Type genericType = typeof(T);

            if (false == genericType.IsEnum)
            {
                return;
            }

            Array enumValues = Enum.GetValues(genericType);
            if (null == enumValues || 0 >= enumValues.Length)
            {
                return;
            }

            foreach (object enumObj in enumValues)
            {
                object numObjParsed = Enum.Parse(typeof(T), enumObj.ToString());
                if (false == numObjParsed is Enum)
                {
                    return;
                }
            
                actionIteration((T)numObjParsed);
            }
        }
        public static bool Contains(this XmlAttributeCollection container, Func<XmlAttribute, bool> condition)
        {
            if (null == container || 0 >= container.Count)
            {
                return false;
            }

            foreach (XmlAttribute a in container)
            {
                if (true == condition(a))
                {
                    return true;
                }
            }

            return false;
        }
        public static bool Contains(this XmlAttributeCollection container, string name)
        {
            if (null == container || 0 >= container.Count)
            {
                return false;
            }

            foreach (XmlAttribute a in container)
            {
                if (true == a.Name.IsSame(name, false))
                {
                    return true;
                }
            }

            return false;
        }
        public static bool Contains(this XmlAttributeCollection container, Type enumType)
        {
            if (null == container || 0 >= container.Count)
            {
                return false;
            }

            Array enumValues = enumType.GetEnumValues();
            if (enumValues.Length <= 0)
            {
                return false;
            }

            List<string> enumList = new List<string>();
            foreach (object e in enumValues)
            {
                enumList.Add(e.ToString());
            }

            foreach (XmlAttribute a in container)
            {
                if (true == enumList.Any(e => { return a.Name.IsSame(e, false); }))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsNullOrEmpty<T>(this string s, bool isTrim = false)
        {
            return (false == isTrim && (null == s || string.IsNullOrEmpty(s)))
                || (true == isTrim && (null == s || string.IsNullOrEmpty(s) || string.IsNullOrEmpty(s.Trim())));
        }

        public static bool IsNullOrEmptyString(this string s)
        {
            if (s.IsNullOrEmpty() || s.Trim().IsNullOrEmpty())
                return true;

            return false;
        }

        public static bool IsSameNullable(this string text, string text2, bool isIgnoreCase = false)
        {
            if (true == text.IsNullOrEmpty() && true == text2.IsNullOrEmpty())
            {
                return true;
            }

            return 0 == string.Compare(text, text2, isIgnoreCase);
        }

        public static bool IsSame(this string text, string text2, bool isIgnoreCase = false)
        {
            if (true == text.IsNullOrEmpty() || true == text2.IsNullOrEmpty())
            {
                return false;
            }

            return 0 == string.Compare(text, text2, isIgnoreCase);
        }

        public static bool ContainsSafe(this string t1, string t2, bool isIgnoreCase = true)
        {
            if( true == t1.IsNullOrEmpty() || true == t2.IsNullOrEmpty() )
            {
                return false;
            }

            return (isIgnoreCase && (t1.Contains(t2) || t1.ToLower().Contains(t2.ToLower()))) || (false == isIgnoreCase && t1.Contains(t2));
        }

        public static bool IsAlphabet(this string t)
        {
            if (true == t.IsNullOrEmpty())
            {
                return false;
            }

            Regex conditionRegex = new Regex(@"[a-zA-Z]");
            return conditionRegex.IsMatch(t);
        }

        public static void ClearSafely<T>(this IList<T> container)
        {
            if (null == container)
                return;

            container.Clear();
        }

        public static void DequeueAll<T>(this Queue<T> queue, Action<T> action)
        {
            if (queue.IsNullOrEmpty())
                return;

            while(false == queue.IsNullOrEmpty())
            {
                T q = queue.Dequeue();
                if (null != q)
                {
                    action(q);
                }
            }
        }
    }
}