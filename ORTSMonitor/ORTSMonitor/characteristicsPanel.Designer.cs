namespace ORTSMonitor
{
    partial class characteristicsPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            plotView1 = new OxyPlot.WindowsForms.PlotView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            clearToolStripMenuItem = new ToolStripMenuItem();
            resetAxesToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            tractiveForceToolStripMenuItem = new ToolStripMenuItem();
            adhesionToolStripMenuItem = new ToolStripMenuItem();
            resistiveForceToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // plotView1
            // 
            plotView1.ContextMenuStrip = contextMenuStrip1;
            plotView1.Dock = DockStyle.Fill;
            plotView1.Location = new Point(0, 0);
            plotView1.Name = "plotView1";
            plotView1.PanCursor = Cursors.Hand;
            plotView1.Size = new Size(284, 261);
            plotView1.TabIndex = 0;
            plotView1.Text = "plotView1";
            plotView1.ZoomHorizontalCursor = Cursors.SizeWE;
            plotView1.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotView1.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { clearToolStripMenuItem, resetAxesToolStripMenuItem, toolStripSeparator1, tractiveForceToolStripMenuItem, adhesionToolStripMenuItem, resistiveForceToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(165, 120);
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new Size(164, 22);
            clearToolStripMenuItem.Text = "&Clear";
            clearToolStripMenuItem.Click += clearToolStripMenuItem_Click;
            // 
            // resetAxesToolStripMenuItem
            // 
            resetAxesToolStripMenuItem.Name = "resetAxesToolStripMenuItem";
            resetAxesToolStripMenuItem.Size = new Size(164, 22);
            resetAxesToolStripMenuItem.Text = "&Reset axes";
            resetAxesToolStripMenuItem.Click += resetAxesToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(161, 6);
            // 
            // tractiveForceToolStripMenuItem
            // 
            tractiveForceToolStripMenuItem.Name = "tractiveForceToolStripMenuItem";
            tractiveForceToolStripMenuItem.Size = new Size(164, 22);
            tractiveForceToolStripMenuItem.Text = "&Tractive Force  ✓";
            tractiveForceToolStripMenuItem.Click += tractiveForceToolStripMenuItem_Click;
            // 
            // adhesionToolStripMenuItem
            // 
            adhesionToolStripMenuItem.Name = "adhesionToolStripMenuItem";
            adhesionToolStripMenuItem.Size = new Size(164, 22);
            adhesionToolStripMenuItem.Text = "&Adhesion  ✓";
            adhesionToolStripMenuItem.Click += adhesionToolStripMenuItem_Click;
            // 
            // resistiveForceToolStripMenuItem
            // 
            resistiveForceToolStripMenuItem.Name = "resistiveForceToolStripMenuItem";
            resistiveForceToolStripMenuItem.Size = new Size(164, 22);
            resistiveForceToolStripMenuItem.Text = "&Resistive force  ✓";
            resistiveForceToolStripMenuItem.Click += resistiveForceToolStripMenuItem_Click;
            // 
            // characteristicsPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 261);
            Controls.Add(plotView1);
            Name = "characteristicsPanel";
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private OxyPlot.WindowsForms.PlotView plotView1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ToolStripMenuItem resetAxesToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tractiveForceToolStripMenuItem;
        private ToolStripMenuItem adhesionToolStripMenuItem;
        private ToolStripMenuItem resistiveForceToolStripMenuItem;
    }
}
