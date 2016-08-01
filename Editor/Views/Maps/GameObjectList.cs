using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shanism.ScenarioLib;
using Shanism.Common.StubObjects;
using Shanism.Common;
using Shanism.Common.Interfaces.Entities;

namespace Shanism.Editor.Views.Maps
{
    partial class GameObjectList : ScenarioControl
    {
        class GameObjectPanel
        {
            public string Name { get; }

            public List<IEntity> Objects { get; }

            public FlowLayoutPanel Panel { get; }

            public GameObjectPanel(string name, List<IEntity> objs, FlowLayoutPanel p)
            {
                Name = name;
                Objects = objs;
                Panel = p;
            }
        }

        List<GameObjectPanel> categories { get; } = new List<GameObjectPanel>();

        int selectedCategoryId = -1;


        public event Action<IEntity> ObjectSelected;

        public GameObjectList()
        {
            InitializeComponent();
            cbOwner.SelectedIndex = 0;
        }

        protected override Task LoadModel()
        {
            if (Model?.Scenario == null)
                return Task.CompletedTask;

            var units = Model.Scenario.DefinedEntities
                .Where(o => o is IUnit);

            var doodads = Model.Scenario.DefinedEntities
                .Where(o => o is IDoodad);

            if (units.Any())
                AddObjects("Custom Units", units, true);
            if (doodads.Any())
                AddObjects("Custom Doodads", doodads, false);

            return Task.CompletedTask;
        }

        void AddObjects(string name, IEnumerable<IEntity> objects, bool canOwn)
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

        FlowLayoutPanel createPanel(List<IEntity> objs, bool canOwn)
        {
            var p = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0),
                Dock = DockStyle.Fill,
            };
            p.VisibleChanged += (o, e) => pOwner.Visible = canOwn;

            foreach (var o in objs)
            {
                var animView = Model.Content
                    .Animations.TryGet(o.Model);

                var btn = new GameObjectButton(o);
                btn.ObjectSelected += onGameObjectClicked;
                btn.SetModel(Model);

                p.Controls.Add(btn);
            }

            return p;
        }


        void onGameObjectClicked(IEntity obj)
        {
            ObjectSelected?.Invoke(obj);
        }

        void onCategoryChanged(object sender, EventArgs e)
        {
            var id = cbCategory.SelectedIndex;
            if (id == selectedCategoryId)
                return;

            //if old category valid, make it invisible
            if (selectedCategoryId >= 0 && selectedCategoryId < categories.Count)
            {
                var selectedCategory = categories[selectedCategoryId];

                selectedCategory.Panel.Visible = false;
                foreach (var c in selectedCategory.Panel
                    .Controls
                    .OfType<GameObjectButton>())
                    c.Checked = false;
            }

            selectedCategoryId = id;

            //if new category valid, make it visible
            if (selectedCategoryId >= 0 && selectedCategoryId < categories.Count)
                categories[selectedCategoryId].Panel.Visible = true;
        }


        void pOwner_VisibleChanged(object sender, EventArgs e)
        {
            //resize mainPanel if object can be owned
            var anchor = pOwner.Visible ? (Control)pOwner : cbCategory;
            var anchorBot = anchor.Bottom + anchor.Margin.Bottom + mainPanel.Margin.Top;
            var d = anchorBot - mainPanel.Top;

            mainPanel.Top += d;
            mainPanel.Height -= d;
        }
    }
}
