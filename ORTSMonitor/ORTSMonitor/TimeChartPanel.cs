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
using OxyPlot.Axes;
using OxyPlot;
using OxyPlot.Series;

namespace ORTSMonitor
{
    public partial class TimeChartPanel : DockContent
    {

        List<LineSeries> lineSeries;
        PlotModel plotModel;
        public ItemSelectionPanel ItemSelectionPanel { get; set; }

        public TimeChartPanel()
        {
            InitializeComponent();

            this.Text = "Time chart";

            plotModel = new PlotModel { Title = "Time Chart" };
        }

        
    }
}
