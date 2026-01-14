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
using OxyPlot.WindowsForms;
using System.Reflection;

namespace ORTSMonitor
{
    public partial class TimeChartPanel : DockContent
    {

        List<LineSeries> lineSeries;
        
        List<PlotView> plotViews;

        List<PlotModel> plotModels;
        public List<ItemSelectionPanel.CheckedItem> ListOfItems;

        bool interlock = false;
        bool updateInProgress = false;
        

        public TimeChartPanel()
        {
            InitializeComponent();

            this.Text = "Time chart";

            lineSeries = new List<LineSeries>();
            plotViews = new List<PlotView>();
            plotModels = new List<PlotModel>();


            ListOfItems = new List<ItemSelectionPanel.CheckedItem>();


            //plotView1.Model = plotModel;

            

        }

        public void UpdateList()
        {
            int numOfRows = 0;
            interlock = true;
            while(updateInProgress) { }

            //tableLayoutPanel1.SuspendLayout();
            plotViews.Clear();
            plotModels.Clear();
            lineSeries.Clear();


            foreach(Control ctrl in tableLayoutPanel1.Controls)
            {
                ctrl.Dispose(); 
            }

            tableLayoutPanel1.Controls.Clear();
            
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Clear();

            if (ListOfItems != null)
            {
                foreach (var item in ListOfItems)
                {
                    if (item.IsChecked)
                    {
                        if (numOfRows == 0)
                        {
                            numOfRows = 1;
                        }
                        else
                        {
                            numOfRows++;
                        }
                        tableLayoutPanel1.RowCount = numOfRows;
                        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
                        plotViews.Add(new PlotView());
                        var plotView = plotViews.Last<PlotView>();

                        plotView.Dock = DockStyle.Fill;
                        plotView.Location = new Point(0, 0);
                        plotView.Name = "plotView" + numOfRows.ToString();
                        plotView.PanCursor = Cursors.Hand;
                        plotView.Size = new Size(284, 261);
                        plotView.TabIndex = 0;
                        plotView.Text = item.Text;
                        plotView.ZoomHorizontalCursor = Cursors.SizeWE;
                        plotView.ZoomRectangleCursor = Cursors.SizeNWSE;
                        plotView.ZoomVerticalCursor = Cursors.SizeNS;

                        tableLayoutPanel1.Controls.Add(plotView);
                        tableLayoutPanel1.SetRow(plotView, numOfRows - 1);
                        tableLayoutPanel1.SetColumn(plotView, 1);

                        plotModels.Add(new PlotModel());
                        var plotModel = plotModels.Last<PlotModel>();

                        plotModel.PlotMargins = new OxyThickness(60, 1, 1, 1);

                        plotModel.Title = "";
                        
                        var xAxis = new LinearAxis
                        {
                            Position = AxisPosition.Bottom,
                            Title = "",
                            Minimum = 0,
                            Maximum = 10,
                            IsAxisVisible = false
                        };
                        plotModel.Axes.Add(xAxis);

                        var yAxis = new LinearAxis
                        {
                            Position = AxisPosition.Left,
                            Title = item.Text,
                            Minimum = 0,
                            Maximum = 100
                        };
                        plotModel.Axes.Add(yAxis);


                        plotView.Model = plotModel;


                        lineSeries.Add(new LineSeries());
                        if (lineSeries.Last<LineSeries>() != null)
                        {
                            lineSeries.Last<LineSeries>().Title = item.Text;
                            lineSeries.Last<LineSeries>().MarkerType = MarkerType.None;

                            plotModel.Series.Add(lineSeries.Last<LineSeries>());
                        }
                    }
                }
            }
            foreach (var view in plotViews)
            {
                view.InvalidatePlot(true);
            }

            if (plotModels.Count > 0)
            {
                var lastModel = plotModels.Last();
                if (lastModel != null)
                {
                    var xAxis = lastModel.Axes.FirstOrDefault(a => a.Position == OxyPlot.Axes.AxisPosition.Bottom);
                    if (xAxis != null)
                    {
                        xAxis.Title = "Time";
                        xAxis.IsAxisVisible = true;
                        xAxis.Reset();
                    }
                    lastModel.PlotMargins = new OxyThickness(60, 1, 1, 60);
                }
            }

            //tableLayoutPanel1.ResumeLayout(false);
            

            interlock = false;
            #if false
            string deb = "";

            deb = "Tab rows:" + numOfRows + Environment.NewLine;
            foreach(var model in plotModels) 
            {
                deb += "Model name: " + model.Title + " | " + model.PlotView.ActualModel.Title + " | " + model.Series[0].Title + Environment.NewLine;
            }
            MessageBox.Show(deb);
            #endif

        }

        public void UpdateData(double time, CabControlsData data)
        {
            int j = 0;

            if (interlock == false)
            {
                updateInProgress = true;

                if (ListOfItems != null)
                {
                    for (int i = 0; i < ListOfItems.Count; i++)
                    {
                        if (ListOfItems[i].IsChecked)
                        {
                            lineSeries[j].Points.Add(new DataPoint(time, data.Controls[i].CurrentValue));
                            var yAxis = plotModels[j].Axes.FirstOrDefault(a => a.Position == OxyPlot.Axes.AxisPosition.Left);
                            if (yAxis != null)
                            {
                                yAxis.Minimum = data.Controls[i].MinValue;
                                yAxis.Maximum = data.Controls[i].MaxValue;
                            }
                            j++;
                        }

                    }
                }

                foreach(var model in plotModels)
                {
                    var xAxis = model.Axes.FirstOrDefault(a => a.Position == OxyPlot.Axes.AxisPosition.Bottom);

                    if (xAxis != null)
                    {
                        if (time > xAxis.Maximum)
                            xAxis.Maximum = time;
                    }
                }

                foreach (var view in plotViews)
                {
                    view.InvalidatePlot(true);
                }
                updateInProgress = false;
            }
        }

        
    }
}
