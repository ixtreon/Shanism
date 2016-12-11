using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common.Game;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.Serialization;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Shanism.Common.StubObjects
{
    public class ObjectStub : IGameObject, ISerializableObject
    {
        public static readonly ObjectStub Default = new ObjectStub();

        /// <summary>
        /// To be used for flagging objects that disappeared!
        /// True if object existed before. False otherwise. 
        /// </summary>
        public bool _ReaderFlag { get; set; }

        public uint Id { get; set; }

        public ObjectType ObjectType { get; set; }

        public ObjectStub() { }

        public ObjectStub(uint id)
        {
            this.Id = id;
        }
        public virtual void ReadDiff(FieldReader r)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteDiff(FieldWriter w, IGameObject newObject)
        {
            throw new NotImplementedException();
        }

        public static ObjectStub GetDefaultObject(ObjectType type)
        {
            switch(type)
            {
                case ObjectType.Unit:
                    break;
                case ObjectType.Effect:
                    break;
                case ObjectType.Doodad:
                    break;
                case ObjectType.Hero:
                    break;

                case ObjectType.Buff:
                    return BuffStub.Default;

                case ObjectType.BuffInstance:
                    break;
                case ObjectType.Ability:
                    break;
                case ObjectType.Item:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new NotImplementedException();
        }

        public static ObjectStub CreateObject(ObjectType type, uint id)
        {
            switch(type)
            {
                case ObjectType.Unit:
                    return new UnitStub(id);

                case ObjectType.Effect:
                    return new EffectStub(id);

                case ObjectType.Doodad:
                    return new DoodadStub(id);

                case ObjectType.Hero:
                    return new HeroStub(id);

                case ObjectType.Buff:
                    return new BuffStub();

                case ObjectType.BuffInstance:
                    return new BuffInstanceStub(id);

                case ObjectType.Ability:
                    return new AbilityStub(id);

                case ObjectType.Item:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
