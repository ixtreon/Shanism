using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScenarioLib;
using ShanoRPGWin.Properties;
using System.IO;

namespace ShanoRPGWin.UI.Scenarios
{
    /// <summary>
    /// A control that lists the library folders with scenarios in them. 
    /// </summary>
    public partial class LibTree : TreeView
    {
        private Dictionary<string, ScenarioLibrary> librariesInUse = new Dictionary<string, ScenarioLibrary>();

        public bool Loaded { get; private set; }

        public event Action OnRefresh;
        public event Action OnLoaded;

        private ToolTip tip = new ToolTip();

        public LibTree()
        {
            InitializeComponent();

            ShowNodeToolTips = true;
            VisibleChanged += LibTree_VisibleChanged;
        }

        private void LibTree_VisibleChanged(object sender, EventArgs e)
        {
            //remove empty nodes ._.
            foreach (var n in Nodes.Find("", true))
                Nodes.Remove(n);
        }

        /// <summary>
        /// Loads the libraries listed in app settings to this tree. 
        /// </summary>
        public async Task LoadAsync()
        {
            if (Loaded)
            {
                await RefreshLibs();
                return;
            }

            Loaded = true;

            //load libs from app settings
            await Task.Run(() =>
            {
                librariesInUse = Settings.Default.ScenarioLibrary
                    .Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => new ScenarioLibrary(s))
                    .ToDictionary(l => l.DirectoryPath, l => l);
            });
            //search for scenarios in them libs
            await RefreshLibs();

            OnLoaded?.Invoke();
        }

        /// <summary>
        /// Asks each library to update its list of scenarios. 
        /// </summary>
        public async Task RefreshLibs()
        {
            Nodes.Clear();
            foreach (var lib in librariesInUse.Values)
            {
                await lib.Refresh();
                addLibraryNode(lib);
            }
            OnRefresh?.Invoke();
        }

        /// <summary>
        /// Saves the libraries listed in this tree to the app settings. 
        /// </summary>
        public void Save()
        {
            Settings.Default.ScenarioLibrary = librariesInUse
                .Select(sc => sc.Key)
                .Aggregate((a, b) => a + '\t' + b);
            Settings.Default.Save();
        }


        /// <summary>
        /// Adds a new library. 
        /// </summary>
        /// <param name="path"></param>
        public void AddLibrary(string path)
        {
            if (librariesInUse.ContainsKey(path))
                return;

            var lib = new ScenarioLibrary(path);
            librariesInUse.Add(path, lib);
            Save();

            //add treestuff
            addLibraryNode(lib);
            Sort();
        }


        /// <summary>
        /// Removes a library. 
        /// </summary>
        /// <param name="path">The directory of the library. </param>
        public void RemoveLibrary(string path)
        {
            ScenarioLibrary lib;
            if (!librariesInUse.TryGetValue(path, out lib))
                throw new Exception("Lib doesn't exist!");

            librariesInUse.Remove(path);
            Save();

            //remove treestuff
            removeLibraryNode(lib);
            Sort();
        }

        public void RemoveScenario(string path)
        {
            //TODO: remove a scenario folder?!
            MessageBox.Show("Under construction! (just delete the folder yourself and click refresh)");
        }

        /// <summary>
        /// Tries to find the given scenario inside this tree. 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ScenarioFile FindScenario(string path)
        {
            return Nodes.Find(path, true).FirstOrDefault()?.Tag as ScenarioFile;
        }

        /// <summary>
        /// Adds a node for this library, removing its old one if needed. 
        /// </summary>
        /// <param name="lib"></param>
        void addLibraryNode(ScenarioLibrary lib)
        {
            //remove if exists
            removeLibraryNode(lib);

            //add again
            var libName = GetShortenedPath(lib.DirectoryPath);
            var libNode = Nodes.Add(lib.DirectoryPath, libName);
            libNode.ToolTipText = lib.DirectoryPath;

            var scNodes = lib.Scenarios
                .Select(sc => new TreeNode
                {
                    ToolTipText = sc.BaseDirectory,
                    Name = sc.BaseDirectory,
                    Text = sc.Name,
                    Tag = sc,
                })
                .ToArray();
            libNode.Nodes.AddRange(scNodes);
        }

        void removeLibraryNode(ScenarioLibrary lib)
        {
            var existing = Nodes.Find(lib.DirectoryPath, false).FirstOrDefault();
            if (existing != null)
                Nodes.Remove(existing);
        }

        public event Action<ScenarioLibrary> SelectedLibrary;
        public event Action<ScenarioFile, ScenarioLibrary> SelectedScenario;
        public event Action SelectionCleared;

        private void LibTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //check whether a library or a scenario
            var nodeKey = e.Node.Name;

            ScenarioLibrary lib = null;
            if (librariesInUse.TryGetValue(nodeKey, out lib))
            {
                //clicked a library
                SelectedLibrary?.Invoke(lib);
                return;
            }

            ScenarioFile sc;
            var parentKey = e.Node.Parent?.Name ?? string.Empty;
            if (librariesInUse.TryGetValue(parentKey, out lib) && lib.TryGet(nodeKey, out sc))
            {
                //clicked a scenario
                SelectedScenario?.Invoke(sc, lib);
                return;
            }

            //something unknown was clickd..
            SelectionCleared?.Invoke();
        }

        static string GetShortenedPath(string path)
        {
            var di = new DirectoryInfo(path);
            var root = di.Root.Name;
            var dir = di.Name;
            if (root == dir)
                return path;

            if (root.Last() != '\\') root += '\\';
            if (dir.First() != '\\') dir = '\\' + dir;
            return root + "..." + dir;
        }
    }
}
