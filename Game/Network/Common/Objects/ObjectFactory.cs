using IO;
using IO.Objects;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network.Objects.Serializers;
using IO.Common;

namespace Network.Objects
{
    /// <summary>
    /// Holds all GameObjects sent by the server and performs RangeQueries for the client. 
    /// </summary>
    class ObjectFactory
    {
        static readonly Dictionary<uint, IGameObject> objects = new Dictionary<uint, IGameObject>();
        static readonly Dictionary<uint, ObjectType> objectTypes = new Dictionary<uint, ObjectType>();

        public static IEnumerable<IGameObject> AllObjects
        {
            get { return objects.Values; } 
        }

        public static IGameObject GetOrCreate(ObjectType ty, uint guid)
        {
            IGameObject obj;

            //check if game object is new. 
            if (!objects.TryGetValue(guid, out obj))
            {
                obj = createNew(ty, guid);
                objects[guid] = obj;
            }

            if (!ty.UnderlyingInterface.IsAssignableFrom(obj.GetType()))    // make sure types match
                throw new Exception("Object {0} was expected to be of type {1} but was {2} instead. ".F(guid, ty.UnderlyingInterface.Name, obj.GetType().Name));

            return obj;
        }

        public static void AddOrUpdate(ObjectType objType, uint guid, IGameObject obj)
        {
            if (objectTypes.ContainsKey(guid) && objectTypes[guid] != objType)
                throw new Exception("Inconsistent ObjectTypes: object #{0} was {1} but is now {2}!".F(guid, objectTypes[guid], objType));

            objects[guid] = obj;
            objectTypes[guid] = objType;
        }

        public static IEnumerable<IGameObject> RangeQuery(Vector pos, Vector size)
        {
            return objects.Values
                .Where(o => o.Position.Inside(pos, size));
        }

        static IGameObject createNew(ObjectType ty, uint guid)
        {
            if (ty == ObjectType.Doodad)
                return new DoodadStub(guid);
            if (ty == ObjectType.Hero)
                return new HeroStub(guid);
            if (ty == ObjectType.Unit)
                return new UnitStub(guid);

            throw new Exception("Unable to create an object of type " + ty.Id);
        }
    }
}
