using Shanism.Common;
using Shanism.Common.Objects;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace Shanism.Engine
{
    public interface IDestructible
    {
        bool IsDestroyed { get; }
    }

    /// <summary>
    /// Represents all things that belong to a game world. 
    /// This includes game objects, abilities, buffs, items, scripts (?)
    /// </summary>
    public abstract class GameObject : GameComponent, IGameObject, IDestructible, IEquatable<GameObject>
    {

        /// <summary>
        /// Gets the <see cref="Common.ObjectType"/> of this game object. 
        /// </summary>
        public abstract ObjectType ObjectType { get; }

        /// <summary>
        /// Gets the unique ID of the game object.
        /// </summary>
        public uint Id { get; }

        /// <summary>
        /// Gets whether this object should be removed from the game as soon as possible. 
        /// </summary>
        public bool IsDestroyed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class.
        /// </summary>
        protected GameObject()
        {
            Id = ObjectAllocator.New();
        }


        /// <summary>
        /// Can be overridden in derived classes to implement custom update handlers. 
        /// </summary>
        /// <param name="msElapsed"></param>
        internal virtual void Update(int msElapsed) { }

        /// <summary>
        /// Marks this GameObject for destruction, eventually removing it from the game. 
        /// </summary>
        public virtual void Destroy()
        {
            Debug.Assert(!IsDestroyed);
            IsDestroyed = true;
        }


        /// <summary>
        /// Determines whether the two objects are the same.
        /// </summary>
        public static bool operator ==(GameObject a, GameObject b) => a?.Id == b?.Id;

        /// <summary>
        /// Determines whether the two objects are different.
        /// </summary>
        public static bool operator !=(GameObject a, GameObject b) => a?.Id != b?.Id;


        public override bool Equals(object obj) => (obj is GameObject other) && Equals(other);

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => $"{ObjectType} #{Id}";

        /// <summary>
        /// Releases the ID of this object. 
        /// </summary>
        internal void Release()
        {
            ObjectAllocator.Release(Id);
        }

        public bool Equals(GameObject other) => Id == other?.Id;
    }
}
