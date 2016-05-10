using Shanism.Editor.Views;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shanism.Editor
{
    class ScenarioTree : TreeView
    {

        public ScenarioConfig Scenario { get; private set; }

        public event Action<ScenarioViewType> SelectionChanged;

        //Loads the given scenario to the scenariotree 
        public void SetScenario(ScenarioConfig sc)
        {
            Scenario = sc;
            if (Scenario == null)
                return;

            Nodes.Clear();

            var nDetails = new TreeNode
            {
                Text = "Details",
                Tag = ScenarioViewType.Details,
            };
            var nMap = new TreeNode
            {
                Text = "Map",
                Tag = ScenarioViewType.Map,
            };

            var nContent = new TreeNode { Text = "Content" };
            var nTextures = new TreeNode
            {
                Text = "Textures",
                Tag = ScenarioViewType.Textures,
            };
            var nAnimations = new TreeNode
            {
                Text = "Animations",
                Tag = ScenarioViewType.Animations,
            };

            Nodes.Add(nDetails);
            Nodes.Add(nMap);
            Nodes.Add(nContent);

            nContent.Nodes.Add(nTextures);
            nContent.Nodes.Add(nAnimations);
        }

        public ScenarioTree()
        {
            AfterSelect += onAfterSelect;
        }

        void onAfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is ScenarioViewType)
                SelectionChanged?.Invoke((ScenarioViewType)e.Node.Tag);
        }
    }
}
