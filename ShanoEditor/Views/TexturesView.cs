using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using IO;
using IO.Content;
using ShanoEditor.ViewModels;

namespace ShanoEditor.Views
{
    partial class TexturesView : ScenarioControl
    {

        public override ScenarioViewType ViewType { get; } = ScenarioViewType.Textures;

        TextureViewModel CurrentTexture { get; set; }

        public TexturesView()
        {
            InitializeComponent();

            texBrowser.TextureSelected += onTextureSelected;
            texBrowser.TextureCheckedChanged += onTextureCheckedChanged;
        }

        protected override async Task LoadModel()
        {
            texBrowser.SetModel(Model.Content.Textures);
        }

        void onTextureCheckedChanged(TextureViewModel tex, bool isChecked)
        {
            pTexProps.Enabled = tex.Included;
            MarkAsChanged();
        }

        void onTextureSelected(TextureViewModel texData)
        {
            pTexProps.Enabled = texData?.Included ?? false;

            if (texData == null)
            {
                pTexProps.SelectedObject = null;
                texView.Reset();
                return;
            }

            //update the right panel
            texView.SetTexture(texData);
            pTexProps.SelectedObject = texData;
        }

        void pTexProps_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var it = e.ChangedItem;

            texView.Invalidate();
            MarkAsChanged();
        }
        
    }
}
