namespace Shanism.Client
{
    public interface IActivatable
    {

        /// <summary>
        /// Gets whether the game window is currently active.
        /// </summary>
        bool IsActive { get; }

    }
}
