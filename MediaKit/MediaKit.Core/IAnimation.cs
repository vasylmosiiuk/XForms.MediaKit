using System;
using Xamarin.Forms;

namespace MediaKit.Core
{
    /// <summary>
    ///     Base interface for all MediaKit animations
    /// </summary>
    public interface IAnimation : IApplicable
    {
        /// <summary>
        ///     Explicit target bindable, so not only <see cref="VisualElement" /> allowed, but any <see cref="BindableObject" />
        /// </summary>
        BindableObject Target { get; set; }

        /// <summary>
        ///     <see cref="BindableProperty" />, which is changeable
        /// </summary>
        BindableProperty TargetProperty { get; set; }

        /// <summary>
        ///     <see cref="Core.FillBehavior" /> strategy of restoring <see cref="TargetProperty" /> after finishing animation
        /// </summary>
        FillBehavior FillBehavior { get; set; }

        /// <summary>
        ///     <see cref="Xamarin.Forms.Easing" /> object which modifies X value
        /// </summary>
        Easing Easing { get; set; }

        /// <summary>
        ///     <see cref="TimeSpan" /> delay before animation started in scope of <see cref="Storyboard" />
        /// </summary>
        TimeSpan BeginTime { get; set; }

        /// <summary>
        ///     <see cref="Core.Duration" /> of animation execution
        /// </summary>
        Duration Duration { get; set; }

        /// <summary>
        ///     <see cref="Core.RepeatBehavior" /> strategy of repeating animation
        /// </summary>
        RepeatBehavior RepeatBehavior { get; set; }

        /// <summary>
        ///     Specifies, does animation 'backs' after finishing single step
        /// </summary>
        bool AutoReverse { get; set; }

        /// <summary>
        ///     Method, executed before starting storyboard animations, used for storing original data before any animations will
        ///     be applied
        /// </summary>
        /// <param name="target"><see cref="BindableObject" />target bindable, which will be changed in scope of animation</param>
        /// <returns>Specific value, which holds some data, used by this animation</returns>
        object StoreAnimationState(BindableObject target);

        /// <summary>
        ///     Method, executed each animation step
        /// </summary>
        /// <param name="x">Current X [0;1] value</param>
        /// <param name="state">Animation state object, returned from <see cref="StoreAnimationState" /></param>
        void Update(double x, object state);

        /// <summary>
        ///     Method, executed when animation finished execution
        /// </summary>
        /// <param name="target"><see cref="BindableObject" />target bindable, changed in scope of animation</param>
        /// <param name="state">Animation state object, returned from <see cref="StoreAnimationState" /></param>
        void RestoreAnimationState(BindableObject target, object state);
    }
}