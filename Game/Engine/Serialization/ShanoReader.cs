using Engine.Objects;
using Engine.Objects.Buffs;
using Engine.Objects.Entities;
using Engine.Systems.Abilities;
using IO;
using IO.Common;
using IO.Message.Network;
using IO.Objects;
using IO.Serialization;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Serialization
{
    public static class ShanoReader
    {
        #region Init
        static readonly Dictionary<ObjectType, List<IPropertyCallAdapter>> _propReadDict = new Dictionary<ObjectType, List<IPropertyCallAdapter>>();

        static ShanoReader()
        {
            AddMapping<IEffect, Effect>(ObjectType.Effect);
            AddMapping<IUnit, Unit>(ObjectType.Unit);
            AddMapping<IHero, Hero>(ObjectType.Hero);
            AddMapping<IDoodad, Doodad>(ObjectType.Doodad);

            AddMapping<IAbility, Ability>(ObjectType.Ability);
            AddMapping<IBuffInstance, BuffInstance>(ObjectType.BuffInstance);
        }

        static void AddMapping<TCommon, TRead>(ObjectType ty)
        {
            var props = typeof(TCommon).GetAllProperties()
                .Where(p => p.GetCustomAttribute(typeof(ProtoIgnoreAttribute)) == null)
                .OrderBy(p => p.Name)
                .ToList();

            _propReadDict[ty] = props
                .Select(p => PropertyCaller.GetInstance(typeof(TRead), p.Name))
                .ToList();
        }
        #endregion


        public static GameFrameMessage PrepareGameFrame(IEnumerable<Entity> visibleObjects)
        {
            var relatedObjectIds = visibleObjects
                .SelectMany(FetchObjectIds)
                .Distinct()
                .ToList();

            byte[] datas;
            using (var ms = new MemoryStream())
            {
                using (var wr = new BinaryWriter(ms))
                {
                    wr.Write(relatedObjectIds.Count);

                    foreach (var objId in relatedObjectIds)
                    {
                        var obj = GameObject.GetById(objId);
                        SerializeObject(wr, obj);
                    }
                }
                datas = ms.ToArray();
            }

            return new GameFrameMessage(datas);
        }

        public static void SerializeObject(BinaryWriter wr, GameObject obj)
        {
            wr.Write((byte)obj.ObjectType);
            wr.Write((uint)obj.Id);

            var props = _propReadDict[obj.ObjectType];

            foreach (var pr in props)
            {
                IxSerializer.Serializer.Write(wr, pr.PropertyType, pr.InvokeGet(obj));
            }
        }
        
        public static byte[] QuickSerialize(GameObject obj)
        {
            using (var ms = new MemoryStream())
            {
                using (var w = new BinaryWriter(ms))
                    SerializeObject(w, obj);
                return ms.ToArray();
            }
        }

        static IEnumerable<uint> FetchObjectIds(Entity obj)
        {
            switch (obj.ObjectType)
            {
                case ObjectType.Hero:
                    foreach (var ability in ((Hero)obj).Abilities)
                        yield return ability.Id;

                    goto case ObjectType.Unit;

                case ObjectType.Unit:
                    foreach (var buff in ((Unit)obj).Buffs)
                        yield return buff.Id;

                    //yield return ((Unit)obj).Owner.Id; -- not a gameobject!!

                    goto case ObjectType.Effect;

                case ObjectType.Doodad:
                case ObjectType.Effect:
                    yield return obj.Id;
                    break;
            }
        }
    }
}
