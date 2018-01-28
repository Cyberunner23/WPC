using System;

using KeePass.Plugins;
using System.Windows.Forms;

using KeePassLib.Security;

namespace WPC
{
    public class WPCExt : Plugin 
    {

        private IPluginHost _host = null;

        public override bool Initialize(IPluginHost host)
        {
            MessageBox.Show("EYYY");
            _host = host;
            var item = new ToolStripMenuItem()
            {
                Name = "AutoPasswordChange",
                Text = "Auto Password Change"
            };
            item.DropDownItems.Add("GMail", null, (sender, e) => { MessageBox.Show("GMail"); });
            item.DropDownItems.Add("Facebook", null, (sender, e) => { MessageBox.Show("Facebook"); });
            item.DropDownItems.Add("Facebook", null, (sender, e) => {
                Type type = sender.GetType();
                MessageBox.Show($"TYPE: {type.Name}");
                var pass = _host.MainWindow.GetSelectedEntry(false);
                pass.Strings.Set("old_password", new ProtectedString().Insert(0, "asdfgh"));

            });

            _host.MainWindow.EntryContextMenu.Items.Add(item);

            var current_db = _host.Database;
            _host.MainWindow.SaveDatabase(current_db, this);

            /*var pass = _host.MainWindow.GetSelectedEntry(false);
            pass.Strings.Set("old_password", new ProtectedString().Insert(0, "asdfgh"));
*/
            return true;
        }
    }
}
