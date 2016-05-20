using Shanism.Common.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Objects
{
    public interface IBuffInstance : IBuff
    {
        int DurationLeft { get; }
    }
}
