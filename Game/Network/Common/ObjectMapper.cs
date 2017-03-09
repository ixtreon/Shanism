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
    public class ObjectMapper
    {
        const int MaxTypeId = 10;

        Action<ObjectStub, FieldReader>[] readers
            = new Action<ObjectStub, FieldReader>[MaxTypeId];
        Action<ObjectStub, IGameObject, FieldWriter>[] writers
            = new Action<ObjectStub, IGameObject, FieldWriter>[MaxTypeId];
        ObjectStub[] defaultObjects
            = new ObjectStub[MaxTypeId];

        public ObjectMapper()
        {
            addType<HeroStub, IHero>(ObjectType.Hero);
            addType<UnitStub, IUnit>(ObjectType.Unit);
            addType<DoodadStub, IDoodad>(ObjectType.Doodad);
            addType<EffectStub, IEffect>(ObjectType.Effect);

            addType<BuffStub, IBuff>(ObjectType.Buff);
            addType<BuffInstanceStub, IEffect>(ObjectType.BuffInstance);
            addType<AbilityStub, IAbility>(ObjectType.Effect);
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

        public void Write(ObjectType objType, ObjectStub oldObj, IGameObject obj, FieldWriter w)
        {
            writers[(int)objType](oldObj, obj, w);
        }

        public ObjectStub Read(ObjectType objType, ObjectStub obj, FieldReader r)
        {
            readers[(int)objType](obj, r);
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
