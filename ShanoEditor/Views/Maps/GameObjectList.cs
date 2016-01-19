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
using IO.Objects;
using IO;

namespace ShanoEditor.Views.Maps
{
    partial class GameObjectList : ScenarioControl
    {
        class GameObjectPanel
        {
            public string Name { get; }

            public List<IGameObject> Objects { get; }

            public FlowLayoutPanel Panel { get; }

            public GameObjectPanel(string name, List<IGameObject> objs, FlowLayoutPanel p)
            {
                Name = name;
                Objects = objs;
                Panel = p;
            }
        }

        List<GameObjectPanel> categories { get; } = new List<GameObjectPanel>();

        int selectedCategory = -1;
        

        public event Action<IGameObject> ObjectSelected;

        public GameObjectList()
        {
            InitializeComponent();
            cbOwner.SelectedIndex = 0;
        }

        protected override async Task LoadModel()
        {
            AddObjects("Units", Model.Scenario.DefinedUnits, true);
            AddObjects("Doodads", Model.Scenario.DefinedDoodads, false);
        }

        void AddObjects(string name, IEnumerable<IGameObject> objects, bool canOwn)
        {
            var objList = objects.ToList();

            //create the panel w/ buttons and add it
            var flowPanel = createPanel(objList, canOwn);
            mainPanel.Controls.Add(flowPanel);
            flowPanel.Visible = false;


            //add category to the drop-down UI
            cbCategory.Items.Add(name);

            categories.Add(new GameObjectPanel(name, objList, flowPanel));

            //select the first menu if none is selected. 
            if (cbCategory.SelectedIndex == -1)
                cbCategory.SelectedIndex = 0;
        }

        void refreshPanelTextures()
        {
            var btns = mainPanel.Controls
                .OfType<FlowLayoutPanel>()
                .SelectMany(p => p.Controls.OfType<GameObjectButton>());


        }

        FlowLayoutPanel createPanel(List<IGameObject> objs, bool canOwn)
        {
            var p = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0),
                Dock = DockStyle.Fill,
            };
            p.VisibleChanged += (o, e) => pOwner.Visible = canOwn;

            foreach(var o in objs)
            {
                var mdl = o.ModelName;
                var anim = Constants.Content.DefaultAnimation;

                var animView = Model.Content.Models.TryGet(mdl)?.Animations.TryGet(anim);

                var btn = new GameObjectButton(o);
                btn.ObjectSelected += onGameObjectClicked;
                btn.SetModel(Model);

                p.Controls.Add(btn);
            }

            return p;
        }


        private void onGameObjectClicked(IGameObject obj)
        {
            ObjectSelected?.Invoke(obj);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = cbCategory.SelectedIndex;
            if(id != selectedCategory)
            {
                if (selectedCategory >= 0 && selectedCategory < categories.Count)
                {
                    categories[selectedCategory].Panel.Visible = false;
                    foreach (var c in categories[selectedCategory].Panel
                        .Controls
                        .OfType<GameObjectButton>())
                        c.Checked = false;
                }

                selectedCategory = id;


                if (selectedCategory >= 0 && selectedCategory < categories.Count)
                    categories[selectedCategory].Panel.Visible = true;
            }
        }

        private void GameObjectList_Load(object sender, EventArgs e)
        {

        }

        private void pOwner_VisibleChanged(object sender, EventArgs e)
        {
            var anchor = pOwner.Visible ? (Control)pOwner : cbCategory;
            var anchorBot = anchor.Bottom + anchor.Margin.Bottom + mainPanel.Margin.Top;
            var d = anchorBot - mainPanel.Top;

            mainPanel.Top += d;
            mainPanel.Height -= d;
        }
    }
}
