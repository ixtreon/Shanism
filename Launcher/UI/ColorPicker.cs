using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoRPGWin.UI
{
    public partial class ColorPicker : UserControl
    {
        private static ToolTip tip = new ToolTip()
        {
            AutomaticDelay = 0
        };

        public delegate void ColorEventHandler(Color c);

        public ColorPicker()
        {
            InitializeComponent();
        }

        private Button selectedButton;


        public event ColorEventHandler ColorSelected;

        public void Add(Color c, string ColorName = "")
        {
            Button b = new Button();
            b.FlatStyle = FlatStyle.Flat;
            b.BackColor = c;
            b.Margin = new System.Windows.Forms.Padding(1);
            b.Size = new Size(colorPanel.Height - 4, colorPanel.Height - 4);
            b.Click += b_Click;

            tip.SetToolTip(b, ColorName);

            colorPanel.Controls.Add(b);

        }

        void b_Click(object sender, EventArgs e)
        {
            if (selectedButton != null)
                selectedButton.FlatAppearance.BorderSize = 1;

            selectedButton = (Button)sender;

            selectedButton.FlatAppearance.BorderSize = 2;

            if (ColorSelected != null)
            {
                ColorSelected(selectedButton.BackColor);
            }
        }


        protected override void OnResize(EventArgs e)
        {
            
            base.OnResize(e);
        }
    }
}
