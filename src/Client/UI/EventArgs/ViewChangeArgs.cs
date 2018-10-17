using Shanism.Client.Views;
using System;

namespace Shanism.Client.UI
{
    /// <summary>
    /// The data about a control changing their parent view.
    /// </summary>
    public class ViewChangeArgs : EventArgs
    {
        
        public View PreviousView { get; }

        /// <summary>
        /// Gets the new parent view of this control.
        /// </summary>
        public View NewView { get; }


        public ViewChangeArgs(View previousView, View newView)
        {
            PreviousView = previousView;
            NewView = newView;
        }
    }
}
