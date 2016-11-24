using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI
{
    class ScrollBar : Control
    {
        public static readonly double DefaultWidth = 0.06;

        double _span = 0.2, _total = 1, _start = 0;

        public double Total
        {
            get { return _total; }
            set { _total = value; }
        }

        public double Span
        {
            get { return _span; }
            set { _span = value; }
        }

        public double Start
        {
            get { return _start; }
            set { _start = value; }
        }

        readonly Control slider;

        Vector? sliderDragPt;

        public ScrollBar()
        {
            Size = new Vector(DefaultWidth, 0.6);
            BackColor = Color.Black.SetAlpha(100);

            SizeChanged += (c) => updateSlider();

            Add(slider = new Button
            {
                BackColor = Color.Black.SetAlpha(100),
            });
            updateSlider();

            slider.MouseDown += Slider_MouseDown;
            slider.MouseUp += Slider_MouseUp;
            slider.MouseMove += Slider_MouseMove;
        }

        void updateSlider()
        {
            slider.Size = new Vector(Size.X, _span / _total * Size.Y);
            slider.Location = new Vector(0, _start / _total * Size.Y);
        }

        private void Slider_MouseMove(Input.MouseArgs e)
        {
            if (sliderDragPt == null)
                return;

            var d = e.Position - sliderDragPt.Value;
            var newPos = (slider.Location.Y + d.Y) / Size.Y * _total;

            Start = newPos.Clamp(0, _total - _span);
            updateSlider();
        }

        void Slider_MouseUp(Input.MouseButtonArgs e)
        {
            sliderDragPt = null;
        }

        void Slider_MouseDown(Input.MouseButtonArgs e)
        {
            sliderDragPt = e.Position;
        }
    }
}
