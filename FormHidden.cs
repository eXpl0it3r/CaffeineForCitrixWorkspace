using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using Microsoft.Win32;
using static Caffeine.Properties.Settings;

namespace Caffeine
{
    public partial class FormHidden : Form
    {
        private const string CcmRegistryPath = @"SOFTWARE\WOW6432Node\Citrix\ICA Client\CCM";

        public FormHidden()
        {
            InitializeComponent();
        }

        private static bool CheckCcmRegistryKeys()
        {
            var ccmValues = new List<string> {"AllowSimulationAPI", "AllowLiveMonitoring"};

            using var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            var ccmKey = baseKey.OpenSubKey(CcmRegistryPath);
            
            return ccmKey != null && ccmValues.All(value => (int) ccmKey.GetValue(value, -1) == 1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tmrTick.Interval = Default.TimerIntervalSeconds * 1000;

            try
            {
                Debug.WriteLine("Testing ICA client object");
                var icaClient = new WFICALib.ICAClientClass();
                var version = icaClient.Version;
                var enumHandle = icaClient.EnumerateCCMSessions();
                var sessionCount = icaClient.GetEnumNameCount(enumHandle);
                Debug.WriteLine($"ICA client object creation successful: Version {version} / Sessions {sessionCount}");
                icaClient.CloseEnumHandle(enumHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while registering the ICA CCM Object", "Error on Startup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.WriteLine($"An error occurred while registering the ICA CCM Object: {ex.Message}");
                Close();
            }

            if (!CheckCcmRegistryKeys())
            {
                MessageBox.Show(@"The CCM registry DWORDS (AllowSimulationAPI & AllowLiveMonitoring) are missing from HKLM:\" + CcmRegistryPath, "Missing CCM values detected on Startup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            Debug.WriteLine("Starting Timer");

            tmrTick.Start();
            Visible = false;
        }

        private void tmrTick_Tick(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("Timer Ticking");

                var icaClient = new WFICALib.ICAClientClass();
                var enumHandle = icaClient.EnumerateCCMSessions();
                var sessionCount = icaClient.GetEnumNameCount(enumHandle);
                Debug.WriteLine($"Found {sessionCount} sessions");

                for (var itemCount = 0; itemCount < sessionCount; itemCount++)
                {
                    try
                    {
                        Debug.WriteLine("Sending keepalive to session: {0}", itemCount);
                        var session = icaClient.GetEnumNameByIndex(enumHandle, itemCount);
                        var icaSession = new WFICALib.ICAClientClass();

                        icaSession.SetProp("OutputMode", "1");
                        icaSession.StartMonitoringCCMSession(session, true);
                        icaSession.Session.Keyboard.SendKeyDown(Default.KeyValue);
                        icaSession.StopMonitoringCCMSession(session);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Keepalive to session: {itemCount} failed: {ex}");
                    }
                }

                icaClient.CloseEnumHandle(enumHandle);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Timer tick exception: {ex}");
            }
        }

        private void FormHidden_Shown(object sender, EventArgs e)
        {
            Hide();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            var fa = new FormAbout();
            fa.Show();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
