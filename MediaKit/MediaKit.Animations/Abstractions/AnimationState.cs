using Xamarin.Forms;

namespace MediaKit.Animations.Abstractions
{
    public abstract class AnimationState
    {
        public readonly BindableObject Target;

        protected AnimationState(BindableObject target)
        {
            Target = target;
        }
    }
}