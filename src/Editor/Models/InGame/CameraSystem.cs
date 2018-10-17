using Shanism.Client;
using Shanism.Client.IO;
using Shanism.Client.UI;
using Shanism.Common;
using Shanism.ScenarioLib;
using System;
using System.Numerics;

namespace Shanism.Editor.Controls
{
    class CameraSystem : ISystem
    {
        public const int MaxCameraArea = 200;
        const float MinInGameZoom = 1;
        const float MaxInGameZoom = 512;

        readonly Control gameFrame;
        readonly ScenarioConfig scenario;

        readonly ScreenSystem screen;
        readonly MouseSystem mouse;

        Vector2? lockedPoint;

        public MouseButton TriggerButton { get; set; } = MouseButton.Right;


        public CameraSystem(IClient game, ScenarioConfig scenario, Control gameFrame)
        {
            this.scenario = scenario;   
            this.gameFrame = gameFrame;

            screen = game.Screen;
            mouse = game.Mouse;

            gameFrame.MouseDown += onMouseDown;
            gameFrame.MouseUp += onMouseUp;
            gameFrame.MouseMove += onMouseMove;
            gameFrame.MouseScroll += onMouseScroll;

            screen.Game.Pan(scenario.Map.Bounds.Center);
            screen.Game.ResetGameZoom();
        }

        public void Update(int msElapsed) { }



        //TODO: rework so domain actions here
        void onMouseScroll(Control sender, MouseScrollArgs e)
        {
            // get clamped resize mult
            var mult = (float)Math.Pow(1.1, e.ScrollDelta);
            ClampToZoom(screen.Game.Size, ref mult);

            // update center & size
            var anchorPoint = mouse.InGamePosition.Clamp(scenario.Map.Bounds);
            var newCenter = anchorPoint + (screen.Game.Center - anchorPoint) * mult;
            var newSize = screen.Game.Size * mult;
            
            screen.Game.Set(newCenter, newSize);
        }

        // resize to zoom etc 

        static void ClampToZoom(Vector2 curSize, ref float mult)
        {
            var newSz = curSize * mult;
            var smallerUnit = Math.Min(newSz.X, newSz.Y);
            var largerUnit = Math.Max(newSz.X, newSz.Y);

            // clamp the multiplier based on the area..
            if (smallerUnit < MinInGameZoom)
                mult *= MinInGameZoom / smallerUnit;

            if (largerUnit > MaxInGameZoom)
                mult *= MaxInGameZoom / largerUnit;
        }

        void onMouseMove(Control sender, MouseArgs e)
        {
            if (lockedPoint != null)
            {
                var curPos = mouse.InGamePosition;
                var dPos = lockedPoint.Value - curPos;
                var center = (screen.Game.Center + dPos).Clamp(scenario.Map.Bounds);

                screen.Game.Pan(center);
            }
        }

        void onMouseUp(Control sender, MouseButtonArgs e)
        {
            if (e.Button == TriggerButton)
                lockedPoint = null;
        }

        void onMouseDown(Control sender, MouseButtonArgs e)
        {
            if (e.Button == TriggerButton)
                lockedPoint = mouse.InGamePosition;
        }
    }
}
