using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ORTSMonitor
{
    public partial class ConsolePanel : DockContent
    {
        public ConsolePanel()
        {
            InitializeComponent();
            this.Text = "Console";
        }

        public string ConsoleText
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }
    }
}
