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
    public partial class ItemSelectionPanel : DockContent
    {
        public ItemSelectionPanel()
        {
            InitializeComponent();
            this.Text = "Signals";
            CheckedItems = new List<CheckedItem>();
        }
        public class CheckedItem
        {
            public string? Text { get; set; }
            public bool IsChecked { get; set; }
        }

        public List<ItemSelectionPanel.CheckedItem> list;

        public List<ItemSelectionPanel.CheckedItem> CheckedItems
        {
            get
            {
                return list;
            }

            set
            {
                checkedListBox1.BeginUpdate();
                checkedListBox1.Items.Clear();
                foreach (ItemSelectionPanel.CheckedItem item in value)
                {
                    if (item.Text != null)
                        checkedListBox1.Items.Add(item.Text, item.IsChecked);
                }
                list = value;
                checkedListBox1.EndUpdate();
            }
        }
        public void UpdateRowSource(CabControlsData cabData)
        {
            checkedListBox1.BeginUpdate();

            // Build a dictionary of existing items and their checked state
            var existingItems = new Dictionary<string, bool>();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                string item = checkedListBox1.Items[i].ToString();
                bool isChecked = checkedListBox1.GetItemChecked(i);
                existingItems[item] = isChecked;
            }

            // Add new controls if they don't already exist
            if (cabData?.Controls != null)
            {
                foreach (var control in cabData.Controls)
                {
                    string display = $"{control.FormattedTypeName}";
                    if (!existingItems.ContainsKey(display))
                    {
                        checkedListBox1.Items.Add(display, false); // Add as unchecked by default
                    }
                }
            }

            // Optionally, remove items that no longer exist in cabData.Controls
            // (If you want to keep the list in sync, otherwise skip this step)
            var currentDisplays = new HashSet<string>(
                cabData.Controls.Select(c => $"{c.FormattedTypeName}")
            );
            for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
            {
                string item = checkedListBox1.Items[i].ToString();
                if (!currentDisplays.Contains(item))
                {
                    checkedListBox1.Items.RemoveAt(i);
                }
            }

            checkedListBox1.EndUpdate();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string itemText = checkedListBox1.Items[e.Index].ToString();
            int numOfItems = checkedListBox1.Items.Count;
            bool newChecked = e.NewValue == CheckState.Checked;

            CheckedItems.Clear();   

            for (int i = 0; i < checkedListBox1.Items.Count; i++) 
            {
                CheckedItems.Add(new CheckedItem());
                var checkedItem = CheckedItems.Last<CheckedItem>();
                checkedItem.Text = checkedListBox1.Items[i].ToString();
                checkedItem.IsChecked = checkedListBox1.GetItemChecked(i);
            }

            CheckedItems[e.Index].IsChecked = newChecked;

            ItemCheckedChanged?.Invoke(this, new ItemCheckedEventArgs(itemText, newChecked, e.Index));
        }

        public event EventHandler<ItemCheckedEventArgs> ItemCheckedChanged;

        public class ItemCheckedEventArgs : EventArgs
        {
            public string ItemText { get; }
            public bool NewCheckedState { get; }

            public int Index { get; }

            public ItemCheckedEventArgs(string itemText, bool newCheckedState, int index)
            {
                ItemText = itemText;
                NewCheckedState = newCheckedState;
                Index = index;
            }
        }
    }
}
