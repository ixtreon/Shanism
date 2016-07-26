using Shanism.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;

namespace Shanism.Editor.Util
{
    class ShanoGrid : PropertyGrid
    {
        public ShanoGrid()
        {
            MouseWheel += ShanoGrid_MouseWheel;
            this.SetAutoSizeMode(AutoSizeMode.GrowAndShrink);
        }

        void ShanoGrid_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!Bounds.Contains(e.Location) || SelectedGridItem == null)
                return;

            var oldVal = SelectedGridItem.Value;

            if (oldVal is int)
            {
                var range = GetRange(SelectedGridItem.PropertyDescriptor);
                var delta = modWheelDelta(range.Step * Math.Sign(e.Delta));
                var newVal = ((int)oldVal + delta).Clamp(range.Minimum, range.Maximum);

                if ((int)oldVal != newVal)
                    setCurrentItemValue(oldVal, newVal);
            }
            else if (oldVal is bool)
            {
                var newVal = e.Delta > 0;

                if ((bool)oldVal != newVal)
                    setCurrentItemValue(oldVal, newVal);
            }
        }

        void setCurrentItemValue(object oldVal, object newVal)
        {
            var pd = SelectedGridItem.PropertyDescriptor;

            if (SelectedObjects.Length == 1)
                pd.SetValue(SelectedObject, newVal);
            else
                pd.SetValue(SelectedObjects, newVal);

            //SelectedGridItem.PropertyDescriptor.SetValue(SelectedObject, newVal);
            Refresh();

            OnPropertyValueChanged(new PropertyValueChangedEventArgs(SelectedGridItem, oldVal));
        }

        static RangeAttribute GetRange(PropertyDescriptor prop)
        {
            return prop.Attributes
                .OfType<RangeAttribute>()
                .FirstOrDefault() ?? RangeAttribute.Default;
        }

        static int modWheelDelta(int delta)
        {
            if (ModifierKeys.HasFlag(Keys.Shift))
                delta *= 50;
            else if (ModifierKeys.HasFlag(Keys.Alt))
                delta *= 10;
            else if (ModifierKeys.HasFlag(Keys.Control))
                delta *= 5;
            return delta;
        }
    }
}
