using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Shanism.Common.StubObjects;

namespace Shanism.Common.Serialization
{
    class UnitSerializer : EntitySerializer
    {
        readonly AbilitySerializer abilitySerializer = new AbilitySerializer();

        readonly BuffInstanceSerializer buffSerializer = new BuffInstanceSerializer();

        public override void Write(BinaryWriter w, IGameObject obj)
        {
            base.Write(w, obj);
            var u = (IUnit)obj;

            //general
            w.Write(u.OwnerId);
            w.Write(u.Level);
            w.Write(u.IsDead);
            w.Write((short)u.StateFlags);
            w.Write(u.VisionRange);

            //stats
            w.Write(u.Life);
            w.Write(u.Mana);
            u.Stats.Write(w);

            //movement
            w.Write(u.MovementState.MoveDirection);
            w.Write(u.MovementState.MaxDistance);

            //casting
            w.Write(u.CastingAbilityId);
            w.Write(u.CastingProgress);
            w.Write(u.TotalCastingTime);


            //buffs
            var nBuffs = (byte)u.Buffs.Count;
            w.Write(nBuffs);
            foreach (var b in u.Buffs.Take(nBuffs))
                buffSerializer.Write(w, b);

            //abilities
            var nAbilities = (byte)u.Abilities.Count;
            w.Write(nAbilities);
            foreach (var a in u.Abilities.Take(nAbilities))
                abilitySerializer.Write(w, a);
        }

        public override ObjectStub Create(uint id) => new UnitStub(id);

        public override void Read(BinaryReader r, IGameObject obj)
        {
            base.Read(r, obj);
            var u = (UnitStub)obj;

            //general
            u.OwnerId = r.ReadUInt32();
            u.Level = r.ReadInt32();
            u.IsDead = r.ReadBoolean();
            u.StateFlags = (StateFlags)r.ReadInt16();
            u.VisionRange = r.ReadSingle();

            //life & mana
            u.Life = r.ReadSingle();
            u.Mana = r.ReadSingle();
            u.Stats.Read(r);

            //movement
            var dir = r.ReadSingle();
            var maxd = r.ReadSingle();
            u.MovementState = new MovementState(dir, maxd);

            //casting
            u.CastingAbilityId = r.ReadUInt32();
            u.CastingProgress = r.ReadInt32();
            u.TotalCastingTime = r.ReadInt32();

            //buffs
            var nBuffs = r.ReadByte();
            while (u.Buffs.Count < nBuffs)
                u.Buffs.Add(new BuffInstanceStub());
            for (int i = 0; i < nBuffs; i++)
                buffSerializer.Read(r, u.Buffs[i]);

            //abilities
            var nAbilities = r.ReadByte();
            while (u.Abilities.Count < nAbilities)
                u.Abilities.Add(new AbilityStub());
            for (int i = 0; i < nAbilities; i++)
                abilitySerializer.Read(r, u.Abilities[i]);
        }
    }
}
