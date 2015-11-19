using ShanoEditor.ScenarioViews;
using ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor
{
    class ScenarioTree : TreeView
    {

        ScenarioFile _scenario;
        public ScenarioFile Scenario
        {
            get { return _scenario; }
            set
            {
                if (value != _scenario)
                {
                    _scenario = value;
                    reload();
                }
            }
        }
        //public MapFile Map { get; private set; }
        public ContentConfig Models { get; private set; }

        /// <summary>
        /// Always one 
        /// </summary>
        public event Action<ScenarioViewType> SelectionChanged;

        //Loads the given scenario to the scenariotree 
        void reload()
        {
            Nodes.Clear();

            if (Scenario == null)
                return;

            var childNodes = Enum.GetValues(typeof(ScenarioViewType))
                .Cast<ScenarioViewType>()
                .OrderBy(sc => sc)
                .Select(sc => new TreeNode
                {
                    Text = sc.ToString(),
                    Tag = sc,
                })
                .ToArray();

            //10,20,... -> main
            // 21, 22 -> child
            TreeNode lastNode = null;
            foreach(var node in childNodes)
            {
                if (((int)node.Tag) % 10 != 0 && lastNode != null)
                    lastNode.Nodes.Add(node);
                else
                    Nodes.Add(lastNode = node);
            }
        }

        public ScenarioTree()
        {
            this.AfterSelect += ScenarioTree_AfterSelect;
        }

        private void ScenarioTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!(e.Node.Tag is ScenarioViewType))
                return;
            var id = (ScenarioViewType)e.Node.Tag;
            SelectionChanged?.Invoke(id);

        }
    }
}
