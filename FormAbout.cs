using System.Diagnostics;
using System.Windows.Forms;

namespace Caffeine;

public partial class FormAbout : Form
{
    public FormAbout()
    {
        InitializeComponent();
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://github.com/eXpl0it3r/CaffeineForCitrixWorkspace")
        {
            UseShellExecute = true
        });
    }
}