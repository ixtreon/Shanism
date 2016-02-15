using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor.MapAdapter
{
    class GLTestControl : GLControl
    {
        private bool _loaded;

        public GLTestControl()
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            _loaded = true;
        }

        readonly Random rnd = new Random();
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!_loaded) return;

            var c = Color.FromArgb(100, 100, rnd.Next(200, 255));
            SwapBuffers();
        }
    }
}
