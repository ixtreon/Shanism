using System;
using System.Collections.Generic;
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
        AnimationViewModel animation;

        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Animations;

        AnimationsViewModel animations => Model.Content.Animations;


        public AnimationsView()
        {
            InitializeComponent();

            animTree.ModelChanged += animTree_ModelChanged;
            animTree.AnimationSelected += animTree_AnimationSelected;

            propGrid.PropertyValueChanged += propGrid_PropertyValueChanged;

            texBox.SelectionChanged += texBox_SelectionChanged;

        }


        protected override Task LoadModel()
        {
            //recreate the tree
            animTree.Load(Model.Content.Animations);

            return Task.CompletedTask;
        }


        #region Event Handlers

        void propGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.Label)
            {
                case nameof(AnimationViewModel.Period):
                    animBox.Refresh();
                    break;

                case nameof(AnimationViewModel.Texture):
                    texBox.SetAnimation(animation);
                    animBox.Refresh();
                    break;

                case nameof(AnimationViewModel.Name):
                    animTree.RefreshSelectedNode();
                    break;
            }

            MarkAsChanged();
        }

        void texBox_SelectionChanged()
        {
            //model
            animation.SetSpan(texBox.Selection);

            //view
            propGrid.Refresh();
            animBox.Refresh();
            animTree.RefreshSelectedNode();

            MarkAsChanged();
        }

        void animTree_ModelChanged()
        {
            propGrid.Refresh();
            animBox.Refresh();

            MarkAsChanged();
        }


        void animTree_AnimationSelected(AnimationViewModel anim)
        {
            animation = anim;

            propGrid.SelectedObject = animation;
            animBox.SetAnimation(animation);
            texBox.SetAnimation(animation);

            Utils.TextureTypeConverter.SetTextures(Model.Content.Textures.Textures.ToArray());

            btnAnimRename.Enabled =
            btnAnimDelete.Enabled = anim != null;
        }

        #endregion


        #region Context Menu Event Handlers

        void btnStripAnimAdd_Click(object sender, EventArgs e)
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

        void btnStripAnimRename_Click(object sender, EventArgs e)
        {
            animTree.RenameSelectedNode();
        }

        void btnStripAnimDelete_Click(object sender, EventArgs e)
        {
            animTree.RemoveSelectedNode();
        }

        void btnReload_Click(object sender, EventArgs e)
        {
            animTree.Load(Model.Content.Animations);
        }

        #endregion
    }
}
