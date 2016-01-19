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

namespace ShanoEditor.Views
{
    /// <summary>
    /// 
    /// </summary>
    partial class ModelsView : ScenarioControl
    {
        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Models;

        TreeNode RootNode;

        public ModelsView()
        {
            InitializeComponent();

            animView.Dock = DockStyle.Fill;

            updateUi();
        }
        Dictionary<string, ModelDef> Models
        {
            get { return Model.Content.Models; }
        }

        protected override async Task LoadModel()
        {
            //recreate the tree
            modelTree.Nodes.Clear();

            //root
            RootNode = modelTree.Nodes.Add("Models");
            RootNode.ContextMenuStrip = rootRightClick;

            //nodes
            foreach (var kvp in Models)
            {
                var modelDef = kvp.Value;
                var anims = modelDef.Animations.ToArray();

                //add each model
                var modelNode = addModelNode(kvp.Key, modelDef);

                //and its animations
                foreach (var anim in anims)
                    addAnimNode(kvp.Value, modelNode, anim.Key, anim.Value);
            }
        }

        
        //adds a treenode for the given model
        TreeNode addModelNode(string modelKey, ModelDef val)
        {
            //insert into tree
            if (RootNode.Nodes.ContainsKey(modelKey))
                return null;

            var modelNode = new TreeNode
            {
                Text = modelKey,
                Name = modelKey,
                Tag = val,
                ContextMenuStrip = modelRightClick,
            };
            RootNode.Nodes.Add(modelNode);

            MarkAsChanged();
            return modelNode;
        }

        /// <summary>
        /// Adds a new animation. Returns false if the model doesn't exist,
        /// or the animation already exists in the model.  
        /// </summary>
        TreeNode addAnimNode(ModelDef model, TreeNode modelNode, string animName, AnimationDef animVal)
        {
            //add treenode
            var animNode = new TreeNode
            {
                Text = animName,
                Name = animName,
                Tag = animVal,
                ContextMenuStrip = animRightClick,
            };
            modelNode.Nodes.Add(animNode);

            MarkAsChanged();
            return animNode;
        }



        void startRename(TreeNode n)
        {
            if (n == null || n == RootNode)
                return;

            modelTree.SelectedNode = n;
            n.BeginEdit();
        }

        bool finishRename(TreeNode n, string newName)
        {
            var oldName = n?.Text;
            if (n == RootNode 
                || oldName == newName 
                || string.IsNullOrEmpty(oldName)
                || string.IsNullOrEmpty(newName))
                return false;

            if(n.Tag is ModelDef)
            {
                if (Models.ContainsKey(newName))
                    return false;
                var modelDef = (ModelDef)n.Tag;
                Models.Remove(n.Text);
                modelDef.Name = newName;
                Models[newName] = modelDef;
            }
            else // animation
            {
                var modelDef = n.Parent.Tag as ModelDef;
                if (modelDef == null)
                    throw new Exception();

                if (modelDef.Animations.ContainsKey(newName))
                    return false;

                modelDef.Animations.Remove(n.Text);
                modelDef.Animations[newName] = (AnimationDef)n.Tag;
            }
            n.Name = newName;
            MarkAsChanged();
            return true;
        }

        /// <summary>
        /// Adding a new model. 
        /// </summary>
        void addUiModel()
        {
            //get a shitty name
            var i = 0;
            string nodeName;
            do
            {
                nodeName = "Model " + (++i);
            }
            while (Models.ContainsKey(nodeName));

            var model = new ModelDef(nodeName);

            Models.Add(nodeName, model);

            //add the node, mark for rename
            var n = addModelNode(nodeName, new ModelDef(nodeName));
            startRename(n);
        }

        /// <summary>
        /// Adds a new animation to the given model. 
        /// </summary>
        void addUiAnim(string modelKey)
        {
            
            var modelNode = RootNode.Nodes.Find(modelKey, false).FirstOrDefault();
            if (modelNode == null)
                return;

            var model = (ModelDef)modelNode.Tag;

            //get a shitty name
            var i = 0;
            string nodeName;
            do
            {
                nodeName = "Anim " + (++i);
            }
            while (model.Animations.ContainsKey(nodeName));

            //add to the model
            var anim = new AnimationDef();
            model.Animations[nodeName] = anim;

            //add the tree node, rename
            var n = addAnimNode(model, modelNode, nodeName, anim);
            startRename(n);
        }

        /// <summary>
        /// Removes the model with the given key. 
        /// </summary>
        void removeModel(string modelKey)
        {
            var modelNode = RootNode.Nodes.Find(modelKey, false)
                .FirstOrDefault();
            if (!Models.ContainsKey(modelKey) || modelNode == null)
                return;

            Models.Remove(modelKey);
            RootNode.Nodes.Remove(modelNode);

            MarkAsChanged();
        }

        /// <summary>
        /// Removes an animation from the model. 
        /// </summary>
        void removeAnim(string modelKey, string animKey)
        {
            var modelNode = RootNode.Nodes.Find(modelKey, false).FirstOrDefault();
            if (!Models.ContainsKey(modelKey) || modelNode == null)
                return;
            var model = Models[modelKey];

            var animNode = modelNode.Nodes.Find(animKey, false).FirstOrDefault();
            if (!model.Animations.ContainsKey(animKey) || animNode == null)
                return;

            model.Animations.Remove(animKey);
            animNode.Remove();
        }

        void updateUi()
        {
            var n = modelTree.SelectedNode;
            var isRoot = n == RootNode;
            var isAnim = n != null && n.Tag is AnimationDef;
            var isModel = n != null && n.Tag is ModelDef;

            animView.Visible = isAnim;

            btnAddAnim.Enabled = 
            btnRename.Enabled =
            btnDelete.Enabled = isAnim || isModel;

            if(isModel)
            {
                n.Expand();
            }
            else if (isAnim)
            {
                animView.SetAnimation((AnimationDef)n.Tag);
            }
        }

        #region event handlers
        private void modelTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            updateUi();
        }

        private void modelTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            e.CancelEdit = !finishRename(e.Node, e.Label);
        }

        private void modelTree_KeyDown(object sender, KeyEventArgs e)
        {
            var n = modelTree.SelectedNode;
            switch(e.KeyCode)
            {
                case Keys.F2:
                    startRename(n);
                    break;
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            startRename(modelTree.SelectedNode);
        }

        private void btnAddModel_Click(object sender, EventArgs e)
        {
            addUiModel();
        }

        private void btnAddAnim_Click(object sender, EventArgs e)
        {
            var n = modelTree.SelectedNode;
            if (n == null)
                return;
            
            if(n.Tag is ModelDef)
                addUiAnim(n.Text);
            else if (n.Tag is AnimationDef)
                addUiAnim(n.Parent.Text);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var n = modelTree.SelectedNode;

            if (!string.IsNullOrEmpty(n?.Text))
            {
                if (modelTree.SelectedNode.Tag is ModelDef)
                    removeModel(n.Text);
                else
                    removeAnim(n.Parent.Text, n.Text);
            }
        }
        #endregion
    }
}
