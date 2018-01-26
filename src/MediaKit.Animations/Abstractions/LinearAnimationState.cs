using MediaKit.Core;
using Xamarin.Forms;

namespace MediaKit.Animations.Abstractions
{
    /// <summary>
    ///     Object, used by Linear animations to store state.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LinearAnimationState<T> : AnimationState
    {
        /// <summary>
        ///     Begin value.
        /// </summary>
        public readonly T From;

        /// <summary>
        ///     Original value of <see cref="IAnimation.TargetProperty" />.
        /// </summary>
        public readonly T StoredValue;

        /// <summary>
        ///     Creates <see cref="LinearAnimationState{T}" /> with <see cref="IAnimation.Target" />, initial value and begin
        ///     value.
        /// </summary>
        /// <param name="target">Animation's target.</param>
        /// <param name="storedValue">Initial value.</param>
        /// <param name="from">Begin value.</param>
        public LinearAnimationState(BindableObject target, T storedValue, T from) : base(target)
        {
            StoredValue = storedValue;
            From = from;
        }
    }
}