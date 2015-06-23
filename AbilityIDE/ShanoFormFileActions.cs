using AbilityIDE.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IO;
using ScriptEngine.Parsers;

namespace AbilityIDE
{
    public partial class ShanoEditor : Form
    {

        /// <summary>
        /// Opens an existing scenario. 
        /// </summary>
        private void open(string filePath = null)
        {
            if(string.IsNullOrEmpty(filePath) && openDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openDialog.SelectedPath;
            }

            Settings.Default.UpdateRecentFiles(filePath);

            var sd = new ScenarioParser(filePath);

            System.IO.Directory.SetCurrentDirectory(filePath);

            treeContent.LoadScenario(filePath);
        }

        private void addToRecent(string path)
        {
            const int maxRecent = 10;
            var recentFiles = Settings.Default.RecentFiles.Split('\n');
            Settings.Default.RecentFiles = openDialog.SelectedPath + '\n' + recentFiles.Take(maxRecent - 1).Aggregate((a, b) => a + '\n' + b);
            Settings.Default.Save();
            recentToolStripMenuItem.DropDownItems.Clear();
        }
    }
}
