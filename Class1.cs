using System;
using System.Windows.Forms;
using static IronPython.Modules._ast;

namespace MissionPlanner
{
    public class AltitudeUpdaters
    {
        private System.Windows.Forms.Timer altitudeUpdateTimer;

        public void showAltitude(ToolStripMenuItem toolStripMenuItem)
        {
            if (altitudeUpdateTimer == null)
            {
                // Create and start the timer for dynamic updates
                altitudeUpdateTimer = new System.Windows.Forms.Timer();
                altitudeUpdateTimer.Interval = 1; // Update every 0.1 seconds (adjust as needed)
                altitudeUpdateTimer.Tick += (sender, e) => altitudeUpdateTimer_Tick(sender, e, toolStripMenuItem);
                altitudeUpdateTimer.Start();
            }
            else if (!altitudeUpdateTimer.Enabled)
            {
                altitudeUpdateTimer.Start();
            }
        }

        private void altitudeUpdateTimer_Tick(object sender, EventArgs e, ToolStripMenuItem toolStripMenuItem)
        {
            if (MainV2.comPort != null && MainV2.comPort.MAV != null && MainV2.comPort.MAV.cs != null)
            {
                float lat = (float)MainV2.comPort.MAV.cs.lat;
                float log = (float)MainV2.comPort.MAV.cs.lng;
                if (lat== 0 && log==0) 
                {
                    toolStripMenuItem.Text = "lat, long: " + lat.ToString() + ", " + log.ToString();
                }
            }
            else
            {

                float lat = (float)MainV2.comPort.MAV.cs.lat;
                float log = (float)MainV2.comPort.MAV.cs.lng;
                toolStripMenuItem.Text = "lat, long: " + lat.ToString() + ", " + log.ToString();
            }
            }
            
        }
    }

