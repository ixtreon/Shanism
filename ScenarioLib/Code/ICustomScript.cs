using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Objects;
using IO.Objects;

namespace ScenarioLib.Code
{
    /// <summary>
    /// An abstract base for the creation of custom game scripts. 
    /// </summary>
    public interface ICustomScript
    {
        void OnPlayerJoined(IPlayer pl);
    }
}
