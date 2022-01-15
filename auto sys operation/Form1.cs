using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.IO;

namespace Autosys
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
        }
        
        [DllImport("user32.dll")]
        static extern int ExitWindowsEx(int uFlags, int dwReason);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool LockWorkStation();

        string hh, mm, am_pm, final, time;
        public string unm = System.Windows.Forms.SystemInformation.UserName;

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            //MessageBox.Show("devil iz the one...");
        }

        public void Form1_Load_Default_settings()
        {
            FileStream fs = new FileStream(@"C:\Users\" + unm + @"\Documents\Auto Sys Operation\Default Config.txt", FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int a = br.ReadInt32();
            int b = br.ReadInt32();
            int c = br.ReadInt32();
            int d = br.ReadInt32();

            int e = br.ReadInt32();
            int f = br.ReadInt32();
            
            MessageBox.Show("defalult values loaded to the application successfully...");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"C:\Users\" + unm + @"\Documents\Auto Sys Operation"))
                Directory.CreateDirectory(@"C:\Users\" + unm + @"\Documents\Auto Sys Operation");

            if (!File.Exists(@"C:\Users\" + unm + @"\Documents\Auto Sys Operation\Default Config.txt"))
            {
                FileStream fs = new FileStream(@"C:\Users\" + unm + @"\Documents\Auto Sys Operation\Default Config.txt", FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs);
                //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&//
                //************************************************//
                //bw.write val's...
                //1.combobox 1 index space " "
                //2.combobox 2 index space " "
                //3.combobox 3 index space " "
                //4.combobox 4 index space " "
                //5.checkbox 1 tick true & viceversa
                //6.checkbox 2 tick true & viceversa
                //************************************************//
                bw.Write(6);
                bw.Write(50);
                bw.Write(0);
                bw.Write(2);

                bw.Write(0);
                bw.Write(0);
                //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&//
                bw.Flush();
                bw.Close();
            }

            if (!File.Exists(@"C:\Users\" + unm + @"\Documents\Auto Sys Operation\Saved Config.txt"))
                File.Create(@"C:\Users\" + unm + @"\Documents\Auto Sys Operation\Saved Config.txt");


            if (!File.Exists(@"C:\Users\" + unm + @"\Documents\Auto Sys Operation\Last Saved Config.txt"))
                File.Create(@"C:\Users\" + unm + @"\Documents\Auto Sys Operation\Last Saved Config.txt");
            
            for (byte i = 1; i <= 12; i++)
            {
                comboBox1.Items.Add(i);
            }

            for (byte i = 0; i <= 59; i++)
            {
                if( (i.ToString().Length)==1 )
                    comboBox2.Items.Add("0"+i.ToString());
                else
                comboBox2.Items.Add(i.ToString());
            }

            comboBox3.Items.Add("AM");
            comboBox3.Items.Add("PM");

            comboBox4.Items.Add("Shutdown");
            comboBox4.Items.Add("Restart");
            comboBox4.Items.Add("Hibernate");
            comboBox4.Items.Add("Sleep");
            comboBox4.Items.Add("Logoff");
            comboBox4.Items.Add("Lock");

            comboBox1.SelectedIndex = 6;
            comboBox2.SelectedIndex = 50;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                button1.Text = "Engaged";
                button2.Text = "&dis Engage..";
                button1.BackColor = Color.Green;
                button2.BackColor = Color.Gray;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;

                hh = comboBox1.SelectedItem.ToString();
                mm = comboBox2.SelectedItem.ToString();
                am_pm = comboBox3.SelectedItem.ToString();

                final = hh + ":" + mm + " " + am_pm;

                timer1.Start();

                //if(f2.checkBox1.Checked==false)
                WindowState = FormWindowState.Minimized;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "plz select from list & don't type");
                button2.PerformClick();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                time = DateTime.Now.ToShortTimeString();
                if (string.Compare(final, time) == 0)
                {
                    button2.PerformClick();

                    switch (comboBox4.SelectedIndex)
                    { 
                        case 0:
                            System.Diagnostics.Process.Start("ShutDown", "-s -c \"system will shutdown within a minute., sorry you can't prevent it...\"");
                            break;
                        case 1:
                            System.Diagnostics.Process.Start("ShutDown", "-r -c \"system will Restart within a minute., sorry you can't prevent it...\"");
                            break;
                        case 2:
                            bool retval1 = Application.SetSuspendState(PowerState.Hibernate, true, false);
                            if (retval1 == false)
                                MessageBox.Show("Cannot Hibernate the System., Plz chk whether the hibernation option iz enabled in the power configuration in control panel", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);        
                            break;
                        case 3:
                            bool retval2 = Application.SetSuspendState(PowerState.Suspend, true, false);
                            if (retval2 == false)
                                MessageBox.Show("Cannot Suspend the System...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);        
                            break;
                        case 4:
                            ExitWindowsEx(0, 0);
                            break;
                        case 5:
                            bool retval3 = LockWorkStation();
                            if (retval3 == false)
                                throw new Win32Exception(Marshal.GetLastWin32Error());
                            break;
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "plz select from list & don't type");
                button2.PerformClick();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            button1.Text = "Engag&e";
            button2.Text = "Dis Engaged..";
            button2.BackColor = Color.Red;
            button1.BackColor = Color.Gray;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
        }

        private void applicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Auto System Operation Application...\n\nThis application has been developed for night downloader'z to remove their strain of switching off the system in morning or any'...\n\n In thiz application just select values from the boXes and choose the mode of operation then just press engage & when the given time matches the system time then the system goes to selected operation...,\n\n Before this when enabling hibernation plz just check in control panel whether hibernation is enabled in power configuration's...\n\n\nNote:~\nThis application had been developed to work on Microsoft Windows7 o/s., so plz install \".Net framework 3.5\" in other operating systems like \"vista or Xp\" to run thiz application... ", "Application Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Application had been developed purely by ƒяº$τ  δε√í⌠ \n\n And for any Comments or Feedback plz try to mail me here at frostisaghost@yahoo.com", "About ƒяº$τ  δε√í⌠", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }
    }
}
