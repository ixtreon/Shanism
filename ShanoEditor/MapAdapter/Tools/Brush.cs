using IO.Common;
using IO.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor.MapAdapter
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
                SendMessage(Place(inGamePos));
            }
        }

        public override void OnMouseMove(MouseButtons btn, Vector inGamePos)
        {
            if (btn == MouseButtons.Left && isPlacing)
                SendMessage(Place(inGamePos));
        }

        public override void OnMouseUp(MouseButtons btn, Vector inGamePos)
        {
            if (btn == MouseButtons.Left)
                isPlacing = false;
        }

        public abstract IOMessage Place(Vector inGamePos);

    }
}
