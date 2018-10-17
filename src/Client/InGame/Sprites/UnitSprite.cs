using Shanism.Client.Assets;
using Shanism.Common.Entities;

namespace Shanism.Client.Sprites
{
    public class UnitSprite : EntitySprite
    {
        public IUnit Unit { get; }

        public UnitSprite(ContentList content, IUnit unit) 
            : base(content, unit)
        {
            Unit = unit;
        }
    }
}
