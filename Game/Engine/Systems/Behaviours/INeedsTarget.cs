using Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Systems.Behaviours
{
    interface INeedsTarget
    {
        Unit Target { get; set; }
    }
}
