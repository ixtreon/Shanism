using Client;
using IO;
using IO.Common;
using IO.Message;
using Microsoft.Xna.Framework.Graphics;
using ScenarioLib;
using ShanoEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShanoEditor.MapAdapter
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
