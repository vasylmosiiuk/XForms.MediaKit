using Xamarin.Forms;

namespace MediaKit.Animations.Abstractions
{
    /// <summary>
    ///     Base class, used by MediaKit built-in KeyFrame animations for storing state.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KeyFrameAnimationState<T> : AnimationState
    {
        /// <summary>
        ///     Initial value of <see cref="AnimationState.Target" /> before animation applied.
        /// </summary>
        public readonly T StoredValue;

        /// <summary>
        ///     TargetProperty of <see cref="AnimationState.Target" />, which will be changed.
        /// </summary>
        public readonly BindableProperty TargetProperty;

        /// <summary>
        ///     Last KeyFrame value.
        /// </summary>
        public T PreviousKeyFrameValue;


        /// <summary>
        ///     Creates a <see cref="KeyFrameAnimationState{T}" /> with specified <see cref="AnimationState.Target" />,
        ///     <see cref="TargetProperty" /> and <see cref="StoredValue" />.
        /// </summary>
        /// <param name="target">Animation's target.</param>
        /// <param name="targetProperty">Bindable property, which will be changed by animation.</param>
        /// <param name="storedValue">A value, which is initial value of target property.</param>
        public KeyFrameAnimationState(BindableObject target, BindableProperty targetProperty, T storedValue) :
            base(target)
        {
            TargetProperty = targetProperty;
            StoredValue = storedValue;
            PreviousKeyFrameValue = storedValue;
        }
    }
}