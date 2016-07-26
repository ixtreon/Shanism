using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Common.Serialization
{
    /// <summary>
    /// Serializes the different kinds of <see cref="IGameObject"/>. 
    /// </summary>
    public class GameSerializer
    {
        readonly ObjectSerializer[] serializers = new ObjectSerializer[16];

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSerializer"/> class.
        /// </summary>
        public GameSerializer()
        {
            serializers[(int)ObjectType.Doodad] = new EntitySerializer();
            serializers[(int)ObjectType.Effect] = new EntitySerializer();
            serializers[(int)ObjectType.Unit] = new UnitSerializer();
            serializers[(int)ObjectType.Hero] = new HeroSerializer();

            //serializers[(int)ObjectType.BuffInstance] = new BuffInstanceSerializer();
            //serializers[(int)ObjectType.Ability] = new AbilitySerializer();
        }

        /// <summary>
        /// Writes the specified game object to the given writer. 
        /// </summary>
        /// <param name="w">The writer where the object is written.</param>
        /// <param name="obj">The object to write.</param>
        /// <param name="ty">The type as which the object is serialized.</param>
        public void Write(BinaryWriter w, IGameObject obj, ObjectType ty)
        {
            var s = serializers[(int)ty];
            s.Write(w, obj);
        }

        /// <summary>
        /// Reads a game object header from the stream.
        /// </summary>
        /// <param name="r">The reader of some stream.</param>
        /// <returns>The next object's header.</returns>
        public ObjectHeader ReadHeader(BinaryReader r)
        {
            return ObjectSerializer.ReadHeader(r);
        }

        /// <summary>
        /// Creates the game object from the given header.
        /// </summary>
        /// <param name="h">The header which specifies the type of game object.</param>
        /// <returns>A fresh game object compatible with the header.</returns>
        public ObjectStub Create(ObjectHeader h)
        {
            var s = serializers[(int)h.Type];
            var o = s.Create(h.Id);
            o.ObjectType = h.Type;
            return o;
        }

        //once after a header is obtained        
        /// <summary>
        /// Updates the given game object. 
        /// </summary>
        /// <param name="r">The reader of some stream.</param>
        /// <param name="h">The header which specifies the type of game object.</param>
        /// <param name="obj">The existing game object.</param>
        public void Update(BinaryReader r, ObjectHeader h, IGameObject obj)
        {
            var s = serializers[(int)h.Type];
            s.Read(r, obj);
        }
    }
}
