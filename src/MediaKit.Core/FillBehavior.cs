namespace MediaKit.Core
{
    /// <summary>
    ///     Strategy of restoring <see cref="IAnimation.TargetProperty" /> value after finishing animation
    /// </summary>
    public enum FillBehavior
    {
        /// <summary>
        ///     Doesn't restory value
        /// </summary>
        HoldEnd,

        /// <summary>
        ///     Restore to origin value
        /// </summary>
        Stop
    }
}