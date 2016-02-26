using IO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using IO.Interfaces.Engine;

namespace Network.Client
{
    /// <summary>
    /// Holds all GameObjects sent by the server and performs RangeQueries for the client. 
    /// </summary>
    static class StubFactory
    {
        static readonly Dictionary<ObjectType, Func<uint, IGameObject>> _objFactory = new Dictionary<ObjectType, Func<uint, IGameObject>>
        {
            { ObjectType.Effect, (id) => new EffectStub(id) },
            { ObjectType.Doodad, (id) => new DoodadStub(id) },
            { ObjectType.Unit, (id) => new UnitStub(id) },
            { ObjectType.Hero, (id) => new HeroStub(id) },

            { ObjectType.Ability, (id) => new AbilityStub(id) },
            { ObjectType.BuffInstance, (id) => new BuffInstanceStub(id) },
        };

        /// <summary>
        /// Creates a stub game object with the specified id. 
        /// </summary>
        /// <param name="ty"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IGameObject Create(ObjectType ty, uint id)
        {
            return _objFactory[ty](id);
        }
    }
}
