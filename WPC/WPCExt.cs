
using System.Linq;
using System.Windows.Forms;

using KeePass.Plugins;
using KeePassLib;
using KeePassLib.Security;


using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Utility;

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

        public byte[] deriveNewPassword (byte[] currentPassword) {
            ProtectedString psCur = new ProtectedString(true, currentPassword);
            PwProfile pwp = PwProfile.DeriveFromPassword(psCur);

            ProtectedString psNew;
            PwGenerator.Generate(
                out psNew,
                pwp,
                null,
                _host.PwGeneratorPool
            );
            byte[] pbNew = psNew.ReadUtf8();

            if (currentPassword.SequenceEqual(pbNew)) {
                //Try again, this shouldn't happen too often
                return deriveNewPassword(currentPassword);
            }

            //Zero fill the currentPassword buffer
            MemUtil.ZeroByteArray(currentPassword);

            //You should zero fill the array after you are done
            //using it. Because the array might still be in memory for a while.
            //We don't want memory sniffers getting it so easily
            //KeePassLib.Utility.MemUtil.ZeroByteArray(pbNew);
            return pbNew;
        }
    }
}
