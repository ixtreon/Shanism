using Shanism.ScenarioLib;
using System;
using System.Linq.Expressions;

namespace Shanism.Editor.Actions
{
    sealed class ScenarioChangeAction<TProp> : PropertyChangeAction<ScenarioConfig, TProp>
    {
        public ScenarioChangeAction(ScenarioConfig sc, Expression<Func<ScenarioConfig, TProp>> exp, TProp newValue) 
            : base(sc, exp, newValue)
        { }
    }
}
