using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbilityIDE
{
    class ScenarioTreeView : TreeView
    {
        
        public TreeNode ProjectRoot { get; private set; }

        public Dictionary<TreeNode, string> LoadScenario(string dir)
        {
            this.Nodes.Clear();

            ProjectRoot = new TreeNode(Path.GetDirectoryName(dir));

            this.Nodes.Add(ProjectRoot);

            var csFiles = Directory.EnumerateFiles(dir, "*.cs", SearchOption.AllDirectories);

            var openedFiles = new Dictionary<TreeNode, string>();

            foreach (var filePath in csFiles)
            {
                var fileName = Path.GetFileName(filePath);

                var fNode = new TreeNode()
                {
                    Name = filePath,
                    Text = fileName,
                };
                openedFiles[fNode] = File.ReadAllText(filePath);


                ProjectRoot.Nodes.Add(fNode);
            }

            return openedFiles;
        }

    }
}
