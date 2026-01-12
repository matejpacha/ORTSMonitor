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

        ProjectFile projectFile;

        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 250; // 250 ms
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

            projectFile = new ProjectFile();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            try
            {
                TrackMonitorData trackData = await reader.ParseTrackMonitorDataAsync();
                CabControlsData cabData = await reader.ParseCabControlsDataAsync();
                string trainDrivingData = await reader.GetTrainDrivingDataAsync(true);

                string forceInfo = await reader.GetHudDataAsStringAsync(4);



                myConsolePanel.ConsoleText = forceInfo;

                

                if (trainDrivingData.StartsWith("Error:"))
                {
                    toolStripStatusLabel1.Text = "OpenRails offline";
                    toolStripStatusLabel1.BackColor = Color.Red;

                }
                else
                {
                    toolStripStatusLabel1.Text = "OpenRails online";
                    toolStripStatusLabel1.BackColor = Color.Green;
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
                    double distance = parsedData.Distance ?? 0.0;

                    double force = hudForceData.ForceDetails.AxleDriveForce ?? 0.0;
                    force = force * 0.001;
                    double adhesionLimit = hudForceData.ForceDetails.LocoAdhesion ?? 0.0;


                    if (speed < 0.0)
                        speed = -speed;

                    myChart.AddSpeedPoint(distance, speed, parsedData.Limit ?? 0.0);

                    locoChar.AddCharPoint(speed, force, adhesionLimit, frictionForce);
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
                myItemSelectionPanel.Hide();
            }
            else
            {
                myItemSelectionPanel.Show();
            }
        }

        private void viewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ConsoleViewOn = !Properties.Settings.Default.ConsoleViewOn;

            if (Properties.Settings.Default.ConsoleViewOn)
            {
                myConsolePanel.Hide();
            }
            else
            {
                myConsolePanel.Show();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
