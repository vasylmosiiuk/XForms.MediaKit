﻿using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    /// <summary>
    ///     Linear animation which proceed <see cref="double" /> values.
    /// </summary>
    public sealed class DoubleAnimation : LinearPrimitiveAnimation<double>
    {
        /// <summary>
        ///     Method, executed each animation step.
        /// </summary>
        /// <param name="x">Current X [0;1] value.</param>
        /// <param name="state">
        ///     Animation state object, created by <see cref="AnimationBase{TState}.StoreAnimationState" /> and
        ///     returned.
        /// </param>
        protected override void Update(double x, LinearAnimationState<double> state)
        {
            var value = state.From + (To - state.From) * x;
            state.Target.SetValue(TargetProperty, value);
        }
    }
}