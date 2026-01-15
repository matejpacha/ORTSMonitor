namespace ORTSMonitor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            timer1 = new System.Windows.Forms.Timer(components);
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolStripStatusLabel3 = new ToolStripStatusLabel();
            toolStripStatusLabel2 = new ToolStripStatusLabel();
            dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            toolStripContainer1 = new ToolStripContainer();
            menuStrip1 = new MenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripMenuItem2 = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            signalsToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem1 = new ToolStripMenuItem();
            tractionCharToolStripMenuItem = new ToolStripMenuItem();
            timeChartToolStripMenuItem = new ToolStripMenuItem();
            distanceChartToolStripMenuItem = new ToolStripMenuItem();
            trackMonitorToolStripMenuItem = new ToolStripMenuItem();
            trainDrivingToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            statusStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.TopToolStripPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Interval = 500;
            timer1.Tick += timer1_Tick;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, toolStripStatusLabel3, toolStripStatusLabel2 });
            statusStrip1.Location = new Point(0, 695);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1009, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(39, 17);
            toolStripStatusLabel1.Text = "Status";
            // 
            // toolStripStatusLabel3
            // 
            toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            toolStripStatusLabel3.Size = new Size(57, 17);
            toolStripStatusLabel3.Text = "[Address]";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new Size(69, 17);
            toolStripStatusLabel2.Text = "Refresh rate";
            // 
            // dockPanel1
            // 
            tableLayoutPanel1.SetColumnSpan(dockPanel1, 2);
            dockPanel1.Dock = DockStyle.Fill;
            dockPanel1.Location = new Point(3, 3);
            dockPanel1.Name = "dockPanel1";
            tableLayoutPanel1.SetRowSpan(dockPanel1, 2);
            dockPanel1.Size = new Size(1003, 665);
            dockPanel1.TabIndex = 4;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(dockPanel1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1009, 671);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            toolStripContainer1.ContentPanel.Controls.Add(tableLayoutPanel1);
            toolStripContainer1.ContentPanel.Size = new Size(1009, 671);
            toolStripContainer1.Dock = DockStyle.Fill;
            toolStripContainer1.Location = new Point(0, 0);
            toolStripContainer1.Name = "toolStripContainer1";
            toolStripContainer1.Size = new Size(1009, 695);
            toolStripContainer1.TabIndex = 6;
            toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            toolStripContainer1.TopToolStripPanel.Controls.Add(menuStrip1);
            // 
            // menuStrip1
            // 
            menuStrip1.Dock = DockStyle.None;
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, viewToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1009, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripSeparator1, toolStripMenuItem2 });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(37, 20);
            toolStripMenuItem1.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(180, 22);
            newToolStripMenuItem.Text = "&New...";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(180, 22);
            openToolStripMenuItem.Text = "&Open...";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(180, 22);
            saveToolStripMenuItem.Text = "&Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(180, 22);
            saveAsToolStripMenuItem.Text = "Save as...";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(180, 22);
            toolStripMenuItem2.Text = "&Exit";
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { signalsToolStripMenuItem, viewToolStripMenuItem1, tractionCharToolStripMenuItem, timeChartToolStripMenuItem, distanceChartToolStripMenuItem, trackMonitorToolStripMenuItem, trainDrivingToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(44, 20);
            viewToolStripMenuItem.Text = "&View";
            // 
            // signalsToolStripMenuItem
            // 
            signalsToolStripMenuItem.Name = "signalsToolStripMenuItem";
            signalsToolStripMenuItem.Size = new Size(149, 22);
            signalsToolStripMenuItem.Text = "&Signals";
            signalsToolStripMenuItem.Click += signalsToolStripMenuItem_Click;
            // 
            // viewToolStripMenuItem1
            // 
            viewToolStripMenuItem1.Name = "viewToolStripMenuItem1";
            viewToolStripMenuItem1.Size = new Size(149, 22);
            viewToolStripMenuItem1.Text = "&Console";
            viewToolStripMenuItem1.Click += viewToolStripMenuItem1_Click;
            // 
            // tractionCharToolStripMenuItem
            // 
            tractionCharToolStripMenuItem.Name = "tractionCharToolStripMenuItem";
            tractionCharToolStripMenuItem.Size = new Size(149, 22);
            tractionCharToolStripMenuItem.Text = "&Traction char";
            tractionCharToolStripMenuItem.Click += tractionCharToolStripMenuItem_Click;
            // 
            // timeChartToolStripMenuItem
            // 
            timeChartToolStripMenuItem.Name = "timeChartToolStripMenuItem";
            timeChartToolStripMenuItem.Size = new Size(149, 22);
            timeChartToolStripMenuItem.Text = "T&ime chart";
            timeChartToolStripMenuItem.Click += timeChartToolStripMenuItem_Click;
            // 
            // distanceChartToolStripMenuItem
            // 
            distanceChartToolStripMenuItem.Name = "distanceChartToolStripMenuItem";
            distanceChartToolStripMenuItem.Size = new Size(149, 22);
            distanceChartToolStripMenuItem.Text = "&Distance chart";
            distanceChartToolStripMenuItem.Click += distanceChartToolStripMenuItem_Click;
            // 
            // trackMonitorToolStripMenuItem
            // 
            trackMonitorToolStripMenuItem.Name = "trackMonitorToolStripMenuItem";
            trackMonitorToolStripMenuItem.Size = new Size(149, 22);
            trackMonitorToolStripMenuItem.Text = "Trac&k monitor";
            trackMonitorToolStripMenuItem.Click += trackMonitorToolStripMenuItem_Click;
            // 
            // trainDrivingToolStripMenuItem
            // 
            trainDrivingToolStripMenuItem.Name = "trainDrivingToolStripMenuItem";
            trainDrivingToolStripMenuItem.Size = new Size(149, 22);
            trainDrivingToolStripMenuItem.Text = "Trai&n driving";
            trainDrivingToolStripMenuItem.Click += trainDrivingToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.DefaultExt = "json";
            openFileDialog1.Filter = "JSON file|*.json|All files|*.*";
            openFileDialog1.Title = "Open file";
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.DefaultExt = "json";
            saveFileDialog1.FileName = "myProjectFile.json";
            saveFileDialog1.Filter = "JSON files|*.json|All files|*.*";
            saveFileDialog1.Title = "Save file";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1009, 717);
            Controls.Add(toolStripContainer1);
            Controls.Add(statusStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "OpenRails CZ/SK Web Monitor";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer1.TopToolStripPanel.PerformLayout();
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private TableLayoutPanel tableLayoutPanel1;
        private ToolStripContainer toolStripContainer1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem toolStripMenuItem2;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem signalsToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripMenuItem tractionCharToolStripMenuItem;
        private ToolStripMenuItem timeChartToolStripMenuItem;
        private ToolStripMenuItem distanceChartToolStripMenuItem;
        private ToolStripMenuItem trackMonitorToolStripMenuItem;
        private ToolStripMenuItem trainDrivingToolStripMenuItem;
        private ToolStripStatusLabel toolStripStatusLabel3;
    }
}
