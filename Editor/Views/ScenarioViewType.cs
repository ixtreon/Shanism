using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Editor.Views
{
    /// <summary>
    /// Used to mark scenario views and automagically create the tree. 
    /// Up to 10 one-level-deep branches for each root node. 
    /// </summary>
    public enum ScenarioViewType
    {
        Details = 0,
        Map = 10,
        Content = 20,
            Animations = 21,
            Models = 22,
            Textures = 23,
    }
}
