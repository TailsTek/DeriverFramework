using DFPluginAPI.Permissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DFPluginAPI.Classes
{
    public abstract class Permission
    {
        internal static Permission[] Permissions = new Permission[] 
        {
            new Variables(),
            new Permissions.Classes(),
        };
        public string Name { get; private set; }
        protected Permission(string Name)
        {
            this.Name = Name;
        }
        public static Permission GetPermission(string name)
        {
            Permission perm = default;
            foreach (var item in Permissions)
            {
                if (item.Name == name)
                {
                    perm = item;
                }
            }
            return perm;
        }
        public static Permission[] GetPermissions(params string[] pparams)
        {
            List<Permission> perms = new List<Permission>();
            foreach (var item in pparams)
            {
                foreach (var it in Permissions)
                {
                    if (it.Name == item)
                        perms.Add(it);
                }
            }
            return perms.ToArray();
        }
    }
}
