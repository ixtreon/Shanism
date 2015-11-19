using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI
{
    class Control2
    {
        //The absolute position. 
        // Updated when a control's _position changes, 
        // or its parent's absolute position changes. 
        Vector _absolutePosition;

        Vector _position;
        Vector _size;           //size or far_pos?!
        private Control2 _parent;

        public Control2 Parent
        {
            get { return _parent; }
            set
            {
                if(_parent != value)
                {
                    if(_parent != null)
                        _parent.AbsolutePositionChanged -= _parent_AbsolutePositionChanged;

                    _parent = value;
                    updateAbsPos();

                    if (_parent != null)
                        _parent.AbsolutePositionChanged += _parent_AbsolutePositionChanged;
                }
            }
        }

        private void _parent_AbsolutePositionChanged()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fired whenever this control's position is changed. 
        /// </summary>
        public event Action AbsolutePositionChanged;



        void updateAbsPos()
        {
            _absolutePosition = (Parent?._absolutePosition ?? Vector.Zero) + _position;
            AbsolutePositionChanged?.Invoke();
        }

        //public virtual void Draw(Graphics g)
        //{

        //}
    }
}
