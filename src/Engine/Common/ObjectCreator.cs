using Shanism.Engine.Objects;
using Shanism.Engine.Entities;
using Shanism.Common;
using Shanism.ScenarioLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shanism.Engine
{
    /// <summary>
    /// Creates game entities from <see cref="ObjectConstructor"/> objects in the context of a <see cref="Scenario"/>. 
    /// </summary>
    public class ObjectCreator
    {
        static readonly HashSet<Type> baseTypes = new HashSet<Type>
        {
            typeof(Doodad),
            typeof(Effect),
        };

        readonly Scenario scenario;
        readonly Dictionary<string, Type> recognizedTypes;


        public ObjectCreator(Scenario sc)
        {
            scenario = sc;
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

            Player owner;
            if (e is Unit && Player.TryParse(oc.Owner, out owner))
            {
                var life  = 42;
                ((Unit)e).Owner = owner;
            }

            //set animation
            if (!string.IsNullOrEmpty(oc.Model))
                e.Model = oc.Model;

            if (oc.Size > 0)
                e.Scale = oc.Size;

            if (oc.Tint.HasValue)
                e.CurrentTint = e.DefaultTint = oc.Tint.Value;

            return e;
        }


        public IEnumerable<Entity> CreateAllEntities()
        {
            return scenario.Config.Map.Objects
                .Select(CreateObject)
                .Where(o => o != null)
                .ToList();
        }
    }
}
