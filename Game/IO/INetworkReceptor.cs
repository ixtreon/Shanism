using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    /// <summary>
    /// A type of game receptor which provides extra methods for networking. 
    /// </summary>
    public interface INetworkReceptor : IGameReceptor
    {

        event Action<IHero> MainHeroChanged;

        event Action<IGameObject> ObjectLeavesVisionRange;
        event Action<IGameObject> ObjectInVisionRange;

        event Action<IGameObject> ObjectMoved;
        event Action<IUnit> UnitActionPerformed;
        event Action<IUnit> UnitMoveStateChanged;
    }
}
