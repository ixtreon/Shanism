namespace Shanism.Common
{
    public enum MovementType
    {
        None,
        Walking,
        Swimming,
        Amphibious,
        Flying,
    }

    public static class MovementTypeExt
    {
        /// <summary>
        /// Gets whether this movement type allows crossing water terrain.
        /// </summary>
        public static bool CanSwim(this MovementType type)
            => type == MovementType.Amphibious
            || type == MovementType.Flying
            || type == MovementType.Swimming;

        /// <summary>
        /// Gets whether this movement type allows crossing ground terrain.
        /// </summary>
        public static bool CanWalk(this MovementType type)
            => type == MovementType.Amphibious
            || type == MovementType.Flying
            || type == MovementType.Walking;
    }
}
