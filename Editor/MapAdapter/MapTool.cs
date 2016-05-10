using Shanism.Client;
using Shanism.Common;
using Shanism.Common.Game;
using Shanism.Common.Message;
using Microsoft.Xna.Framework.Graphics;
using Shanism.ScenarioLib;
using Shanism.Editor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shanism.Editor.MapAdapter
{
    abstract class MapTool : IDisposable
    {

        protected IEditorEngine Engine { get; }

        protected CompiledScenario Scenario => Engine.ScenarioView.Scenario;

        protected MapConfig Map => Scenario.Config.Map;

        public event Action<IOMessage> MessageSent;

        protected MapTool(IEditorEngine engine)
        {
            Engine = engine;
        }

        public virtual void OnMouseMove(MouseButtons btn, Vector inGamePos) { }
        public virtual void OnMouseDown(MouseButtons btn, Vector inGamePos) { }
        public virtual void OnMouseUp(MouseButtons btn, Vector inGamePos) { }
        public virtual void OnDraw(IEditorMapControl control, Vector inGamePos) { }

        public virtual void OnKeyPress(KeyEventArgs e) { }

        protected void SendMessage(IOMessage m) => MessageSent?.Invoke(m);

        public virtual void Dispose() { }
    }
}
