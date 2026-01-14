namespace ORTSMonitor
{
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using OxyPlot.WindowsForms;
    using System.Net.Http;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Windows.Forms;
    using WeifenLuo.WinFormsUI.Docking;

    public partial class Form1 : Form
    {

        private HttpClient httpClient = new HttpClient();
        OpenRailsWebDataReader reader = new OpenRailsWebDataReader("http://localhost:2150");

        ItemSelectionPanel myItemSelectionPanel;
        ConsolePanel myConsolePanel;
        HtmlViewer trackMonitor;
        HtmlViewer trainDriving;
        ChartPanel myChart;
        characteristicsPanel locoChar;
        TimeChartPanel timeChart;
        double timeSpan;
        private DateTime lastTickTime = DateTime.MinValue;
        double distance = 0.0;
        double time = 0.0;

        ProjectFile projectFile;

        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 500; // 500 ms
            timer1.Start();

            toolStripStatusLabel1.Text = "Waiting for OpenRails...";

            

            dockPanel1.Theme = new VS2015LightTheme(); // or another valid theme

            myItemSelectionPanel = new ItemSelectionPanel();
            myItemSelectionPanel.Show(dockPanel1, DockState.DockLeftAutoHide);

            myConsolePanel = new ConsolePanel();
            myConsolePanel.Show(dockPanel1, DockState.DockRight);

            trackMonitor = new HtmlViewer();
            trackMonitor.Text = "Track Monitor";

            trackMonitor.Navigate("http://localhost:2150/TrackMonitor/index.html");
            trackMonitor.Show(dockPanel1, DockState.DockTop);

            trainDriving = new HtmlViewer();
            trainDriving.Text = "Train Driving";

            trainDriving.Navigate("http://localhost:2150/TrainDriving/index.html");
            trainDriving.Show(dockPanel1, DockState.DockBottom);

            myChart = new ChartPanel();
            myChart.Show(dockPanel1, DockState.DockTop);

            locoChar = new characteristicsPanel();
            locoChar.Show(dockPanel1, DockState.DockBottom);

            timeChart = new TimeChartPanel();
            timeChart.ListOfItems = myItemSelectionPanel.CheckedItems;
            timeChart.UpdateList();
            timeChart.Show(dockPanel1, DockState.DockTop);

            projectFile = new ProjectFile();
            timeSpan = 0.0;
            distance = 0.0;

            myItemSelectionPanel.ItemCheckedChanged += ItemSelectionPanel_ItemCheckedChanged;

            myItemSelectionPanel.FormClosed += MyItemSelectionPanel_FormClosed;
            myConsolePanel.FormClosed += MyConsolePanel_FormClosed;

        }

        private void MyConsolePanel_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.ConsoleViewOn = false;
            Properties.Settings.Default.Save();
        }

        private void MyItemSelectionPanel_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.SignalsViewOn = false;
            Properties.Settings.Default.Save();
        }

        private void ItemSelectionPanel_ItemCheckedChanged(object sender, ItemSelectionPanel.ItemCheckedEventArgs e)
        {
            timeChart.ListOfItems = myItemSelectionPanel.CheckedItems;
            timeChart.UpdateList();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            if (lastTickTime != DateTime.MinValue)
            {
                TimeSpan elapsed = now - lastTickTime;
                timeSpan = elapsed.TotalSeconds;
                // Use elapsed.TotalMilliseconds, elapsed.TotalSeconds, etc.
                time = time + timeSpan;
            }
            lastTickTime = now;

            timer1.Stop();

            try
            {
                TrackMonitorData trackData = await reader.ParseTrackMonitorDataAsync();
                CabControlsData cabData = await reader.ParseCabControlsDataAsync();
                string trainDrivingData = await reader.GetTrainDrivingDataAsync(true);

                string forceInfo = await reader.GetHudDataAsStringAsync(4);

                HudResponse commonInfo = await reader.GetHudDataAsync(0);
                string timeValue = null;
                int timeRowIndex = -1;

                for (int i = 0; i < commonInfo.CommonTableRows.Count; i++)
                {
                    var row = commonInfo.CommonTableRows[i];
                    if (row.Count >= 3)
                    {
                        string firstCol = row[0]?.Trim() ?? "";
                        string lastCol = row[2]?.Trim() ?? "";

                        if (firstCol.Equals("Time", StringComparison.OrdinalIgnoreCase))
                        {
                            timeValue = lastCol;
                            timeRowIndex = i;
                            break;
                        }
                    }
                }

                DateTime.TryParse(timeValue, out DateTime parsedDateTime);


                myConsolePanel.ConsoleText = forceInfo;

                

                if (trainDrivingData.StartsWith("Error:"))
                {
                    toolStripStatusLabel1.Text = "OpenRails offline";
                    toolStripStatusLabel1.BackColor = Color.Red;

                    toolStripStatusLabel2.Text = "Refresh rate: waiting ..." ;

                }
                else
                {
                    toolStripStatusLabel1.Text = "OpenRails online";
                    toolStripStatusLabel1.BackColor = Color.Green;

                    toolStripStatusLabel2.Text = "Refresh rate: " + timeSpan.ToString();

                    TrainDrivingData parsedData = await reader.ParseTrainDrivingDataAsync(true);


                    HudForceData hudForceData = reader.ParseHudForceText(forceInfo);

                    // Pøístup k datùm
                    //foreach (var row in trackData.Rows)
                    //{
                    //    if (!row.IsSeparator)
                    //   {
                    //        //Console.WriteLine($"Limit: {row.LimitValue}, Distance: {row.DistanceValue}");
                    //        textBox1.Text += $"Limit: {row.LimitValue}, Distance: {row.DistanceValue}";
                    //    }
                    //}

                    double frictionForce = 0.0;

                    foreach(HudCarForceData car in hudForceData.CarForces)
                    {
                        frictionForce += (car.Friction ?? 0.0) + (car.Tunnel ?? 0.0) + (car.Wind ?? 0.0) + (car.Curve ?? 0.0) - (car.Gravity ?? 0.0);
                    }
                    frictionForce = frictionForce * 0.001;

                    myItemSelectionPanel.UpdateRowSource(cabData);

                    if (parsedData.Distance == null)
                    {

                    }

                    double speed = parsedData.Speed ?? 0.0;
                    
                    if (parsedData.Distance != null) 
                    {
                        distance = parsedData.Distance ?? 0.0;
                    }
                    else
                    {
                        distance += Math.Abs(speed) / 3.6 * timeSpan;
                    }
                    
                    double curve = hudForceData.CarForces[0].CurveRadius ?? 0.0;
                    double grade = hudForceData.CarForces[0].Gradient ?? 0.0;

                    double direction = 0.0;
                    string dirString = (parsedData.Direction ?? "").ToLower();
                    if (dirString == "forward")
                    {
                        direction = 1.0;
                    }
                    else
                    {
                        if (dirString == "reverse")
                        {
                            direction = -1.0;
                        }
                        else
                        {
                            direction = 0.0;
                        }
                    }


                    double force = hudForceData.ForceDetails.AxleDriveForce ?? 0.0;
                    force = force * 0.001;

                    double locoMass = hudForceData.CarForces[0].Mass ?? 1.0;

                    double adhesionLimit = hudForceData.ForceDetails.LocoAdhesion ?? 0.0;
                    adhesionLimit = adhesionLimit * 0.01 * locoMass * 9.81;


                    if (speed < 0.0)
                        speed = -speed;

                    myChart.AddSpeedPoint(distance, speed, parsedData.Limit ?? 0.0, grade * 10.0, curve *0.01);

                    locoChar.AddCharPoint(speed, force * direction, adhesionLimit, frictionForce);

                    timeChart.UpdateData(time, cabData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba pøi stahování: " + ex.Message);
            }
            finally
            {
                timer1.Start();
            }

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    string json = File.ReadAllText(openFileDialog1.FileName);
                    if (json != null)
                    {
                        var options = new JsonSerializerOptions
                        {
                            Converters = { new JsonStringEnumConverter() },
                            WriteIndented = true
                        };

                        projectFile = JsonSerializer.Deserialize<ProjectFile>(json, options);

                        myItemSelectionPanel.CheckedItems = projectFile.CheckedItems;
                        timeChart.ListOfItems = myItemSelectionPanel.CheckedItems;
                        timeChart.UpdateList();

                        foreach (DockContent content in dockPanel1.Contents)
                        {
                            for (int i = 0; i < projectFile.DockWindows.Count; i++)
                            {
                                if (projectFile.DockWindows[i].Name == content.Name)
                                {
                                    content.Visible = projectFile.DockWindows[i].Visible;
                                    content.DockState = projectFile.DockWindows[i].State;
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    projectFile.Name = saveFileDialog1.FileName;

                    projectFile.CheckedItems = myItemSelectionPanel.CheckedItems;

                    foreach (DockContent content in dockPanel1.Contents)
                    {
                        projectFile.DockWindows.Add(new DockWindowSetting(content.Name, content.DockState, Visible));
                    }

                    // Serialize to JSON and save
                    var options = new JsonSerializerOptions
                    {
                        Converters = { new JsonStringEnumConverter() },
                        WriteIndented = true
                    };
                    string json = JsonSerializer.Serialize(projectFile, options);
                    File.WriteAllText(saveFileDialog1.FileName, json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void signalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SignalsViewOn = !Properties.Settings.Default.SignalsViewOn;

            if (Properties.Settings.Default.SignalsViewOn)
            {
                if(myItemSelectionPanel.IsDisposed)
                {
                    myItemSelectionPanel = new ItemSelectionPanel();
                    myItemSelectionPanel.Show(dockPanel1, DockState.DockLeft);
                    myItemSelectionPanel.ItemCheckedChanged += ItemSelectionPanel_ItemCheckedChanged;

                    if(timeChart.IsDisposed == false) 
                    {
                        timeChart.ListOfItems = myItemSelectionPanel.CheckedItems;
                        timeChart.UpdateList();
                    }
                }
                myItemSelectionPanel.Show(dockPanel1);
            }
            else
            {
                if (myItemSelectionPanel.IsDisposed == false)
                {
                    myItemSelectionPanel.Hide();
                }
            }
        }

        private void viewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ConsoleViewOn = !Properties.Settings.Default.ConsoleViewOn;

            if (Properties.Settings.Default.ConsoleViewOn)
            {
                if (myConsolePanel.IsDisposed)
                {
                    myConsolePanel = new ConsolePanel();
                    myConsolePanel.Show(dockPanel1, DockState.DockRight);
                }
                myConsolePanel.Show(dockPanel1);
            }
            else
            {
                if (myConsolePanel.IsDisposed == false)
                {
                    myConsolePanel.Hide();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
