using Shanism.Common;
using System;

namespace Shanism.Client.UI
{
    /// <summary>
    /// A type of <see cref="Button"/> that can be toggled.
    /// </summary>
    public class ToggleButton : Button
    {
        bool _isSelected;

        /// <summary>
        /// Gets or sets if clicking the button
        /// while it is already selected
        /// keeps it selected still.
        /// </summary>
        public bool StickyToggle { get; set; } = false;

        /// <summary>
        /// Gets or sets whether this toggle button
        /// responds to user input.
        /// </summary>
        public bool CanToggle { get; set; } = true;

        public event UiEventHandler<EventArgs> Selected;
        public event UiEventHandler<EventArgs> Deselected;

        /// <summary>
        /// Gets or sets whether this button is currently selected (toggled). 
        /// </summary>
        public virtual bool IsSelected
        {
            get => _isSelected;
            set => setSelected(value);
        }

        /// <summary>
        /// Gets the group of controls this toggle button belongs to.
        /// If this value is null, then <see cref="Control.Parent"/> is used.
        /// <para/>
        /// Only one <see cref="ToggleButton"/> can have <see cref="IsSelected"/>
        /// set to <c>true</c> within a given control group.
        /// </summary>
        public Control ControlGroup { get; set; }

        public Color SelectedColor { get; set; }

        public override Color BackColor
        {
            get => IsSelected ? SelectedColor : base.BackColor;
            set => base.BackColor = value;
        }

        public ToggleButton()
        {
            base.BackColor = UiColors.Button;
            SelectedColor = UiColors.Button.MixWith(Color.Black, 0.1f);
        }

        protected override void OnMouseDown(MouseButtonArgs e)
        {
            if(!CanToggle)
                return;

            if(IsSelected && StickyToggle)
                return;

            IsSelected ^= true;

            base.OnMouseDown(e);
        }

        void setSelected(bool val)
        {
            if(_isSelected != val)
            {
                _isSelected = val;

                OnToggleChanged(EventArgs.Empty);
            }

            // deselect all toggleButtons in the parent
            if(_isSelected && (ControlGroup != null || Parent != null))
                foreach(var c in (ControlGroup ?? Parent).Controls)
                    if(c is ToggleButton btn && btn != this)
                        btn.IsSelected = false;
        }

        protected virtual void OnToggleChanged(EventArgs e)
        {
            if(_isSelected)
                Selected?.Invoke(this, e);
            else
                Deselected?.Invoke(this, e);
        }

        public override void Draw(Canvas c)
        {
            base.Draw(c);

        }
    }
}
