using Shanism.Common;
using Shanism.ScenarioLib;
using System;

namespace Shanism.Editor.Actions.Objects
{
    class ObjectBrushAction : ActionBase
    {
        readonly MapConfig map;
        readonly ObjectConstructor ctor;

        public ObjectBrushAction(MapConfig map, ObjectConstructor ctor)
        {
            this.map = map;
            this.ctor = ctor;
            Description = $"Added a `{shortName(ctor.TypeName)}` at {ctor.Location}.";
        }

        public override void Apply()
        {
            map.Objects.Add(ctor);
        }

        public override void Revert()
        {
            if (!map.Objects.Contains(ctor))
                throw new InvalidOperationException();

            map.Objects.RemoveLast();
        }

        static string shortName(string fullTypeName)
        {
            if (fullTypeName == null)
                return null;

            var dotId = fullTypeName.LastIndexOf('.');
            if (dotId < 0 || dotId == fullTypeName.Length - 1)
                return fullTypeName.Substring(dotId + 1);

            return fullTypeName;
        }
    }
}
