using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IO.Objects;
using IO;

namespace ShanoEditor.ScenarioViews.Maps
{

    public class GameObjectButton : Button
    {
        const int normalBorderSize = 1;
        const int selectedBorderSize = 3;

        public event Action<GameObjectButton> UserClicked;

        bool _isSelected = false;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                FlatAppearance.BorderSize = value ? selectedBorderSize : normalBorderSize;
            }
        }

        public IGameObject GameObject { get; }

        public GameObjectButton(IGameObject proto)
        {
            Size = new Size(64, 64);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = normalBorderSize;

            GameObject = proto;
            Text = proto.Name;
        }

        protected override void OnClick(EventArgs e)
        {
            IsSelected = true;
            UserClicked?.Invoke(this);

            base.OnClick(e);
        }
    }
}
