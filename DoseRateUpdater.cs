
using System;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using MissionPlanner;
using static IronPython.Modules._ast;
using System.IO;

namespace MissionPlanner
{
    public class DoseRateUpdater
    {
        private System.Windows.Forms.Timer tickFunc;

        public void showDoseRate(ToolStripMenuItem toolStripMenuItem)
        {

            if (tickFunc == null)
            {
                tickFunc = new System.Windows.Forms.Timer();
                tickFunc.Interval = 1000; // Set the interval to 1 second
                tickFunc.Tick += (sender, e) => RadiationDetection(sender, e, toolStripMenuItem);
                tickFunc.Start();

            }
            else if (!tickFunc.Enabled)
            {
                tickFunc.Start();
            }
        }

        public void RadiationDetection(object sender, EventArgs e, ToolStripMenuItem toolStripMenuItem)
        {
            if (MainV2.comPort != null && MainV2.comPort.MAV != null && MainV2.comPort.MAV.cs != null)
            {
                
                double finaldose = MainV2.comPort.MAV.cs.dose_rate;
                double finalthreshold = MainV2.comPort.MAV.cs.threshold;
                float rdmstatus = MainV2.comPort.MAV.cs.rdmstatus;

                if (rdmstatus == 0)
                {
                    toolStripMenuItem.AutoSize = false;
                    toolStripMenuItem.Width = 70; // Adjust the width as needed
                    toolStripMenuItem.TextAlign = ContentAlignment.MiddleCenter;

                    toolStripMenuItem.Text = "No data";
                    toolStripMenuItem.ForeColor = Color.Black;


                }

                else
                {
                    toolStripMenuItem.AutoSize = false;
                    toolStripMenuItem.Width = 70; // Adjust the width as needed
                    toolStripMenuItem.TextAlign = ContentAlignment.MiddleCenter;
                    
                    toolStripMenuItem.Text = $"{finaldose.ToString("0.00")} \n nsv/h";
                    toolStripMenuItem.ForeColor = finaldose >= finalthreshold ? Color.Red : Color.Black;


                }
                
                

            }

        }
    }

}

