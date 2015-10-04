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
using ScriptLib;
using AbilityIDE.Properties;
using System.IO;

namespace AbilityIDE
{
    public partial class ShanoEditor : Form
    {

        Dictionary<TreeNode, string> Files;

        public ShanoEditor()
        {
            InitializeComponent();
            populateRecentMenu();
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
                string fileName;
                try
                {
                    fileName = Path.GetFileName(path);
                }
                catch(ArgumentException e)
                {
                    continue;
                }

                var recentFileMenuItem = new ToolStripMenuItem(fileName)
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
            //TODO: save all modified files
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

            if (Files.ContainsKey(n))
            {
                var txt = Files[n];

                cCodeEditor.Text = txt;
            }
        }

        //TODO: REMOVE

        ScenarioCompiler compiler = new ScenarioCompiler();

        private int lastEdit = 0;

        private async void checkSyntax()
        {
            var ticket = Interlocked.Increment(ref lastEdit);

            await Task.Delay(500);

            //if (lastEdit == ticket)
            //{
            //    var result = compiler.CompileFiles(new[] { scenarioView1.SelectedNode.Name }, "kur.dll");

            //    if (result.Success)
            //    {

            //    }
            //    else
            //    {
            //        foreach (var d in result.Diagnostics)
            //        {
            //            if (d.Location.IsInSource)
            //            {
            //                richTextBox1.Select(d.Location.SourceSpan.Start, d.Location.SourceSpan.End);
            //                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Underline);
            //            }
            //        }
            //    }
            //}
        }
    }
}
