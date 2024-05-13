﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

// Suriya made changes here

namespace MissionPlanner
{
    public class DoseRateUpdater
    {
        private System.Windows.Forms.Timer tickFunc;

        float userThreshold;
        float userDetSense1;
        float userDetSense2;

        float GMone;
        float GMtwo;



        public class PopupForm : Form
        {
            // Labels for user input descriptions
            private Label lblDetectorSensitivity1;
            private Label lblDetectorSensitivity2;
            private Label lblThreshold;

            // TextBoxes for user input (made public)
            public TextBox txtDetectorSensitivity1;
            public TextBox txtDetectorSensitivity2;
            public TextBox txtThreshold;

            // Button to submit the value
            private Button btnSubmit;

            public PopupForm()
            {
                InitializeComponent();
                this.StartPosition = FormStartPosition.CenterScreen; // Center the form
            }

            private void InitializeComponent()
            {
                // Labels for user input descriptions
                lblDetectorSensitivity1 = new Label();
                lblDetectorSensitivity1.Text = "Detector sensitivity 1:";
                lblDetectorSensitivity1.AutoSize = true; // Allow automatic sizing
                lblDetectorSensitivity1.Anchor = (AnchorStyles.Left | AnchorStyles.Top); // Anchor to top-left corner

                lblDetectorSensitivity2 = new Label();
                lblDetectorSensitivity2.Text = "Detector sensitivity 2:";
                lblDetectorSensitivity2.AutoSize = true;
                lblDetectorSensitivity2.Anchor = (AnchorStyles.Left | AnchorStyles.Top);

                lblThreshold = new Label();
                lblThreshold.Text = "Threshold:";
                lblThreshold.AutoSize = true;
                lblThreshold.Anchor = (AnchorStyles.Left | AnchorStyles.Top);

                // TextBoxes for user input
                txtDetectorSensitivity1 = new TextBox();
                txtDetectorSensitivity1.Dock = DockStyle.Top; // Position at the top of the form

                txtDetectorSensitivity2 = new TextBox();
                txtDetectorSensitivity2.Dock = DockStyle.Top;

                txtThreshold = new TextBox();
                txtThreshold.Dock = DockStyle.Top;

                // Arrange controls in a vertical layout (using a TableLayoutPanel)
                TableLayoutPanel panel = new TableLayoutPanel();
                panel.Dock = DockStyle.Fill; // Fill the entire form
                panel.AutoSize = true; // Allow automatic sizing for the panel
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Labels
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); // TextBoxes (flexible space)
                panel.Controls.Add(lblDetectorSensitivity1, 0, 0);
                panel.Controls.Add(txtDetectorSensitivity1, 1, 0);
                panel.Controls.Add(lblDetectorSensitivity2, 0, 1);
                panel.Controls.Add(txtDetectorSensitivity2, 1, 1);
                panel.Controls.Add(lblThreshold, 0, 2);
                panel.Controls.Add(txtThreshold, 1, 2);

                Controls.Add(panel); // Add the panel to the form

                // Initialize the Submit button and add it to the form
                btnSubmit = new Button();
                btnSubmit.Text = "OK";
                btnSubmit.Dock = DockStyle.Bottom; // Position at bottom
                btnSubmit.Click += BtnSubmit_Click;
                Controls.Add(btnSubmit);

                // Reduce the form height (adjust as needed)
                this.ClientSize = new System.Drawing.Size(300, 125); // Example size

                // Other form properties and settings can be configured here
                this.Text = "Detector Parameters";
            }

            private void BtnSubmit_Click(object sender, EventArgs e)
            {
                // Retrieve user input from TextBoxes
                string detectorSensitivity1 = txtDetectorSensitivity1.Text;
                string detectorSensitivity2 = txtDetectorSensitivity2.Text;
                string threshold = txtThreshold.Text;

                // Close the form and return DialogResult.OK (or handle errors)
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }



        public void showDoseRate(ToolStripMenuItem toolStripMenuItem)
        {
            PopupForm popup = new PopupForm();

            if (popup.ShowDialog() == DialogResult.OK)
            {
                // Update user variables with specific user input 

                userThreshold = float.Parse(popup.txtThreshold.Text);
                userDetSense1 = float.Parse(popup.txtDetectorSensitivity1.Text);
                userDetSense2 = float.Parse(popup.txtDetectorSensitivity2.Text);

            }

            popup.Dispose();

            if (tickFunc == null)
            {
                tickFunc = new System.Windows.Forms.Timer();
                tickFunc.Interval = 1;
                tickFunc.Tick += (sender, e) => RadiationDetection(sender, e, toolStripMenuItem);
                tickFunc.Start();

            }

            else if (!tickFunc.Enabled)
            {
                tickFunc.Start();
            }

        }


        private void RadiationDetection(object sender, EventArgs e, ToolStripMenuItem toolStripMenuItem)
        {

            if (MainV2.comPort != null && MainV2.comPort.MAV != null && MainV2.comPort.MAV.cs != null)
            {

                string message = MainV2.comPort.MAV.cs.message;
                if (message != null)
                {
                    string pattern1 = @"^GMs\s\d(\d{4})";
                    //string pattern2 = @"^GMe\s\d(\d{4})";                
                    string pattern2 = @"^GMs\s\d\d\d\d\d\s\d\d\d\d\d\s\d(\d{4})";

                    Match match1 = Regex.Match(message, pattern1);
                    if (match1.Success)
                    {
                        // Extract the second to fifth digits after "GMs"
                        string extractedValue1 = match1.Groups[1].Value;
                        float GM1 = float.Parse(extractedValue1);
                        GMone = GM1;

                    }
                    else
                    {
                        toolStripMenuItem.Text = "nothing here";
                    }


                    Match match2 = Regex.Match(message, pattern2);
                    if (match2.Success)
                    {
                        // Extract the second to fifth digits after "GMs"
                        string extractedValue2 = match2.Groups[1].Value;
                        float GM2 = float.Parse(extractedValue2);
                        GMtwo = GM2;

                    }
                    else
                    {
                        toolStripMenuItem.Text = "nothing here";
                    }

                    if (userThreshold <= GMtwo)
                    {
                        float finalValue1 = GMtwo / userDetSense2;
                        if (GMtwo <= userThreshold)
                        {
                            toolStripMenuItem.Text = finalValue1 + " nsv/h ";
                            toolStripMenuItem.ForeColor = Color.Black;

                        }
                        else
                        {
                            toolStripMenuItem.Text = finalValue1 + " nsv/h ";
                            toolStripMenuItem.ForeColor = Color.Red;
                        }

                    }
                    else
                    {
                        float finalValue1 = GMone / userDetSense1;
                        if (GMone <= userThreshold)
                        {
                            toolStripMenuItem.Text = finalValue1 + " nsv/h ";
                            toolStripMenuItem.ForeColor = Color.Black;

                        }
                        else
                        {
                            toolStripMenuItem.Text = finalValue1 + " nsv/h ";
                            toolStripMenuItem.ForeColor = Color.Red;
                        }
                    }
                }

                else
                {
                    toolStripMenuItem.Text = "nothing here";
                    toolStripMenuItem.ForeColor = Color.Red;
                }

            }
            else
            {
                toolStripMenuItem.Text = "nothing here";
                toolStripMenuItem.ForeColor = Color.Red;
            }

        }

    }
}
