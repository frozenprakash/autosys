using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace Autosys
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        Form1 f1=new Form1();

        private void button1_Click(object sender, EventArgs e)
        {
            f1.Form1_Load_Default_settings();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
