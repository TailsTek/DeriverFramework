using DFPluginAPI.Classes;
using System;

namespace DFPluginAPI
{
    public abstract class Plugin
    {
        /// <summary>
        /// List of permissions.
        /// </summary>
        public Permission[] Permissions { get; private set; }
        public string Name { get; private set; }
        internal PluginAPI PluginAPI { get; private set; }

        public Plugin(string name, params string[] pparams)
        {
            Name = name;
            Permissions = Permission.GetPermissions(pparams);
        }
        public void SetAPI(PluginAPI API)
        {
            PluginAPI = API;
        }

        protected abstract void Start();
        protected abstract void Stop();
        public virtual string GetString(params string[] pparams)
        {
            return PluginAPI.GetString(pparams);
        }
        public virtual float GetFloat(params string[] pparams)
        {
            return PluginAPI.GetFloat(pparams);
        }
        public virtual bool GetBool(params string[] pparams)
        {
            return PluginAPI.GetBool(pparams);
        }
        public virtual string GetType(params string[] pparams)
        {
            return PluginAPI.GetType(pparams);
        }
        public virtual T GetValue<T>(params string[] pparams)
        {
            return PluginAPI.GetValue<T>(pparams);
        }
    }
}
