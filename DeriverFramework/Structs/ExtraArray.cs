using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DF.Structs
{
    [Serializable]
    public class ExtraArray<T>
    {
        public Memory<T> Array { get; private set; }
        public int Count { get { return Array.Length; } }
        public ExtraArray(Memory<T> mem)
        {
            Array = mem;
        }
        public ExtraArray(T[] array)
        {
            Array = new Memory<T>(array);
        }
        public ExtraArray()
        {
            Array = new Memory<T>(System.Array.Empty<T>());
        }
        public static ExtraArray<T> Null;
        public static bool IsNull(ExtraArray<T> extraArray)
        {
            return (object)extraArray == (object)Null;
        }
        public bool IsNull()
        {
            return IsNull(this);
        }
        public void Add(T obj)
        {
            var array = Array.Span.ToArray();
            T[] newarray = new T[array.Length + 1];
            for (var i = 0; i != array.Length; i++)
            {
                newarray[i] = array[i];
            }
            newarray[newarray.Length - 1] = obj;
            Array = newarray.ToArray().AsMemory();
        }
        public void Remove(T obj)
        {
            var array = Array.Span.ToArray();
            Span<T> newarray = new Span<T>(new T[array.Length - 1], 0, array.Length - 1);
            sbyte detected = 0;
            for (var i = 0; i != newarray.Length; i++)
            {
                if ((object)array[i] == (object)obj)
                {
                    detected = 1;
                    continue;
                }
                newarray[i] = array[i + detected];
            }
            Array = newarray.ToArray().AsMemory();
        }
        public void RemoveAt(int index)
        {
            if (Correct(index) == 1)
            {
                var array = Array.Span.ToArray();
                Span<T> newarray = new Span<T>(new T[array.Length - 1], 0, array.Length - 1);
                sbyte detected = 0;
                for (var i = 0; i != array.Length; i++)
                {
                    if (i == index)
                    {
                        detected = -1;
                        continue;
                    }
                    newarray[i] = array[i + detected];
                }
                Array = newarray.ToArray().AsMemory();
            }
        }
        public void Clear()
        {
            Array = Memory<T>.Empty;
        }
        public T this[int index]
        {
            get
            {
                return Array.Span[index];
            }
            set
            {
                Replace(value, index);
            }
        }
        private void Replace(T obj, int index)
        {
            var array = Array.Span;
            array[index] = obj;
            Array = array.ToArray().AsMemory();
        }
        private byte Correct(int index)
        {
            bool r3 = index == Array.Length - 1 && index >= 0;
            if (r3)
                return 1;
            else
                return 0;
        }
    }
}
