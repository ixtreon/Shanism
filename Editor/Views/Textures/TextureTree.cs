using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shanism.Editor.ViewModels;
using System.IO;
using Shanism.Common;

namespace Shanism.Editor.Views.Content
{


    class TextureTree : TreeView
    {
        private ContextMenuStrip textureMenu;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem btnTextureView;
        private ToolStripMenuItem btnTextureOpenFolder;
        private ContextMenuStrip folderMenu;
        private ToolStripMenuItem btnFolderOpen;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem btnCreateFolder;
        private ToolTip toolTip1;
        private bool _loading;


        public event Action<TextureViewModel> TextureSelected;
        public event Action<TextureViewModel, bool> TextureIncludedChanged;


        public TexturesViewModel Model { get; private set; }

        public void SetModel(TexturesViewModel model)
        {
            Model = model;
            Model.TexturesReloaded += reloadTextures;

            _loading = true;
            reloadTextures();
            _loading = false;
        }

        public TextureTree()
        {
            InitializeComponent();

            ImageList = ImageLists.FolderList;
            CheckBoxes = true;
            DoubleBuffered = true;

            btnTextureView.Click += BtnTextureView_Click;
            btnTextureOpenFolder.Click += BtnTextureOpenFolder_Click;
            btnFolderOpen.Click += BtnFolderOpen_Click;
            btnCreateFolder.Click += async (o, e) => await CreateFolder();
        }

        public async Task CreateFolder()
        {
            const string DefaultName = "New Folder";

            var n = SelectedNode;
            if (n == null) return;
            if (n.Tag is TextureViewModel) n = n.Parent;

            var baseDir = n.Name;

            //TODO
            var relPath = DefaultName;
            var i = 2;
            while (Directory.Exists(Path.Combine(baseDir, relPath)))
                relPath = DefaultName + " " + (i++);

            Directory.CreateDirectory(Path.Combine(baseDir, relPath));

            await Model.Reload();
        }

        #region Context Menu Event Handlers

        void BtnTextureOpenFolder_Click(object sender, EventArgs e)
        {
            var n = SelectedNode.Tag as TextureViewModel;
            if (n == null)
                return;

            var texDir = Path.GetDirectoryName(n.FullPath);
            System.Diagnostics.Process.Start(texDir);
        }

        void BtnTextureView_Click(object sender, EventArgs e)
        {
            var n = SelectedNode.Tag as TextureViewModel;
            if (n == null)
                return;

            try
            {
                System.Diagnostics.Process.Start(n.FullPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open the texture at `{n.FullPath}`. The error was:\n{ex.Message}");
            }
        }

        void BtnFolderOpen_Click(object sender, EventArgs e)
        {
            var path = SelectedNode.Name;

            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                System.Diagnostics.Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open the folder `{path}`. The error was:\n{ex.Message}");
            }
        }

        #endregion


        protected override void OnAfterCheck(TreeViewEventArgs e)
        {
            var n = e.Node;
            var texData = n.Tag as TextureViewModel;

            //recursively select all children, if node is a folder
            if (texData == null)
            {
                selectAll(n, n.Checked);
                return;
            }

            // save to the scenario
            if (!_loading)
            {
                texData.Included = n.Checked;
                TextureIncludedChanged?.Invoke(texData, n.Checked);
            }

            base.OnAfterCheck(e);
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            var texData = e.Node.Tag as TextureViewModel;
            TextureSelected?.Invoke(texData);

            base.OnAfterSelect(e);
        }

        protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SelectedNode = e.Node;

                var texModel = e.Node.Tag as TextureViewModel;
                var cursorPos = Cursor.Position;

                if (texModel != null)
                    textureMenu.Show(cursorPos);
                else
                    folderMenu.Show(cursorPos);
            }
            base.OnNodeMouseClick(e);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
                drgevent.Effect = drgevent.AllowedEffect & DragDropEffects.Copy;
            else if (drgevent.Data.GetDataPresent(typeof(TreeNode)))
                drgevent.Effect = drgevent.AllowedEffect & DragDropEffects.Move;

            base.OnDragEnter(drgevent);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            //select node under mouse pointer
            var pt = PointToClient(new System.Drawing.Point(drgevent.X, drgevent.Y));
            SelectedNode = GetNodeAt(pt) ?? SelectedNode;

            base.OnDragOver(drgevent);
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
            base.OnItemDrag(e);
        }

        protected override async void OnDragDrop(DragEventArgs drgevent)
        {
            var destNode = SelectedNode;

            //copy-paste image files
            if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var draggedFiles = ((string[])drgevent.Data.GetData(DataFormats.FileDrop));

                foreach (var fn in draggedFiles)
                    Model.AddTexture(fn, destNode.Name);

                await Model.Reload();

                SelectedNode = Nodes.Find(destNode.Name, true).FirstOrDefault();
                SelectedNode?.Expand();
            }
            //move other nodes
            else if (drgevent.Data.GetDataPresent(typeof(TreeNode)))
            {
                var incomingNode = (TreeNode)drgevent.Data.GetData(typeof(TreeNode));

                //if destination node is a texture, get its parent folder
                if (destNode.Tag is TextureViewModel)
                    destNode = destNode.Parent;

                if (incomingNode.Tag is TextureViewModel)  //an image file
                {
                    //get its parent directory
                    var tex = ((TextureViewModel)incomingNode.Tag);
                    var parentNode = incomingNode.Parent;

                    if (destNode != parentNode)
                    {
                        Model.MoveTexture(tex, destNode.Name);

                        await Model.Reload();
                    }

                }
                else
                {
                    //a directory. do nothing yet
                }
            }

            base.OnDragDrop(drgevent);
        }

        /// <summary>
        /// Creates the TreeNodes for all relevant directories. 
        /// The <see cref="TreeNode.Name"/> property corresponds to the dir path. 
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="rootDir"></param>
        void buildDirTree(TreeNode rootNode, DirectoryInfo rootDir)
        {
            foreach (var dInfo in rootDir.GetDirectories())
            {
                var dNode = new TreeNode
                {
                    Text = dInfo.Name,
                    Name = dInfo.FullName,
                    ToolTipText = dInfo.FullName,

                    ImageIndex = 0,
                    SelectedImageIndex = 0,
                };
                rootNode.Nodes.Add(dNode);

                buildDirTree(dNode, dInfo);
            }
        }

        /// <summary>
        /// Removes and then adds all textures in the scenario from the tree. 
        /// </summary>
        void reloadTextures()
        {
            Nodes.Clear();

            var contentDirInfo = new DirectoryInfo(Model.ContentDirectory);

            if (!Directory.Exists(contentDirInfo.FullName))
                Directory.CreateDirectory(contentDirInfo.FullName);

            var rootNode = Nodes.Add(contentDirInfo.FullName, contentDirInfo.Name);
            rootNode.ToolTipText = contentDirInfo.FullName;

            //create the directory structure
            buildDirTree(rootNode, contentDirInfo);

            //add textures
            foreach (var tex in Model.Textures.Values)
            {
                //split path into segments
                var texturePathSegments = tex.Path
                    .Split(Path.DirectorySeparatorChar);


                //find or create the folder structure
                var texDir = Path.GetDirectoryName(tex.FullPath);
                var folderNode = Nodes.Find(texDir, true).Single();

                //create the texture node
                var nodeText = Path.GetFileName(tex.FullPath);
                folderNode.Nodes.Add(new TreeNode
                {
                    Text = nodeText,
                    Name = tex.Path,
                    ToolTipText = tex.Path,
                    Tag = tex,

                    Checked = tex.Included,

                    ImageIndex = 1,
                    SelectedImageIndex = 1,
                });
            }

            rootNode.Expand();

        }


        void selectAll(TreeNode n, bool selected)
        {
            foreach (TreeNode cn in n.Nodes)
            {
                cn.Checked = selected;
                selectAll(cn, selected);
            }
        }


        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textureMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnTextureView = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTextureOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.folderMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnFolderOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCreateFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textureMenu.SuspendLayout();
            this.folderMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // textureMenu
            // 
            this.textureMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnTextureView,
            this.btnTextureOpenFolder});
            this.textureMenu.Name = "textureMenu";
            this.textureMenu.Size = new System.Drawing.Size(202, 48);
            // 
            // btnTextureView
            // 
            this.btnTextureView.Name = "btnTextureView";
            this.btnTextureView.Size = new System.Drawing.Size(201, 22);
            this.btnTextureView.Text = "View";
            // 
            // btnTextureOpenFolder
            // 
            this.btnTextureOpenFolder.Name = "btnTextureOpenFolder";
            this.btnTextureOpenFolder.Size = new System.Drawing.Size(201, 22);
            this.btnTextureOpenFolder.Text = "Open Containing Folder";
            // 
            // folderMenu
            // 
            this.folderMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnFolderOpen,
            this.toolStripSeparator1,
            this.btnCreateFolder});
            this.folderMenu.Name = "folderMenu";
            this.folderMenu.Size = new System.Drawing.Size(162, 54);
            // 
            // btnFolderOpen
            // 
            this.btnFolderOpen.Name = "btnFolderOpen";
            this.btnFolderOpen.Size = new System.Drawing.Size(161, 22);
            this.btnFolderOpen.Text = "Open in Explorer";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
            // 
            // btnCreateFolder
            // 
            this.btnCreateFolder.Name = "btnCreateFolder";
            this.btnCreateFolder.Size = new System.Drawing.Size(161, 22);
            this.btnCreateFolder.Text = "New Folder";
            // 
            // TextureTree
            // 
            this.AllowDrop = true;
            this.HideSelection = false;
            this.textureMenu.ResumeLayout(false);
            this.folderMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
