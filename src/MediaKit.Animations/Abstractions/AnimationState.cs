using Xamarin.Forms;

namespace MediaKit.Animations.Abstractions
{
    /// <summary>
    /// Base animation state object, used by MediaKit built-in animation. You may use it in your custom animations.
    /// </summary>
    public abstract class AnimationState
    {
        /// <summary>
        /// Animation's target object.
        /// </summary>
        public readonly BindableObject Target;

        /// <summary>
        /// Base object constructor.
        /// </summary>
        /// <param name="target">Animation's target object to store.</param>
        protected AnimationState(BindableObject target)
        {
            Target = target;
        }
    }
}