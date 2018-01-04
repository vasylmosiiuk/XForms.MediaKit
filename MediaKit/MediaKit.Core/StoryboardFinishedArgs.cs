using Xamarin.Forms;

namespace MediaKit.Core
{
    /// <summary>
    ///     Extra arguments with information about animation state on finishing
    /// </summary>
    public class StoryboardFinishedArgs
    {
        /// <summary>
        ///     <see cref="Xamarin.Forms.Animation" />Xamarin Forms animation, which was finished
        /// </summary>
        public readonly Animation Animation;

        /// <summary>
        ///     Flag, marks that animation was cancelled
        /// </summary>
        public readonly bool IsCancelled;

        /// <summary>
        ///     Animation state on finishing
        /// </summary>
        public readonly double X;

        /// <summary>
        ///     Extra arguments with information about animation state on finishing
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="x"></param>
        /// <param name="isCancelled"></param>
        public StoryboardFinishedArgs(Animation animation, double x, bool isCancelled)
        {
            Animation = animation;
            X = x;
            IsCancelled = isCancelled;
        }
    }
}