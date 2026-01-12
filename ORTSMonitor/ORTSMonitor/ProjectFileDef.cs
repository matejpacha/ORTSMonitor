using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;

namespace ORTSMonitor
{
    public class ProjectFile
    {
        public string ? Name { get; set; }
        public List<ItemSelectionPanel.CheckedItem> CheckedItems { get; set; } = new List<ItemSelectionPanel.CheckedItem>();

        public List<DockWindowSetting> DockWindows { get; set; } = new List<DockWindowSetting>();
        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
    }

    public class DockWindowSetting
    {
        public string Name { get; set; }
        public DockState State { get; set; }
        public bool Visible { get; set; }

        public DockWindowSetting() { }

        public DockWindowSetting(string name, DockState state, bool visible)
        {
            Name = name;
            State = state;
            Visible = visible;
        }
    }

}
