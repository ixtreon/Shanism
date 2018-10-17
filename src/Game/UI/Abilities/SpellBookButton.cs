using Shanism.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Client.UI.Abilities
{
    class SpellBookButton : SpellButton
    {

        public SpellBookButton(IAbility ability, Vector2 sz)
        {
            Ability = ability;
            CanToggle = false;
            Size = sz;
            Padding = sz.X / 16;
        }
    }
}
