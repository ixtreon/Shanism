using Microsoft.Xna.Framework;
using Shanism.Client.UI.Common;
using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Menus
{
    class OptionsWindow : Window
    {

        readonly FlowPanel mainPanel;
        readonly Button btnApply;

        //Graphics
        readonly CheckBox vSync;
        readonly SliderLabel renderSize;

        //Game
        readonly CheckBox extendCast;

        public OptionsWindow()
        {
            //MaximumSize = MinimumSize = Size = new Vector(0.8, 1.0);
            CanFocus = false;
            Location = (new Vector(2, 1) - Size) / 2;
            ParentAnchor = AnchorMode.None;
            TitleText = "Options";
            VisibleChanged += onVisibleChanged;

            Add(btnApply = new Button
            {
                Location = Size - Button.DefaultSize - LargePadding,
                ParentAnchor = AnchorMode.Right | AnchorMode.Bottom,

                Text = "Apply",
            });
            btnApply.MouseUp += BtnAccept_MouseUp;

            Add(mainPanel = new FlowPanel
            {
                Location = new Vector(LargePadding, TitleHeight + LargePadding),
                Size = Size - LargePadding * 2 - new Vector(0, TitleHeight + Padding + btnApply.Size.Y),
                ParentAnchor = AnchorMode.All,

                Direction = FlowDirection.Vertical,
            });


            addHeader("Graphics");

            mainPanel.Add(vSync = new CheckBox
            {
                Size = new Vector(mainPanel.Size.X - 2 * Padding, 0.07),
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,

                Text = "VSync",
            });
            vSync.CheckedChanged += vSyncBox_CheckedChanged;

            mainPanel.Add(renderSize = new SliderLabel
            {
                Size = new Vector(mainPanel.Size.X - 2 * Padding, 0.07),
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,

                ToolTip = "Size of the game render",
            });
            renderSize.Label.Text = "Render Size";
            renderSize.Slider.MinValue = 0.05;
            renderSize.Slider.MaxValue = 1.00;
            renderSize.Slider.ValueChanged += renderSizeSlider_ValueChanged;


            addHeader("Game");

            mainPanel.Add(extendCast = new CheckBox
            {
                Size = new Vector(mainPanel.Size.X - 2 * Padding, 0.07),
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,

                Text = "Extend Cast",
                ToolTip = "Allow casting even if the mouse cursor is out of range.",
            });
            extendCast.CheckedChanged += ExtendCast_CheckedChanged;
        }

        void addHeader(string name)
        {
            mainPanel.Add(new Label
            {
                AutoSize = false,
                Size = new Vector(mainPanel.Size.X - 2 * Padding, 0.1),
                ParentAnchor = AnchorMode.Left | AnchorMode.Right | AnchorMode.Top,

                TextXAlign = 0.5f,
                Text = name,
            });
        }

        void onVisibleChanged(Control obj)
        {
            if (IsVisible)
            {
                Settings.Reload();
                loadSettings();
            }
        }

        void loadSettings()
        {
            renderSize.Slider.Value = Settings.Current.RenderSize;
            vSync.IsChecked = Settings.Current.VSync;
            extendCast.IsChecked = Settings.Current.ExtendCast;
        }

        void BtnAccept_MouseUp(Input.MouseButtonArgs obj)
        {
            Settings.Current.Save();
            IsVisible = false;
        }

        void renderSizeSlider_ValueChanged(Slider obj)
        {
            Settings.Current.RenderSize = (float)renderSize.Slider.Value;
        }

        void vSyncBox_CheckedChanged(CheckBox obj)
        {
            Settings.Current.VSync = vSync.IsChecked;
        }

        void ExtendCast_CheckedChanged(CheckBox obj)
        {
            Settings.Current.ExtendCast = extendCast.IsChecked;
        }
    }
}
