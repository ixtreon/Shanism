using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using ShanoRpgWinGl.Sprites;
using Microsoft.Xna.Framework;

namespace ShanoRpgWinGl.UI
{
    class SpellBar : Control
    {
        const int MaxButtonCount = 20;

        private int _buttonCount;
        public int ButtonCount
        {
            get { return _buttonCount; }
            set
            {
                if (_buttonCount == value)
                    return;
                if (_buttonCount < value)
                {
                    for (int i = _buttonCount; i < value; i++)
                    {
                        //Debug.Assert(SpellButtons[i] == null);
                        SpellButtons.Add(new SpellButton()
                        {
                            Size = new Microsoft.Xna.Framework.Vector2(ButtonSize, ButtonSize),
                            RelativePosition = new Microsoft.Xna.Framework.Vector2(i * ButtonSize, 0),
                        });
                        Add(SpellButtons[i]);
                    }
                }
                else
                {
                    for (int i = value; i < _buttonCount; i++)
                        Remove(SpellButtons[i]);
                    SpellButtons.RemoveRange(value, _buttonCount - value);
                }
                _buttonCount = value;
                this.Size = new Microsoft.Xna.Framework.Vector2(_buttonCount * ButtonSize, ButtonSize);
            }
        }

        public float ButtonSize { get; private set; }

        private List<SpellButton> SpellButtons = new List<SpellButton>();

        public SpellBar(int btnCount = 6)
        {
            ButtonSize = 0.12f;
            this.ButtonCount = btnCount;
        }

        public override void Draw(SpriteBatch sb)
        {
            var backColor = new Color(50, 50, 50, 200);
            SpriteCache.BlankTexture.Draw(sb, ScreenPosition, ScreenSize, backColor);
            base.Draw(sb);
        }
    }
}
