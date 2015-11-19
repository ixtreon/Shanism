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

namespace ShanoEditor.ScenarioViews.Maps
{
    partial class GameObjectList : ScenarioView
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

        GameObjectButton selectedButton;
        int selectedCategory = -1;

        /// <summary>
        /// Gets the currently selected game object. 
        /// </summary>
        public IGameObject SelectedObject { get { return selectedButton.GameObject; } }


        public GameObjectList()
        {
            InitializeComponent();
        }

        public void AddObjects(string name, IEnumerable<IGameObject> objects)
        {
            var objList = objects.ToList();

            //create the panel w/ buttons and add it
            var flowPanel = createPanel(objList);
            mainPanel.Controls.Add(flowPanel);
            flowPanel.Visible = false;


            //add category to the drop-down UI
            cbCategory.Items.Add(name);

            categories.Add(new GameObjectPanel(name, objList, flowPanel));
        }

        void refreshPanelTextures()
        {
            var btns = mainPanel.Controls
                .OfType<FlowLayoutPanel>()
                .SelectMany(p => p.Controls.OfType<GameObjectButton>());


        }

        FlowLayoutPanel createPanel(List<IGameObject> objs)
        {
            var p = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0),
                Dock = DockStyle.Fill,
            };

            foreach(var o in objs)
            {
                var btn = new GameObjectButton(o);
                btn.UserClicked += onGameObjectClicked;
                p.Controls.Add(btn);
            }

            return p;
        }


        private void onGameObjectClicked(GameObjectButton btn)
        {
            if (selectedButton != null && selectedButton != btn)
                selectedButton.IsSelected = false;
            selectedButton = btn;
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = cbCategory.SelectedIndex;
            if(id != selectedCategory)
            {
                if (selectedCategory >= 0 && selectedCategory < categories.Count)
                    categories[selectedCategory].Panel.Visible = false;
                selectedCategory = id;
                if (selectedCategory >= 0 && selectedCategory < categories.Count)
                    categories[selectedCategory].Panel.Visible = true;
            }
        }
    }
}
