using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShanoEditor.Properties;
using System.IO;
using ShanoEditor.Views;

namespace ShanoEditor
{
    public partial class ShanoEditorForm : Form
    {

        ScenarioControl[] scenarioViews;
        

        public bool StatusLoading
        {
            set
            {
                scenarioLoadProgressBar.Visible = value;
                //if (value)  statusLabel.Text = "Loading...";
                //else        statusLabel.Text = "";
            }
        }
        public bool StatusSaving
        {
            set
            {
                scenarioLoadProgressBar.Visible = value;
                //if (value) statusLabel.Text = "Saving...";
                //else statusLabel.Text = "";
            }
        }

        static IEnumerable<Control> enumControls(Control c)
        {
            var cc = c.Controls.Cast<Control>();
            return cc.Concat(cc.SelectMany(enumControls));
        }


        public ShanoEditorForm()
        {
            InitializeComponent();
            updateRecentsMenu();

            scenarioTree.SelectionChanged += tree_SelectionChanged;

            scenarioViews = splitContainer1.Panel2.Controls
                .OfType<ScenarioControl>()
                .ToArray();

            foreach (var c in scenarioViews)
            {
                c.Dock = DockStyle.Fill;
                c.Visible = false;
                c.ScenarioChanged += scenarioView_ChangedScenario;
            }
        }
        

        void scenarioView_ChangedScenario()
        {
            updateCaption();
        }

        void tree_SelectionChanged(ScenarioViewType ty)
        {
            foreach (var c in scenarioViews)
                if (c.ViewType == ty)
                    c.Show();
                else
                    c.Hide();
        }

        void updateRecentsMenu()
        {
            recentToolStripMenuItem.DropDownItems.Clear();

            var recents = Settings.Default.RecentFiles
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p));

            //if no recent items, create a placeholder item and return
            if(!recents.Any())
            {
                recentToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem("<empty>")
                    {
                        Enabled = false,
                    });
                return;
            }

            recentToolStripMenuItem.DropDownItems.Clear();
            foreach(var path in recents)
            {

                var recentFileMenuItem = new ToolStripMenuItem(path)
                {
                    ToolTipText = path,
                };
                recentFileMenuItem.Click += async (o, e) => { await open(path); };
                recentToolStripMenuItem.DropDownItems.Add(recentFileMenuItem);
            }
        }

        void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            create();
        }

        async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await open();
        }

        async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await save();
        }

        void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: save all files somewhere else
        }

        void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void toolStripButton1_Click(object sender, EventArgs e)
        {
            //checkSyntax();
        }
        

        async void ShanoEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            var wasClosed = await close();
            if (!wasClosed)
                e.Cancel = true;
        }
    }
}
