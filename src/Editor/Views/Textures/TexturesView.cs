using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shanism.Common.Content;
using Shanism.Editor.ViewModels;
using Shanism.Common.Util;
using Shanism.Editor.Views.Textures;

namespace Shanism.Editor.Views
{
    partial class TexturesView : ScenarioControl
    {

        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Textures;

        ShanoTreeNode selectedNode;

        public TexturesView()
        {
            InitializeComponent();

            texBrowser.SelectionChanged += texBrowser_SelectionChanged;
            texBrowser.TextureChanged += texBrowser_TextureChanged;
            texBrowser.SelectedNodeChanged += texBrowser_SelectedNodeChanged;

            texBox.AnimationsCreated += texBox_AnimationsCreated;
        }

        void texBox_AnimationsCreated(AnimationDef[] anims)
        {
            foreach (var anim in anims)
                Model.Content.Animations.Add(new AnimationViewModel(Model.Content.Textures, anim));

            //select first guyyy
        }

        void texBrowser_SelectedNodeChanged(ShanoTreeNode n)
        {
            selectedNode = n;
        }

        void texBrowser_TextureChanged(TextureNode node)
        {
            if (node == null || propGrid.SelectedObject == node.Texture)
                propGrid.Refresh();

            MarkAsChanged();
        }

        protected override Task LoadModel()
        {
            texBrowser.SetModel(Model.Content.Textures);

            return Task.CompletedTask;
        }

        void texBrowser_SelectionChanged(TextureViewModel[] texs)
        {
            var tex = texs.Length == 1 ? texs[0] : null;

            propGrid.SelectedObjects = texs;
            texBox.SetTexture(tex);
        }

        void propGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            //update texture preview & tree (including folders above)
            texBox.Refresh();

            selectedNode.RefreshParents();
            selectedNode.RefreshRecurse();  //in case it's a folder

            MarkAsChanged();
        }
    }
}
