using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Interfaces.Objects
{
    public interface IBuffInstance : IGameObject
    {
        /// <summary>
        /// Gets the time remaining before this buff expires. 
        /// </summary>
        int DurationLeft { get; }

        IBuff Prototype { get; }
    }
}
