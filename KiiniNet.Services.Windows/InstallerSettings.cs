using System.ComponentModel;
using System.Configuration.Install;

namespace KiiniNet.Services.Windows
{
    [RunInstaller(true)]
    public partial class InstallerSettings : System.Configuration.Install.Installer
    {
        public InstallerSettings()
        {
            InitializeComponent();
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
