
using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

using KeePass.Plugins;
using KeePassLib;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;
using KeePassLib.Utility;
using WPC.Websites;

using YamlDotNet.Serialization;

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

            _autoPasswordChangerMenuItem.DropDown.Items.Add("Facebook", null, (sender, args) =>
            {
                Facebook facebookInteraction = new Facebook();
                ChangePassword(facebookInteraction);
            });
            
            _autoPasswordChangerMenuItem.DropDown.Items.Add("Github", null, (sender, args) =>
            {
                Github githubInteraction = new Github();
					ChangePassword(githubInteraction);
            });
            
            _autoPasswordChangerMenuItem.DropDown.Items.Add("Twitter", null, (sender, args) =>
            {
                Twitter twitterInteraction = new Twitter();
					ChangePassword(twitterInteraction);
            });
            
			try {
				var deserializer = new DeserializerBuilder().Build();
				string ymlPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/sites.yml";
				if (File.Exists(ymlPath)) {
					var sites = deserializer.Deserialize<List<Site>>(File.OpenText(ymlPath));
					foreach (Site s in sites) {
						_autoPasswordChangerMenuItem.DropDown.Items.Add(s.name, null, (sender, args) =>
							{
								CustomInteraction customInteraction = new CustomInteraction(s);
								ChangePassword(customInteraction);
							});
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

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
            ProtectedString newPassword = new ProtectedString(true, DeriveNewPassword(password.ReadUtf8()));

            try
            {
                interactor.login(username.ReadString(), password.ReadString());    
            } 
            catch(Exception e)
            {
                MessageBox.Show($"Failed to login into the website: {e.Message}");
                return;
            }
            
            
            selectedEntry.Strings.Set("wpc_old_password", password);
            selectedEntry.Strings.Set(PwDefs.PasswordField, newPassword);
            selectedEntry.CustomData.Set("wpc_passwd_update_status", "incomplete");
            
            PwDatabase currentDatabase = _host.Database;
            _host.MainWindow.SaveDatabase(currentDatabase, this);

            try
            {
                interactor.changePassword(password.ReadString(), newPassword.ReadString());
            }
            catch (Exception e)
            {
                MessageBox.Show($"Failed to change the password on the website: {e.Message}");
                return;
            }
            

            selectedEntry.CustomData.Set("wpc_passwd_update_status", "complete");
            
            _host.MainWindow.SaveDatabase(currentDatabase, this);
        }

        private byte[] DeriveNewPassword (byte[] currentPassword) {
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
                return DeriveNewPassword(currentPassword);
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
