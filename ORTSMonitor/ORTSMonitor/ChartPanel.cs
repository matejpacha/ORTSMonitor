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
        LineSeries lineSeriesGradient;
        LineSeries lineSeriesCurve;
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

            lineSeriesCurve = new LineSeries
            {
                Title = "Curve Radius",
                MarkerType = MarkerType.None
            };
            lineSeriesGradient = new LineSeries
            {
                Title = "Gradient",
                MarkerType = MarkerType.None
            };

            plotModel.Series.Add(lineSeriesSpeed);
            plotModel.Series.Add(lineSeriesSpeedLimit);
            plotModel.Series.Add(lineSeriesCurve);
            plotModel.Series.Add(lineSeriesGradient);


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
                Title = "Speed [km/h], Gradient [10x %], Curve [0.01x m]",
                Minimum = 0,
                Maximum = 100
            };
            plotModel.Axes.Add(yAxis);

            if (Properties.Settings.Default.ChartSpeedLimitVisible)
            {
                lineSeriesSpeedLimit.IsVisible = true;
                contextMenuStrip1.Items[3].Text = "&Speed limit ✓";
            }
            else
            {
                lineSeriesSpeedLimit.IsVisible = false;
                contextMenuStrip1.Items[3].Text = "&Speed limit";
            }

            if (Properties.Settings.Default.ChartCurveVisible)
            {
                lineSeriesCurve.IsVisible = true;
                contextMenuStrip1.Items[4].Text = "&Curve radius ✓";
            }
            else
            {
                lineSeriesCurve.IsVisible = false;
                contextMenuStrip1.Items[4].Text = "&Curve radius";
            }

            if (Properties.Settings.Default.ChartGradeVisible)
            {
                lineSeriesGradient.IsVisible = true;
                contextMenuStrip1.Items[5].Text = "&Gradient ✓";
            }
            else
            {
                lineSeriesGradient.IsVisible = false;
                contextMenuStrip1.Items[5].Text = "&Gradient";
            }
        }

        public void AddSpeedPoint(double distance, double speed, double limit, double grade, double curve)
        {
            lineSeriesSpeed.Points.Add(new DataPoint(distance, speed));
            lineSeriesSpeedLimit.Points.Add(new DataPoint(distance, limit));
            lineSeriesGradient.Points.Add(new DataPoint(distance, grade));
            lineSeriesCurve.Points.Add(new DataPoint(distance, curve));
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

                if (limit > yAxis.Maximum)
                    yAxis.Maximum = limit;
                if (speed > yAxis.Maximum)
                    yAxis.Maximum = speed;
                if (grade > yAxis.Maximum)
                    yAxis.Maximum = grade;
                if (curve > yAxis.Maximum)
                    yAxis.Maximum = curve;

                if (limit < yAxis.Minimum)
                    yAxis.Minimum = limit;
                if (speed < yAxis.Minimum)
                    yAxis.Minimum = speed;
                if (grade < yAxis.Minimum)
                    yAxis.Minimum = grade;
                if (curve < yAxis.Minimum)
                    yAxis.Minimum = curve;

                yAxis.Reset();
            }
            plotView1.Invalidate();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lineSeriesSpeed.Points.Clear();
            lineSeriesSpeedLimit.Points.Clear();
        }

        private void speedLimitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lineSeriesSpeedLimit.IsVisible)
            {
                lineSeriesSpeedLimit.IsVisible = false;
                contextMenuStrip1.Items[3].Text = "&Speed limit";
            }
            else
            {
                lineSeriesSpeedLimit.IsVisible = true;
                contextMenuStrip1.Items[3].Text = "&Speed limit ✓";
            }

        }

        private void curveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lineSeriesCurve.IsVisible)
            {
                lineSeriesCurve.IsVisible = false;
                contextMenuStrip1.Items[4].Text = "&Curve radius";
            }
            else
            {
                lineSeriesCurve.IsVisible = true;
                contextMenuStrip1.Items[4].Text = "&Curve radius ✓";
            }
        }

        private void gradientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lineSeriesGradient.IsVisible)
            {
                lineSeriesGradient.IsVisible = false;
                contextMenuStrip1.Items[4].Text = "&Gradient";
            }
            else
            {
                lineSeriesGradient.IsVisible = true;
                contextMenuStrip1.Items[4].Text = "&Gradient ✓";
            }

        }
    }
}
