using System;

using KeePass.Plugins;
using System.Windows.Forms;

namespace WPC
{
    public class WPCExt : Plugin 
    {

        private IPluginHost _host = null;

        public override bool Initialize(IPluginHost host)
        {
            MessageBox.Show("EYYY");
            _host = host;
            return true;
        }
    }
}
