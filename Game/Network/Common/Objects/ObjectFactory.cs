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
        private static Dictionary<int, IGameObject> objects = new Dictionary<int, IGameObject>();
        private static Dictionary<int, ObjectType> objectTypes = new Dictionary<int, ObjectType>();

        public static IEnumerable<IGameObject> AllObjects
        {
            get { return objects.Values; } 
        }

        public static IGameObject GetOrCreate(ObjectType ty, int guid)
        {
            IGameObject obj;

            //check if game object is new. 
            if (!objects.TryGetValue(guid, out obj))
            {
                obj = createNew(ty, guid);
                objects[guid] = obj;
            }

            if (!ty.UnderlyingInterface.IsAssignableFrom(obj.GetType()))    // make sure types match
                throw new Exception("Object {0} was expected to be of type {1} but was {2} instead. ".Format(guid, ty.UnderlyingInterface.Name, obj.GetType().Name));

            return obj;
        }

        public static void AddOrUpdate(ObjectType objType, int guid, IGameObject obj)
        {
            if (objectTypes.ContainsKey(guid) && objectTypes[guid] != objType)
                throw new Exception("Inconsistent ObjectTypes: object #{0} was {1} but is now {2}!".Format(guid, objectTypes[guid], objType));

            objects[guid] = obj;
            objectTypes[guid] = objType;
        }

        public static IEnumerable<IGameObject> RangeQuery(Vector pos, Vector size)
        {
            return objects.Values
                .Where(o => o.Position.Inside(pos, size));
        }

        private static IGameObject createNew(ObjectType ty, int guid)
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
