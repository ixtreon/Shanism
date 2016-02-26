using IO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO.Objects
{
    public interface IBuffInstance : IBuff
    {
        int DurationLeft { get; }
    }
}
