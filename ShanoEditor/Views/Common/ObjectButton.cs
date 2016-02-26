using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShanoEditor.ViewModels;

namespace ShanoEditor.Views
{
    class ObjectButton<T> : RadioButton, IScenarioControl
    {
        /// <summary>
        /// The event raised whenever the button is selected. 
        /// </summary>
        public event Action<T> ObjectSelected;

        /// <summary>
        /// Gets the object this button is made for. 
        /// </summary>
        public T Object { get; }

        public ScenarioViewModel Model { get; private set; }


        public ObjectButton(T value, int buttonSize = 32)
        {
            Size = new System.Drawing.Size(buttonSize, buttonSize);
            Appearance = Appearance.Button;
            Text = string.Empty;

            Object = value;
        }

        public void SetModel(ScenarioViewModel model)
        {
            this.Model = model;
            Invalidate();
        }

        #region Overrides and Event Handlers

        protected override void OnClick(EventArgs e)
        {
            ObjectSelected?.Invoke(Object);
            base.OnClick(e);
        }
        #endregion
    }
}
