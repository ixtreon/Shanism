using Shanism.Common.Interfaces.Entities;
using Shanism.Common.Interfaces.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Shanism.Common.StubObjects;
using Shanism.Common.Game;

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
            w.Write((byte)u.OrderType);
            w.Write((short)u.States);
            w.Write((float)u.VisionRange);

            //life & mana
            w.Write((float)u.Life);
            w.Write((float)u.MaxLife);
            w.Write((float)u.LifeRegen);
            w.Write((float)u.Mana);
            w.Write((float)u.MaxMana);
            w.Write((float)u.ManaRegen);

            //movement
            w.Write(u.IsMoving);
            w.Write((float)u.MoveDirection);
            w.Write((float)u.MoveSpeed);

            //combat
            w.Write(u.AttackCooldown);
            w.Write((float)u.Defense);
            w.Write((float)u.MinDamage);
            w.Write((float)u.MaxDamage);
            w.Write((float)u.MagicDamage);

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
            u.OrderType = (OrderType)r.ReadByte();
            u.States = (StateFlags)r.ReadInt16();
            u.VisionRange = r.ReadSingle();

            //life & mana
            u.Life = r.ReadSingle();
            u.MaxLife = r.ReadSingle();
            u.LifeRegen = r.ReadSingle();
            u.Mana = r.ReadSingle();
            u.MaxMana = r.ReadSingle();
            u.ManaRegen = r.ReadSingle();

            //movement
            u.IsMoving = r.ReadBoolean();
            u.MoveDirection = r.ReadSingle();
            u.MoveSpeed = r.ReadSingle();

            //combat
            u.AttackCooldown = r.ReadInt32();
            u.Defense = r.ReadSingle();
            u.MinDamage = r.ReadSingle();
            u.MaxDamage = r.ReadSingle();
            u.MagicDamage = r.ReadSingle();

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
