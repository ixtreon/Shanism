using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using IO.Common;
using Color = Microsoft.Xna.Framework.Color;


namespace Client.UI
{
    class SpellBar : Control
    {
        const int MaxButtonCount = 64;

        int _buttonsPerRow = 8;

        public static double ButtonSize { get; } = 0.1;

        public int MaxButtonsPerRow
        {
            get { return _buttonsPerRow; }
            set
            {
                _buttonsPerRow = value;
                reflow();
            }
        }

        public int ButtonCount
        {
            get { return Controls.Count(); }
            set
            {
                updateButtonCount(value);
            }
        }

        void updateButtonCount(int newCount)
        {
            var oldCount = Controls.Count();
            if (oldCount == newCount)
                return;

            if (oldCount < newCount)
            {
                foreach (var i in Enumerable.Range(oldCount, newCount - oldCount))
                    Add(new SpellButton
                    {
                        Size = new Vector(ButtonSize, ButtonSize),
                    });
            }
            else
            {
                foreach (var i in Enumerable.Range(newCount, oldCount - newCount))
                    Remove(controls[i]);
            }

            reflow();
        }

        void reflow()
        {
            foreach (var i in Enumerable.Range(0, ButtonCount))
                controls[i].Location = new Vector((i % MaxButtonsPerRow) * ButtonSize, (i / MaxButtonsPerRow) * ButtonSize);

            var w = Math.Min(ButtonCount, MaxButtonsPerRow);
            var h = Math.Max(1, Math.Ceiling((double)ButtonCount / MaxButtonsPerRow));
            Size = new Vector(w, h) * ButtonSize;
        }


        public SpellBar()
        {
            BackColor = new Color(50, 50, 50, 200);

            ButtonCount = 16;
            MaxButtonsPerRow = 8;

            Locked = false;
        }
    }
}
