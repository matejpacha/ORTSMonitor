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
using OxyPlot.Axes;

namespace ORTSMonitor
{
    public partial class characteristicsPanel : DockContent
    {

        LineSeries lineSeriesForce;
        LineSeries lineSeriesLimit;
        LineSeries lineSeriesFriction;

        ScatterSeries highlightForce;
        ScatterSeries highlightLimit;
        ScatterSeries highlightFriction;

        PlotModel plotModel;

        bool ZoomMode;

        public characteristicsPanel()
        {
            InitializeComponent();

            this.Text = "Characteristics";

            plotModel = new PlotModel { Title = "Force vs. Speed" };
            lineSeriesForce = new LineSeries
            {
                Title = "Force",
                MarkerType = MarkerType.None
            };

            lineSeriesLimit = new LineSeries
            {
                Title = "Force Limit",
                MarkerType = MarkerType.None
            };

            lineSeriesFriction = new LineSeries
            {
                Title = "Resistance",
                MarkerType = MarkerType.None
            };

            highlightForce = new ScatterSeries
            {
                MarkerType = MarkerType.Circle, // or your preferred marker
                MarkerFill = OxyColors.Green,
                MarkerSize = 6
            };
            highlightLimit = new ScatterSeries
            {
                MarkerType = MarkerType.Circle, // or your preferred marker
                MarkerFill = OxyColors.Orange,
                MarkerSize = 6
            };
            highlightFriction = new ScatterSeries
            {
                MarkerType = MarkerType.Circle, // or your preferred marker
                MarkerFill = OxyColors.Red,
                MarkerSize = 6
            };



            plotModel.Series.Add(lineSeriesForce);
            plotModel.Series.Add(lineSeriesLimit);
            plotModel.Series.Add(lineSeriesFriction);
            plotModel.Series.Add(highlightForce);
            plotModel.Series.Add(highlightLimit);
            plotModel.Series.Add(highlightFriction);

            plotView1.Model = plotModel;

            // Lineární osa
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Speed",
                Minimum = 0,
                Maximum = 100
            };
            plotModel.Axes.Add(xAxis);


            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Force",
                Minimum = 0,
                Maximum = 100
            };
            plotModel.Axes.Add(yAxis);
        }

        public void AddCharPoint(double speed, double force, double limit, double friction)
        {
            lineSeriesForce.Points.Add(new DataPoint(speed, force));
            lineSeriesLimit.Points.Add(new DataPoint(speed, limit));
            lineSeriesFriction.Points.Add(new DataPoint(speed, friction));

            highlightForce.Points.Clear();
            highlightForce.Points.Add(new ScatterPoint(speed, force));

            highlightLimit.Points.Clear();
            highlightLimit.Points.Add(new ScatterPoint(speed, limit));

            highlightFriction.Points.Clear();
            highlightFriction.Points.Add(new ScatterPoint(speed, friction));

            var xAxis = plotModel.Axes.FirstOrDefault(a => a.Position == OxyPlot.Axes.AxisPosition.Bottom);
            if (xAxis != null)
            {
                xAxis.Minimum = 0;   // Set new minimum
                if (speed > xAxis.Maximum)
                    xAxis.Maximum = speed;

                xAxis.Reset();
            }
            var yAxis = plotModel.Axes.FirstOrDefault(a => a.Position == OxyPlot.Axes.AxisPosition.Left);
            if (yAxis != null)
            {
                if (limit > yAxis.Maximum)
                    yAxis.Maximum = limit;
                if (force > yAxis.Maximum)
                    yAxis.Maximum = force;
                if (friction > yAxis.Maximum)
                    yAxis.Maximum = friction;

                if (force < yAxis.Minimum)
                    yAxis.Minimum = force;
                if (friction < yAxis.Minimum)
                    yAxis.Minimum = friction;


                yAxis.Reset();
            }
            plotView1.Invalidate();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lineSeriesForce.Points.Clear();
            lineSeriesLimit.Points.Clear();
            lineSeriesFriction.Points.Clear();
        }

        private void resetAxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var xAxis = plotModel.Axes.FirstOrDefault(a => a.Position == OxyPlot.Axes.AxisPosition.Bottom);
            if (xAxis != null)
            {
                xAxis.Minimum = 0;   // Set new minimum
                xAxis.Maximum = 100; // Set new maximum

                xAxis.Reset();
            }

            var yAxis = plotModel.Axes.FirstOrDefault(a => a.Position == OxyPlot.Axes.AxisPosition.Left);
            if (yAxis != null)
            {
                yAxis.Minimum = 0;
                yAxis.Maximum = 100;
            }
        }

        private void tractiveForceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lineSeriesForce.IsVisible)
            {
                lineSeriesForce.IsVisible = false;
                highlightForce.IsVisible = false;
                contextMenuStrip1.Items[3].Text = "Traction force";
            }
            else
            {
                lineSeriesForce.IsVisible = true;
                highlightForce.IsVisible = true;
                contextMenuStrip1.Items[3].Text = "Traction force ✓";
            }
        }

        private void adhesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lineSeriesLimit.IsVisible)
            {
                lineSeriesLimit.IsVisible = false;
                highlightLimit.IsVisible = false;
                contextMenuStrip1.Items[4].Text = "Adhesion limit";
            }
            else
            {
                lineSeriesLimit.IsVisible = true;
                highlightLimit.IsVisible = true;
                contextMenuStrip1.Items[4].Text = "Adhesion limit ✓";
            }
        }

        private void resistiveForceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lineSeriesFriction.IsVisible)
            {
                lineSeriesFriction.IsVisible = false;
                highlightFriction.IsVisible = false;
                contextMenuStrip1.Items[5].Text = "Resistive force";
            }
            else
            {
                lineSeriesFriction.IsVisible = true;
                highlightFriction.IsVisible = true;
                contextMenuStrip1.Items[5].Text = "Resistive force ✓";
            }
        }
    }
}
