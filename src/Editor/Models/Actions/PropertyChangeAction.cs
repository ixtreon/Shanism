using Shanism.Common;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Shanism.Editor.Actions
{
    abstract class PropertyChangeAction<TObj> : ActionBase
    {
        public TObj Target { get; }

        public PropertyChangeAction(TObj target)
        {
            Target = target;
        }
    }

    class PropertyChangeAction<TObj, TProp> : PropertyChangeAction<TObj>
    {
        readonly Func<TObj, TProp> getter;
        readonly Action<TObj, TProp> setter;

        public TProp NewValue { get; }
        public TProp OldValue { get; private set; }


        public PropertyChangeAction(TObj target, Expression<Func<TObj, TProp>> exp, TProp newValue)
            : base(target)
        {
            var e = (exp.Body as MemberExpression);
            if (e == null)
                throw new Exception("Expected a member access in the body...");

            var pi = (e.Member as PropertyInfo) ?? throw new Exception("Expected a property access in the body...");
            getter = pi.CreateGetter<TObj, TProp>();
            setter = pi.CreateSetter<TObj, TProp>();

            NewValue = newValue;
            Description = $"Changed {typeof(TObj).Name}.{exp.Name} to `{newValue}`.";
        }

        public override void Apply()
        {
            if (Target == null)
                throw new Exception("Target is not set...");

            OldValue = getter(Target);
            setter(Target, NewValue);
        }

        public override void Revert()
        {
            if (!getter(Target).Equals(NewValue))
                throw new InvalidOperationException($"Revert failed: ...");

            setter(Target, OldValue);
        }
    }
}
