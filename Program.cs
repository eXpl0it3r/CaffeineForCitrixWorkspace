using System;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Forms;

[assembly: SupportedOSPlatform("windows")]
namespace CaffeineV2
{
    public static class Program
    {
        private static Mutex _mutex;

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _mutex = new Mutex(true, "CaffeineForWorkspaceMutex", out var createdNew);

            if (createdNew)
            {
                Application.Run(new FormHidden());
            }
            else
            {
                MessageBox.Show("The application is already running.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
