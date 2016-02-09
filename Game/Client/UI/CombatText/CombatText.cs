using IO.Common;
using IO.Message.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;
namespace Client.UI.CombatText
{
    class CombatText : Control
    {
        static int DefaultDuration = 1000;

        static Vector textAcceleration = new Vector(0, 5);
        static Vector textSpeed = new Vector(0.75, -2.5);


        int textDirection = 1;
        
          
        class TextData
        {
            public Color Color;
            public Vector Location;
            public Vector Velocity = textSpeed;

            public string Text;
            public int Duration;
        }

        readonly HashSet<TextData> labels = new HashSet<TextData>();

        public CombatText()
        {
            CanHover = false;
        }

        public void AddDamageLabel(DamageEventMessage msg)
        {
            //get unit position
            var unit = ObjectGod.Default.TryGet(msg.UnitId);
            if (unit == null)
                return;

            var labelPos = Screen.GameToUi(unit.Position);

            //get damagetype color
            var color = Color.Red;


            var td = new TextData
            {
                Color = color,
                Location = unit.Position, //shitty UI coordinates ;z
                Text = msg.ValueChange.ToString("0"),
                Duration = DefaultDuration,
            };
            textDirection *= -1;
            td.Velocity.X *= textDirection;

            labels.Add(td);
        }

        protected override void OnUpdate(int msElapsed)
        {
            List<TextData> toRemove = new List<TextData>();
            foreach(var td in labels)
            {
                td.Duration -= msElapsed;
                if (td.Duration < 0)
                    toRemove.Add(td);
                td.Velocity += textAcceleration * msElapsed / 1000;
                td.Location += td.Velocity * msElapsed / 1000;
            }

            foreach (var td in toRemove)
                labels.Remove(td);

            Maximize();
        }

        public override void OnDraw(Graphics g)
        {
            foreach(var td in labels)
            {
                var screenPos = Screen.GameToScreen(td.Location);
                Content.Fonts.NormalFont.DrawStringScreen(g.SpriteBatch, td.Text, td.Color, screenPos, 0.5f, 0.5f);
            }
        }
    }
}
