using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shanism.Engine.Objects;
using Shanism.ScenarioLib;

namespace Shanism.Editor.Views.Maps
{
    public partial class SelectionWindow : UserControl
    {
        List<Entity> objects = new List<Entity>();

        public SelectionWindow()
        {
            InitializeComponent();
            BackColor = Color.Red.SetAlpha(0.3f);
        }


        public void SetObjects(IEnumerable<Entity> objs)
        {
            objects = objs?.ToList() ?? new List<Entity>();

            Visible = objects.Any();

            if (Visible)
            {
                if (objects.Count > 1)
                    lblCaption.Text = $"<{objects.Count} Entities>";
                else
                    lblCaption.Text = objects.Single().Name;

                propertyGrid1.SelectedObjects = objects
                    .Select(o => o.Data)
                    .OfType<ObjectConstructor>()
                    .ToArray();
            }

            Invalidate();
        }

        public void ClearObjects() => SetObjects(null);

    }
}
