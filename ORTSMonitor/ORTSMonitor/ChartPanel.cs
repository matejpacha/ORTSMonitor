using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
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
    public partial class ChartPanel : DockContent
    {
        LineSeries lineSeriesSpeed;
        LineSeries lineSeriesSpeedLimit;
        PlotModel plotModel;
        public ChartPanel()
        {


            InitializeComponent();

            this.Text = "Chart";

            plotModel = new PlotModel { Title = "Speed vs. Distance" };
            lineSeriesSpeed = new LineSeries
            {
                Title = "Speed",
                MarkerType = MarkerType.None
            };

            lineSeriesSpeedLimit = new LineSeries
            {
                Title = "Speed Limit",
                MarkerType = MarkerType.None
            };

            plotModel.Series.Add(lineSeriesSpeed);
            plotModel.Series.Add(lineSeriesSpeedLimit);
            plotView1.Model = plotModel;

            // Lineární osa
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Distance",
                Minimum = 0,
                Maximum = 1000
            };
            plotModel.Axes.Add(xAxis);

            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Speed",
                Minimum = 0,
                Maximum = 100
            };
            plotModel.Axes.Add(yAxis);
        }

        public void AddSpeedPoint(double distance, double speed, double limit)
        {
            lineSeriesSpeed.Points.Add(new DataPoint(distance, speed));
            lineSeriesSpeedLimit.Points.Add(new DataPoint(distance, limit));
            var xAxis = plotModel.Axes.FirstOrDefault(a => a.Position == OxyPlot.Axes.AxisPosition.Bottom);
            if (xAxis != null)
            {
                xAxis.Minimum = 0;   // Set new minimum
                if ((distance == 0.0) && (speed == 0.0))
                {
                    lineSeriesSpeed.Points.Clear();
                    lineSeriesSpeedLimit.Points.Clear();
                    xAxis.Maximum = 100; // Set new maximum
                }
                else
                {
                    if (distance > xAxis.Maximum)
                        xAxis.Maximum = distance;
                }

                xAxis.Reset();
            }
            var yAxis = plotModel.Axes.FirstOrDefault(a => a.Position == OxyPlot.Axes.AxisPosition.Left);
            if (yAxis != null)
            {
                yAxis.Minimum = 0;
                if (limit > yAxis.Maximum)
                    yAxis.Maximum = limit;
                if (speed > yAxis.Maximum)
                    yAxis.Maximum = speed;
                yAxis.Reset();
            }
            plotView1.Invalidate();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lineSeriesSpeed.Points.Clear();
            lineSeriesSpeedLimit.Points.Clear();
        }
    }
}
