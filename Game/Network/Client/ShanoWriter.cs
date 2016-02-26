using IO;
using IO.Common;
using IO.Interfaces.Engine;
using IO.Message.Network;
using IO.Objects;
using IO.Serialization;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Network.Client
{
    public static class ShanoWriter
    {
        static readonly Dictionary<ObjectType, List<IPropertyCallAdapter>> _propWriteDict
            = new Dictionary<ObjectType, List<IPropertyCallAdapter>>();

        static ShanoWriter()
        {
            AddMapping<IEffect, EffectStub>(ObjectType.Effect);
            AddMapping<IUnit, UnitStub>(ObjectType.Unit);
            AddMapping<IHero, HeroStub>(ObjectType.Hero);
            AddMapping<IDoodad, DoodadStub>(ObjectType.Doodad);

            AddMapping<IAbility, AbilityStub>(ObjectType.Ability);
            AddMapping<IBuffInstance, BuffInstanceStub>(ObjectType.BuffInstance);
        }


        static void AddMapping<TCommon, TWrite>(ObjectType ty)
        {
            var props = typeof(TCommon).GetAllProperties()
                .Where(p => p.GetCustomAttribute(typeof(ProtoIgnoreAttribute)) == null)
                .OrderBy(p => p.Name)
                .ToList();

            _propWriteDict[ty] = props
                .Select(p => PropertyCaller.GetInstance(typeof(TWrite), p.Name))
                .ToList();
        }


        public static IEnumerable<IGameObject> ReadObjectStream(IObjectCache objCache, GameFrameMessage msg)
        {
            using (var ms = new MemoryStream(msg.Data))
            using (var r = new BinaryReader(ms))
            {
                var nItems = r.ReadInt32();

                for(var i = 0; i < nItems; i++)
                {
                    //read type, id
                    var objType = (ObjectType)r.ReadByte();
                    var objId = r.ReadUInt32();

                    //ask the cache about the stub object
                    var obj = objCache.GetOrAdd(objType, objId);

                    //update all properties' values
                    var props = _propWriteDict[objType];
                    foreach (var pr in props)
                    {
                        var newVal = IxSerializer.Serializer.Read(r, pr.PropertyType);
                        pr.InvokeSet(obj, newVal);
                    }

                    yield return obj;
                }
            }
        }


        public static void DeserializeObject(BinaryReader r, ObjectStub obj)
        {
            var objType = (ObjectType)r.ReadByte();
            var props = _propWriteDict[objType];

            foreach(var pr in props)
            {
                var newVal = IxSerializer.Serializer.Read(r, pr.PropertyType);
                pr.InvokeSet(obj, newVal);
            }
        }
    }
}
