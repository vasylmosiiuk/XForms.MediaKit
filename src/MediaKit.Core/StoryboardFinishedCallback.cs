namespace MediaKit.Core
{
    /// <summary>
    ///     Used to handle storyboard finishing.
    /// </summary>
    /// <param name="storyboard"><see cref="Storyboard" />, finished executing</param>
    /// <param name="args">Extra arguments with information about animation state on finishing</param>
    public delegate void StoryboardFinishedCallback(Storyboard storyboard, StoryboardFinishedArgs args);
}