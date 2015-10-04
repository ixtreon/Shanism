using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Objects
{
    public interface IPlayer
    {
        bool IsNeutralAggressive { get; }
        bool IsNeutralFriendly { get; }

        bool IsPlayer { get; }

        string Name { get; }
    }
}
