using IO;
using IO.Objects;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Common;
using IO.Message.Network;
using IO.Message;
using IO.Message.Server;
using IO.Serialization;

namespace Network.Objects
{
    /// <summary>
    /// Holds all GameObjects sent by the server and performs RangeQueries for the client. 
    /// </summary>
    class ObjectFactory
    {


        static readonly Dictionary<uint, IGameObject> objects = new Dictionary<uint, IGameObject>();
        static readonly HashSet<uint> objectsSeen = new HashSet<uint>();

        public static IEnumerable<IGameObject> AllObjects
        {
            get { return objects.Values; } 
        }

        public static IGameObject Get(uint guid)
        {
            IGameObject obj;
            if (!objects.TryGetValue(guid, out obj))
                return null;

            return obj;
        }

        public static void HandleObjectMessage(IOMessage msg)
        {
            switch(msg.Type)
            {
                case MessageType.ObjectSeen:
                    var mm = (ObjectSeenMessage)msg;
                    objectsSeen.Add(mm.Guid);
                    break;

                case MessageType.ObjectUnseen:
                    var mmm = (ObjectUnseenMessage)msg;
                    objectsSeen.Add(mmm.Guid);
                    break;

                case MessageType.ObjectData:
                    var m = (ObjectDataMessage)msg;
                    //TODO: decode the datas
                    break;

                default:
                    throw new Exception("ObjectFactory did not expect a {0} message");
            }
        }

        public static void UpdateDatas(ObjectDataMessage msg)
        {

        }

        //public static void AddOrUpdate(ObjectType objType, uint guid, IGameObject obj)
        //{
        //    if (objectTypes.ContainsKey(guid) && objectTypes[guid] != objType)
        //        throw new Exception("Inconsistent ObjectTypes: object #{0} was {1} but is now {2}!".F(guid, objectTypes[guid], objType));

        //    objects[guid] = obj;
        //    objectTypes[guid] = objType;
        //}

        public static IEnumerable<IGameObject> RangeQuery(Vector pos, Vector size)
        {
            return objects.Values
                .Where(o => o.Position.Inside(pos, size));
        }
    }
}
