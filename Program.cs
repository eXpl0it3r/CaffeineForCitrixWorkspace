﻿using System;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Forms;

[assembly: SupportedOSPlatform("windows")]

namespace Caffeine;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        using var mutex = new Mutex(true, "CaffeineForWorkspaceMutex", out var createdNew);

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