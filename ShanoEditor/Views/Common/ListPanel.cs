using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor.Views
{
    /// <summary>
    /// A panel that contains other panel and displays them in a fancy list. 
    /// </summary>
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
                resizeExpandedPanel();

                expandedPanel.Expanded = true;
            }
        }

        public ListPanel()
        {
            InitializeComponent();
        }

        void resizeExpandedPanel()
        {
            if (expandedPanel != null)
                expandedPanel.MainPanel.Height = Height - panels.Sum(p => p.CaptionLabel.Height) - 1;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //the stick makes sure all controls resize nicely. 
            stick.Width = flowPanel.Width;

            resizeExpandedPanel();
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

        public Button CaptionLabel { get; }

        public Control MainPanel { get; }

        public event Action<ListedPanel> CaptionClicked
        {
            add { CaptionLabel.Click += (o, e) => value(this); }
            remove { throw new NotImplementedException(); }
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
