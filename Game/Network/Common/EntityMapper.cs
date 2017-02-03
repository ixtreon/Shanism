using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shanism.Common;
using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Interfaces.Objects;
using Shanism.Common.StubObjects;
using Shanism.Network.Serialization;

namespace Shanism.Network.Common
{
    public class EntityMapper
    {
        const int MaxTypeId = 10;

        Action<ObjectStub, FieldReader>[] readers
            = new Action<ObjectStub, FieldReader>[MaxTypeId];
        Action<ObjectStub, IGameObject, FieldWriter>[] writers
            = new Action<ObjectStub, IGameObject, FieldWriter>[MaxTypeId];
        ObjectStub[] defaultObjects
            = new ObjectStub[MaxTypeId];

        public EntityMapper()
        {
            addType<HeroStub, IHero>(ObjectType.Hero);
            addType<UnitStub, IUnit>(ObjectType.Unit);
            addType<DoodadStub, IDoodad>(ObjectType.Doodad);
            addType<EffectStub, IEffect>(ObjectType.Effect);
        }

        void addType<TStub, TInt>(ObjectType ty)
            where TStub : ObjectStub, new()
            where TInt : IGameObject
        {
            readers[(int)ty] = PropertyMapper<TStub, TInt>.Reader;
            writers[(int)ty] = PropertyMapper<TStub, TInt>.Writer;
            defaultObjects[(int)ty] = new TStub();
        }

        public ObjectStub GetDefault(ObjectType ty)
            => defaultObjects[(byte)ty];

        public void Write(ObjectStub oldObj, IGameObject obj, FieldWriter w)
        {
            writers[(int)obj.ObjectType](oldObj, obj, w);
        }

        public ObjectStub Read(ObjectStub obj, FieldReader r)
        {
            readers[(int)obj.ObjectType](obj, r);
            return obj;
        }

        public ObjectStub Create(ObjectType type, uint id)
        {
            switch (type)
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
