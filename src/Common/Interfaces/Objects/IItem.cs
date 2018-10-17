using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Objects
{
    public interface IItem
    {
        IAbility Ability { get; }
        IBuff CarryBuff { get; }
        string Description { get; }
        IBuff EquipBuff { get; }
        string Icon { get; }
        string Name { get; }
        EquipSlot Slot { get; }
    }
}
