using IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShanoRpgWinGl.UI
{
    class SpellBook : Window
    {
        private readonly IHero Hero;

        public SpellBook(IHero h)
        {
            this.Hero = h;
        }
    }
}
