using DFPluginAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DF.Tools
{
    public sealed class PluginManager : PluginAPI
    {
        public override bool GetBool(params string[] pparams)
        {
            return false;
        }

        public override float GetFloat(params string[] pparams)
        {
            return 0;
        }

        public override string GetString(params string[] pparams)
        {
            return null;
        }

        public override string GetType(params string[] pparams)
        {
            return null;
        }

        public override T GetValue<T>(params string[] pparams)
        {
            return default;
        }

        public override void SetBool(bool val, params string[] pparams)
        {
            return;
        }

        public override void SetFloat(float val, params string[] pparams)
        {
            return;
        }

        public override void SetString(string val, params string[] pparams)
        {
            return;
        }

        public override void SetValue<T>(T val, params string[] pparams)
        {
            return;
        }
    }
}
