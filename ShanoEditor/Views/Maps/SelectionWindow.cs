using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor.Views.Maps
{
    public partial class SelectionWindow : UserControl
    {
        public SelectionWindow()
        {
            InitializeComponent();
            BackColor = Color.Red.SetAlpha(0.3f);
        }
    }
}
