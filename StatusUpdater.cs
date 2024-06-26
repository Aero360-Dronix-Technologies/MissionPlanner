using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MissionPlanner.GCSViews;

namespace MissionPlanner
{
    public class StatusUpdater
    {
        private System.Windows.Forms.Timer statusTimer;
        private System.Windows.Forms.Timer checkTimer;
        private bool patternMatched;

        public void showStatus(ToolStripMenuItem toolStripMenuItem)
        {

            


            if (statusTimer == null)
            {
                statusTimer = new System.Windows.Forms.Timer();
                statusTimer.Interval = 1000; // Check every second
                statusTimer.Tick += (sender, e) => findstatus(sender, e, toolStripMenuItem);
                statusTimer.Start();
            }
            else if (!statusTimer.Enabled)
            {
                statusTimer.Start();
            }
            /*
            if (checkTimer == null)
            {
                checkTimer = new System.Windows.Forms.Timer();
                checkTimer.Interval = 5000; // 5 seconds interval
                checkTimer.Tick += (sender, e) => checkConnectionStatus(toolStripMenuItem);
                checkTimer.Start();
            }
            else if (!checkTimer.Enabled)
            {
                checkTimer.Start();
            }
            */
        }

        private void findstatus(object sender, EventArgs e, ToolStripMenuItem toolStripMenuItem)
        {
            if (MainV2.comPort != null && MainV2.comPort.MAV != null && MainV2.comPort.MAV.cs != null)
            {
                string statusMessage = MainV2.comPort.MAV.cs.message;
                float rdmstatus = MainV2.comPort.MAV.cs.rdmstatus;

                if (rdmstatus == 0)
                {
                    toolStripMenuItem.Text = "Disconnected";
                    toolStripMenuItem.ForeColor = Color.Red;
                }
                else
                {
                    toolStripMenuItem.Text = "Connected";
                    toolStripMenuItem.ForeColor = Color.Green;
                }
                
            }
            else
            {
                patternMatched = false;
            }
        }

        private void checkConnectionStatus(ToolStripMenuItem toolStripMenuItem)
        {
            if (!patternMatched)
            {
                toolStripMenuItem.Text = "Connected";
                toolStripMenuItem.ForeColor = Color.Green;
            }
            patternMatched = false; // Reset for the next interval
        }
    }
}

 