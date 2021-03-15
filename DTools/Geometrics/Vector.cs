using DTools.Diagnostics;
using DTools.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DTools.Geometrics
{
    public struct Vector<T> : IEnumerable<T>
    {
        public Type Type { get; private set; }
        private readonly T[] vector;
        public int Dimension { get; private set; }
        public static Vector<T> Zero(int dimension)
        {
            return new Vector<T>(dimension);
        }
        public static Vector<T> Create(int dim, T value)
        {
            T[] vec = new T[dim];
            for (var i = 0; i != dim; i++)
                vec[i] = value;
            return new Vector<T>(vec);
        }
        public Vector(Vector<T> vec)
        {
            Type = typeof(T);
            Dimension = vec.Dimension;
            vector = vec.vector;
        }
        public Vector(int dimension)
        {
            Type = typeof(T);
            Dimension = dimension;
            vector = new T[Dimension];
        }
        public Vector(params T[] values)
        {
            Type = typeof(T);
            Dimension = values.Length;
            vector = values;
        }
        public override bool Equals(object obj)
        {
            return obj is Vector<T> vector &&
                   EqualityComparer<Type>.Default.Equals(Type, vector.Type) &&
                   EqualityComparer<T[]>.Default.Equals(this.vector, vector.vector) &&
                   Dimension == vector.Dimension;
        }

        public override int GetHashCode()
        {
            int hashCode = -985006958;
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(Type);
            hashCode = hashCode * -1521134295 + EqualityComparer<T[]>.Default.GetHashCode(vector);
            hashCode = hashCode * -1521134295 + Dimension.GetHashCode();
            return hashCode;
        }
        public override string ToString()
        {
            var str = "";
            str += $"Dim: {Dimension} ";
            str += "{ ";
            for (var i = 0; i != Dimension; i++)
            {
                str += $"{this[i]}, ";
            }
            str += "}";
            return str;
        }
        public T this[int index]
        {
            get
            {
                return (index >= 0 && index < vector.Length) ? vector[index] : throw new Exception($"Unknown index {index} of {vector.Length}");
            }
            set
            {
                vector[index] = (index >= 0 && index < vector.Length) ? value : throw new Exception($"Unknown index {index} of {vector.Length}");
            }
        }
        #region Delegates
        public delegate Vector<T> VectorAndVector(Vector<T> left, Vector<T> right);
        public delegate Vector<T> VecterAndFloat(Vector<T> left, float right);
        public delegate Vector<T> VecterAndDouble(Vector<T> left, double right);
        public delegate Vector<T> VecterAndDecimal(Vector<T> left, decimal right);
        #endregion
        #region OperatorsFrontEnd
        public static Vector<T> operator +(Vector<T> left, Vector<T> right)
        {
            //return VectorAdd(left, right);
            throw new NotImplementedException();
        }
        public static Vector<T> operator -(Vector<T> left, Vector<T> right)
        {
            throw new NotImplementedException();
            //return VectorSubstract(left, right);
        }
        #endregion
        #region OperatorsBackEnd
        /*
        private static Vector<T> VectorAdd(Vector<T> left, Vector<T> right)
        {
            string TSource = JitTools.TypeToCsharpSource(left.Type);
            string code = string.Concat
            ($@"(Vector<{TSource}> left, Vector<{TSource}> right) =>",
                "{", 
                $@"
                    var vec = new Vector<{TSource}>(left);
                    if (vec.Dimension != right.Dimension)
                        throw new ArgumentException();
                    if (vec.Type != right.Type)
                        throw new Exception({JitTools.StringToCsharpCode("Type not equals.")});
                ",
            @"
                    for (var i = 0; i != vec.Dimension; i++)
                    {
                        vec[i] += right[i];
                    }
                    return vec;
                }
                "
            );
            var dlg = Compiller.Compile<VectorAndVector>(code, "DTools.Geometrics");
            return dlg(left, right);
        }
        private static Vector<T> VectorSubstract(Vector<T> left, Vector<T> right)
        {
            string TSource = JitTools.TypeToCsharpSource(left.Type);
            string code = string.Concat
            ($@"(Vector<{TSource}> left, Vector<{TSource}> right) =>",
                "{",
                $@"
                    var vec = new Vector<{TSource}>(left);
                    if (vec.Dimension != right.Dimension)
                        throw new ArgumentException();
                    if (vec.Type != right.Type)
                        throw new Exception({JitTools.StringToCsharpCode("Type not equals.")});
                ",
            @"
                    for (var i = 0; i != vec.Dimension; i++)
                    {
                        vec[i] -= right[i];
                    }
                    return vec;
                }
                "
            );
            var dlg = Compiller.Compile<VectorAndVector>(code, "DTools.Geometrics");
            return dlg(left, right);
        }
        */
        #endregion
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)vector).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return vector.GetEnumerator();
        }
    }
}
