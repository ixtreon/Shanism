using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShanoEditor.ViewModels;

namespace ShanoEditor.Views.Content
{
    partial class TextureBrowser : UserControl
    {
        TexturesViewModel Model;

        public event Action<TextureViewModel> TextureSelected;
        public event Action<TextureViewModel, bool> TextureCheckedChanged;

        public void SetModel(TexturesViewModel model)
        {
            Model = model;
            texTree.SetModel(model);
            texTree.TextureSelected += (t) => TextureSelected?.Invoke(t);
            texTree.TextureIncludedChanged += (t, v) => TextureCheckedChanged?.Invoke(t, v);
        }

        public TextureBrowser()
        {
            InitializeComponent();
        }

        async void btnReload_Click(object sender, EventArgs e)
        {
            await Model.Reload();
        }

        async void btnCreateFolder_Click(object sender, EventArgs e)
        {
            await texTree.CreateFolder();
        }
    }
}
