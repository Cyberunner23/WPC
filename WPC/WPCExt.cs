using System.Net.NetworkInformation;
using System.Windows.Forms;

using KeePass.Plugins;
using KeePassLib;
using KeePassLib.Security;


namespace WPC
{
    public class WPCExt : Plugin 
    {
        private IPluginHost _host = null;

        private ToolStripMenuItem _autoPasswordChangerMenuItem;
        
		
        public override bool Initialize(IPluginHost host)
        {
            _host = host;

            _autoPasswordChangerMenuItem = new ToolStripMenuItem()
            {
                Name = "AutoPasswordChanger",
                Text = "Auto Password Changer"
            };

            _autoPasswordChangerMenuItem.Click += (sender, args) =>
            {
                ChangePassword(null);
            };

            _host.MainWindow.EntryContextMenu.Items.Add(_autoPasswordChangerMenuItem);
            
            return true;
        }

        public override void Terminate()
        {
            
        }


        private void ChangePassword(IWebInteractor interactor)
        {
            
            
            
            PwEntry selectedEntry = _host.MainWindow.GetSelectedEntry(false);
            ProtectedString username = selectedEntry.Strings.GetSafe(PwDefs.UserNameField);
            ProtectedString password = selectedEntry.Strings.GetSafe(PwDefs.PasswordField);
            
            
            
            selectedEntry.Strings.Set("wpc_old_password", new ProtectedString().Insert(0, "asdfgh"));
            
        }
        
    }
}
