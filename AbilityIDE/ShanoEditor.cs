﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AbilityIDE.Properties;
using System.IO;
using AbilityIDE.ScenarioViews;

namespace AbilityIDE
{
    public partial class ShanoEditor : Form
    {

        ScenarioView[] scenarioViews;


        public bool StatusLoading
        {
            set
            {
                if (value)  statusLabel.Text = "Loading...";
                else        statusLabel.Text = "";
            }
        }
        public bool StatusSaving
        {
            set
            {
                if (value) statusLabel.Text = "Saving...";
                else statusLabel.Text = "";
            }
        }

        static IEnumerable<Control> enumControls(Control c)
        {
            var cc = c.Controls.Cast<Control>();
            return cc.Concat(cc.SelectMany(enumControls));
        }


        public ShanoEditor()
        {
            InitializeComponent();
            populateRecentMenu();

            scenarioTree.SelectionChanged += tree_SelectionChanged;



            scenarioViews = enumControls(this)
                .Cast<Control>()
                .Where(c => typeof(ScenarioView).IsAssignableFrom(c.GetType()))
                .Cast<ScenarioView>()
                .ToArray();

            foreach (var c in scenarioViews)
            {
                c.Dock = DockStyle.Fill;
                c.ScenarioChanged += scenarioView_ChangedScenario;
            }
        }

        private void scenarioView_ChangedScenario()
        {
            updateUi();
        }

        private void tree_SelectionChanged(ScenarioViewType ty)
        {
            var toShow = scenarioViews.FirstOrDefault(v => v.ViewType == ty);

            //hide
            foreach (var c in scenarioViews)
                if(c != toShow)
                    c.Hide();
            //show
            toShow?.Show();
        }

        private void populateRecentMenu()
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

            foreach(var path in recents)
            {

                var recentFileMenuItem = new ToolStripMenuItem(path)
                {
                    ToolTipText = path,
                };
                recentFileMenuItem.Click += (o, e) => { open(path); };
                recentToolStripMenuItem.DropDownItems.Add(recentFileMenuItem);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: create a new scenario. from a template?
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            open();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: save all files somewhere else
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //checkSyntax();
        }

        private void scenarioView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var n = e.Node;

            //if (Files.ContainsKey(n))
            //{
            //    var txt = Files[n];

            //    cCodeEditor.Text = txt;
            //}
        }

    }
}