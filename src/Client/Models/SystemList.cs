using System;
using System.Collections;
using System.Collections.Generic;

namespace Shanism.Client.Components
{
    public class SystemList : IEnumerable<ISystem>
    {

        protected Dictionary<Type, ISystem> Components { get; } = new Dictionary<Type, ISystem>();


        public void Add(ISystem c) => Add(c.GetType(), c);

        public void Add<T>(T c) where T : ISystem
            => Add(typeof(T), c);

        public void Add(Type ty, ISystem c)
        {
            if (Components.ContainsKey(ty))
                throw new InvalidOperationException($"Duplicate registration for the `{ty.Name}` type.");

            Components.Add(ty, c);
        }
        
        public void AddRange(IEnumerable<ISystem> cs)
        {
            foreach (var c in cs)
                Add(c);
        }

        /// <summary>
        /// Updates all systems in the order in which they were added.
        /// </summary>
        public void Update(int msElapsed)
        {
            foreach (var kvp in Components)
                kvp.Value.Update(msElapsed);
        }

        public IEnumerator<ISystem> GetEnumerator() => Components.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Components.Values.GetEnumerator();
    }
}
