namespace Shanism.Engine.Models.Systems
{
    /// <summary>
    /// A system for the <see cref="ShanoEngine"/>.
    /// </summary>
    abstract class GameSystem
    {
        /// <summary>
        /// Gets the name of the system.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Updates the game system.
        /// </summary>
        /// <param name="msElapsed">The time elapsed since the last call to <see cref="Update(int)"/> in milliseconds.</param>
        internal abstract void Update(int msElapsed);
    }
}
