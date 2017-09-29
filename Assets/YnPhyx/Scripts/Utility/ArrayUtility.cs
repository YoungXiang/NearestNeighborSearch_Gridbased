using System;
using System.Collections;
using System.Collections.Generic;

namespace YnPhyx
{
    /// <summary>
    ///   <para>Helpers for builtin arrays ...</para>
    /// </summary>
    public sealed class ArrayUtility
    {
        public ArrayUtility()
        {
        }

        public static void Add<T>(ref T[] array, T item)
        {
            Array.Resize<T>(ref array, (int)array.Length + 1);
            array[(int)array.Length - 1] = item;
        }

        public static void AddRange<T>(ref T[] array, T[] items)
        {
            int length = (int)array.Length;
            Array.Resize<T>(ref array, (int)array.Length + (int)items.Length);
            for (int i = 0; i < (int)items.Length; i++)
            {
                array[length + i] = items[i];
            }
        }

        public static bool ArrayEquals<T>(T[] lhs, T[] rhs)
        {
            bool flag;
            if (lhs == null || rhs == null)
            {
                flag = lhs == rhs;
            }
            else if ((int)lhs.Length == (int)rhs.Length)
            {
                int num = 0;
                while (num < (int)lhs.Length)
                {
                    if (lhs[num].Equals(rhs[num]))
                    {
                        num++;
                    }
                    else
                    {
                        flag = false;
                        return flag;
                    }
                }
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public static bool ArrayReferenceEquals<T>(T[] lhs, T[] rhs)
        {
            bool flag;
            if (lhs == null || rhs == null)
            {
                flag = lhs == rhs;
            }
            else if ((int)lhs.Length == (int)rhs.Length)
            {
                int num = 0;
                while (num < (int)lhs.Length)
                {
                    if (object.ReferenceEquals(lhs[num], rhs[num]))
                    {
                        num++;
                    }
                    else
                    {
                        flag = false;
                        return flag;
                    }
                }
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public static void Clear<T>(ref T[] array)
        {
            Array.Clear(array, 0, (int)array.Length);
            Array.Resize<T>(ref array, 0);
        }

        public static bool Contains<T>(T[] array, T item)
        {
            return (new List<T>(array)).Contains(item);
        }

        public static T Find<T>(T[] array, Predicate<T> match)
        {
            return (new List<T>(array)).Find(match);
        }

        public static List<T> FindAll<T>(T[] array, Predicate<T> match)
        {
            return (new List<T>(array)).FindAll(match);
        }

        public static int FindIndex<T>(T[] array, Predicate<T> match)
        {
            return (new List<T>(array)).FindIndex(match);
        }

        public static int IndexOf<T>(T[] array, T value)
        {
            return (new List<T>(array)).IndexOf(value);
        }

        public static void Insert<T>(ref T[] array, int index, T item)
        {
            ArrayList arrayLists = new ArrayList();
            arrayLists.AddRange(array);
            arrayLists.Insert(index, item);
            array = arrayLists.ToArray(typeof(T)) as T[];
        }

        public static int LastIndexOf<T>(T[] array, T value)
        {
            return (new List<T>(array)).LastIndexOf(value);
        }

        public static void Remove<T>(ref T[] array, T item)
        {
            List<T> ts = new List<T>(array);
            ts.Remove(item);
            array = ts.ToArray();
        }

        public static void RemoveAt<T>(ref T[] array, int index)
        {
            List<T> ts = new List<T>(array);
            ts.RemoveAt(index);
            array = ts.ToArray();
        }
    }
}