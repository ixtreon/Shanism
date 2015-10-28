using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IO.Content;

namespace AbilityIDE.ScenarioViews
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ModelsView : ScenarioView
    {
        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Models;

        private Dictionary<string, ModelDef> Models;


        public Dictionary<string, TextureView> TextureDictionary { get; set; }

        TreeNode RootNode;

        public ModelsView()
        {
            InitializeComponent();
        }

        protected override async Task LoadScenario()
        {
            Models = Scenario.ModelConfig.Models
                .ToDictionary(m => m.Name, m => m);
            await ReloadTree();
        }


        public override void SaveScenario()
        {
            base.SaveScenario();
        }

        //reloads the nodes in the tree using the Models var
        async Task ReloadTree()
        {
            modelTree.Nodes.Clear();
            RootNode = modelTree.Nodes.Add("Models");

            //add each model
            foreach(var kvp in Models)
            {
                var modelNode = new TreeNode
                {
                    Text = kvp.Key,
                };
                RootNode.Nodes.Add(modelNode);

                //add each animation
                foreach (var anim in kvp.Value.Animations)
                {
                    modelNode.Nodes.Add(anim.Key);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

        }

        private void modelTree_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
