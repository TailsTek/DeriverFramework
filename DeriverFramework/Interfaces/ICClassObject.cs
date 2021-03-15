using DF.Classes;
using DF.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DF.Interfaces
{
    /// <summary>
    /// CClass object.
    /// </summary>
    public interface ICClassObject
    {
        /// <summary>
        /// Getting object name.
        /// </summary>
        /// <returns></returns>
        internal string ObjectName();
        /// <summary>
        /// Type.
        /// </summary>
        /// <returns></returns>
        public CClassObjectType Type();
        /// <summary>
        /// Invoke method with parameters & get result.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal string Invoke(params string[] Params);
        /// <summary>
        /// Set for object cclass.
        /// </summary>
        /// <param name="cclass"></param>
        internal void SetCClass(CClass cclass);
        /// <summary>
        /// Get object.
        /// </summary>
        /// <returns></returns>
        internal object GetObject();
        /// <summary>
        /// Get HashCode of object.
        /// </summary>
        /// <returns></returns>
        internal int GetHashCode();
    }
}
