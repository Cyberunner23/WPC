using System;

using KeePass.Plugins;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

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
