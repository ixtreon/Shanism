using Shanism.Engine.Objects;
using Shanism.Engine.Objects.Entities;
using Shanism.Common;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine.Common
{
    public class ObjectCreator
    {
        static readonly HashSet<Type> baseTypes = new HashSet<Type>
        {
            typeof(Doodad),
            typeof(Effect),
        };

        readonly Dictionary<string, Type> recognizedTypes;

        public IEnumerable<Type> CustomTypes => recognizedTypes.Values.Except(baseTypes);

        public IEnumerable<Type> BaseTypes => baseTypes;


        public ObjectCreator(CompiledScenario sc)
        {
            recognizedTypes = baseTypes
                .Concat(sc.DefinedEntityTypes)
                .ToDictionary(ty => ty.FullName, ty => ty);
        }


        public Entity CreateObject(ObjectConstructor oc)
        {
            var objTy = recognizedTypes.TryGet(oc.TypeName);
            if (objTy == null)
                return null;

            var e = (Entity)Activator.CreateInstance(objTy);
            e.Position = oc.Location;

            //TODO: set owner
            //if (!string.IsNullOrEmpty(oc.Owner) && e is Unit)
            //    ((Unit)e).Owner = Player.NeutralAggressive;

            //set animation
            if (!string.IsNullOrEmpty(oc.Animation))
                e.AnimationSuffix = oc.Animation;

            if (oc.Size > 0)
                e.Scale = oc.Size;

            if (oc.Tint.HasValue)
                e.CurrentTint = e.DefaultTint = oc.Tint.Value;

            return e;
        }
    }
}
