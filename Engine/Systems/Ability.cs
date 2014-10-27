using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Maps;
using Engine.Objects;
using IO;
using IO.Common;

namespace Engine.Systems
{
    public abstract class Ability : ScenarioObject, IAbility
    {
        public class CastEventArgs
        {
            /// <summary>
            /// Gets or sets whether the spell cast was successful. True by default. 
            /// </summary>
            public bool Success = true;

        }


        public Hero Hero { get; internal set; }


        public string Name { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }
        public int CurrentCooldown { get; set; }


        public readonly AbilityType AbilityType;

        public int Cooldown { get; set; }
        public int ManaCost { get; set; }


        public Ability(AbilityType abilityType)
        {
            this.AbilityType = abilityType;
            this.Icon = "default";
            this.ManaCost = 1;
        }


        internal CastEventArgs Cast(object target) //wtf
        {
            var e = new CastEventArgs();
            switch (AbilityType)
            {
                case AbilityType.NoTarget:
                    OnCast(e);
                    break;
                case AbilityType.PointTarget:
                    OnCast(e, (Vector)target);
                    break;
                case AbilityType.UnitTarget:
                    OnCast(e, (Unit)target);
                    break;
                default:
                    throw new Exception("Unrecognized ability type!");
            }
            return e;
        }

        //only one of the following three will get called by the engine. 
        //make sure to override the correct one, 
        //depending on your choice for an abilityType

        /// <summary>
        /// The function which gets called when a no-target ability is cast. 
        /// </summary>
        public virtual void OnCast(CastEventArgs e)
        {

        }

        public virtual void OnCast(CastEventArgs e, Vector target)
        {

        }

        public virtual void OnCast(CastEventArgs e, Unit target)
        {

        }

        internal void Update(int msElapsed)
        {
            CurrentCooldown = Math.Max(0, CurrentCooldown - msElapsed);

            OnUpdate(msElapsed);
        }

        public virtual void OnUpdate(int msElapsed)
        {

        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
