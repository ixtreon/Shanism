using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shanism.Editor.ViewModels;

namespace Shanism.Editor.Views
{
    /// <summary>
    /// Shows them defined models in a scenario. 
    /// </summary>
    partial class AnimationsView : ScenarioControl
    {
        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Animations;

        AnimationsViewModel animations => Model.Content.Animations;

        public AnimationsView()
        {
            InitializeComponent();

            animView.Dock = DockStyle.Fill;
            animView.AnimationChanged += onViewAnimationChanged;
            animView.AnimationRenameRequest += onAnimationViewRename;

            animTree.ModelChanged += onAnimationTreeChange;
            animTree.AnimationSelected += onAnimationSelected;

        }

        void onAnimationTreeChange()
        {
            animView.Reload(null);
            MarkAsChanged();
        }

        void onAnimationViewRename(string newName)
        {
            if (animations.ContainsKey(newName))
                return;

            animView.Animation.Name = newName;
            animTree.RefreshSelectedNode();
            MarkAsChanged();
        }

        void onViewAnimationChanged()
        {
            animTree.RefreshSelectedNode();
            MarkAsChanged();
        }

        protected override async Task LoadModel()
        {
            //recreate the tree
            animTree.Load(Model.Content.Animations);
        }


        void onAnimationSelected(AnimationViewModel anim)
        {
            animView.Visible = true;
            
            btnAnimRename.Enabled =
            btnAnimDelete.Enabled = anim != null;

            if (animView.Animation != anim)
                animView.SetAnimation(Model.Content.Textures, anim);
        }


        #region event handlers

        private void btnStripAnimAdd_Click(object sender, EventArgs e)
        {
            var n = animTree.SelectedNode;
            if (n == null)
                return;

            var basePath = n.Name;

            //get fresh animation name
            var animName = "";
            var animId = 0;

            do
            {
                animId++;
                animName = $"{basePath}/animation-{animId}";
            }
            while (animations.ContainsKey(animName));


            //add the node
            animTree.AddNewAnimation(animName, true);
        }

        private void btnStripAnimRename_Click(object sender, EventArgs e)
        {
            animTree.RenameSelectedNode();
        }

        private void btnStripAnimDelete_Click(object sender, EventArgs e)
        {
            animTree.RemoveSelectedNode();
        }
        #endregion

        private void btnReload_Click(object sender, EventArgs e)
        {
            animTree.Load(Model.Content.Animations);
        }
    }
}
