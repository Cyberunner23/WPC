using System;

using KeePass.Plugins;

namespace WPC
{
    public class WPCExt : Plugin 
    {

        private IPluginHost _host = null;

        public override bool Initialize(IPluginHost host)
        {
            _host = host;
            return true;
        }
    }
}
