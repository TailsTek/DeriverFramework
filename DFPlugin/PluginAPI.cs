using DFPluginAPI.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFPluginAPI
{
    public abstract class PluginAPI
    {
        public Plugin[] Plugins { get; private set; }
        public Plugin[] LoadedPlugins { get; private set; }
        public Permission[] WorkingPermissions { get; private set; }

        protected void AddPlugin(Plugin plugin)
        {
            var pl = Plugins.ToList();
        }

        protected void LoadPlugins()
        {
            var pl = LoadedPlugins.ToList();
            foreach (var item in Plugins)
            {
                foreach (var perm in item.Permissions)
                {
                    if (WorkingPermissions.Contains(perm))
                    {
                        
                    }
                }
            }
        }

        protected void UnloadPlugins()
        {

        }

        #region abstract
        public abstract string GetString(params string[] pparams);
        public abstract float GetFloat(params string[] pparams);
        public abstract bool GetBool(params string[] pparams);
        public abstract string GetType(params string[] pparams);
        public abstract T GetValue<T>(params string[] pparams);

        public abstract void SetString(string val, params string[] pparams);
        public abstract void SetFloat(float val, params string[] pparams);
        public abstract void SetBool(bool val, params string[] pparams);
        public abstract void SetValue<T>(T val, params string[] pparams);
        #endregion

        public void SetAllPerms()
        {
            WorkingPermissions = Permission.Permissions;
        }
        public void AddPerm(string permname)
        {
            Permission perm = Permission.GetPermission(permname);
            if (perm == null)
                return;

            foreach (var item in WorkingPermissions)
            {
                if (item.Name == perm.Name)
                    return;
            }
            var perms = WorkingPermissions.ToList();
            perms.Add(perm);
            WorkingPermissions = perms.ToArray();
        }
        public void RemovePerm(string permname)
        {
            Permission perm = Permission.GetPermission(permname);
            if (perm == null)
                return;

            foreach (var item in WorkingPermissions)
            {
                if (item.Name == perm.Name)
                    return;
            }
            var perms = WorkingPermissions.ToList();
            perms.Remove(perm);
            WorkingPermissions = perms.ToArray();
        }
        public void RemoveAtPerm(int permid)
        {
            var perms = WorkingPermissions.ToList();
            perms.RemoveAt(permid);
            WorkingPermissions = perms.ToArray();
        }
    }
}
