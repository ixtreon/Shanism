using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shanism.Editor.MapAdapter
{
    abstract class Brush : MapTool
    {
        bool isPlacing;

        public Brush(IEditorEngine e)
            : base(e)
        {

        }

        public override void OnMouseDown(MouseButtons btn, Vector inGamePos)
        {
            if (btn == MouseButtons.Left)
            {
                isPlacing = true;
                Place(inGamePos);
            }
        }

        public override void OnMouseMove(MouseButtons btn, Vector inGamePos)
        {
            if (btn == MouseButtons.Left && isPlacing)
                Place(inGamePos);
        }

        public override void OnMouseUp(MouseButtons btn, Vector inGamePos)
        {
            if (btn == MouseButtons.Left)
                isPlacing = false;
        }

        public abstract void Place(Vector inGamePos);

    }
}
