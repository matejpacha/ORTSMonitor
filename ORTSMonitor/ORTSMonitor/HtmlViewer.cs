using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.WinForms;

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
    public partial class HtmlViewer : DockContent
    {
        private WebView2 webBrowser;
        public HtmlViewer()
        {
            InitializeComponent();
            this.Text = "HTML Viewer";
            webBrowser = new WebView2 { Dock = DockStyle.Fill };
            this.Controls.Add(webBrowser);
        }

        public async Task InitializeAsync()
        {
            await webBrowser.EnsureCoreWebView2Async();
        }

        public void Navigate(string url)
        {
            // C#
            webBrowser.Source = new System.Uri(url);
        }

        public void SetText(string text)
        {
            this.Text = text;
        }
    }
}
