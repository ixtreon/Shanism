using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor.ScenarioViews.Maps
{
    public partial class ListPanel : UserControl
    {
        List<ListedPanel> panels { get; } = new List<ListedPanel>();

        ListedPanel expandedPanel;

        public void AddPanel(Control p, string caption)
        {
            //create, hook to click
            var lp = new ListedPanel(flowPanel, p, caption);
            lp.CaptionClicked += p_CaptionClicked;

            //expand if first, mark as such
            lp.Expanded = !panels.Any();
            if (lp.Expanded)
                expandedPanel = lp;

            panels.Add(lp);

        }

        void p_CaptionClicked(ListedPanel p)
        {
            if (expandedPanel != p)
            {
                expandedPanel.Expanded = false;
                expandedPanel = p;
                expandedPanel.Expanded = true;
            }
        }

        public ListPanel()
        {
            InitializeComponent();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            stick.Width = flowPanel.Width;
        }
    }


    class ListedPanel : IDisposable
    {
        readonly FlowLayoutPanel master;

        public string Caption
        {
            get { return CaptionLabel.Text; }
            set { CaptionLabel.Text = value; }
        }

        Button CaptionLabel { get; }

        Control MainPanel { get; }

        public event Action<ListedPanel> CaptionClicked
        {
            add { CaptionLabel.Click += (o, e) => value(this); }
            remove { CaptionLabel.Click -= (o, e) => value(this); }
        }

        public bool Expanded
        {
            get { return MainPanel.Visible; }
            set
            {
                MainPanel.Visible = value;
                CaptionLabel.BackColor = 
                    value ? SystemColors.ControlDarkDark : SystemColors.ControlDark;
            }
        }

        public ListedPanel(FlowLayoutPanel master, Control toAdd, string caption)
        {
            this.master = master;

            MainPanel = toAdd ?? new Panel();
            MainPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MainPanel.Margin = new Padding(0);

            CaptionLabel = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(0),

                FlatStyle = FlatStyle.Flat,
                BackColor = SystemColors.ControlDark,

                TextAlign = ContentAlignment.MiddleLeft,
                Size = new Size(666, 25),
                Font = new Font(FontFamily.GenericSansSerif, 11.25f, FontStyle.Bold),

                Text = caption,
            };
            CaptionLabel.FlatAppearance.BorderSize = 0;
            CaptionLabel.FlatAppearance.MouseOverBackColor = SystemColors.ControlDarkDark;

            master.Controls.Add(CaptionLabel);
            master.Controls.Add(MainPanel);
        }

        public void Dispose()
        {
            master.Controls.Remove(CaptionLabel);
            master.Controls.Remove(MainPanel);
        }
    }

}
