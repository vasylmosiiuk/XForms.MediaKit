using System;
using Xamarin.Forms;

namespace MediaKit.VisualStateManager
{
    /// <summary>
    ///     <see cref="VisualStateGroup.CurrentStateChanged" /> and <see cref="VisualStateGroup.CurrentStateChanging" /> events
    ///     payload object.
    /// </summary>
    public sealed class VisualStateChangedEventArgs : EventArgs
    {
        internal VisualStateChangedEventArgs(VisualState oldState, VisualState newState, VisualElement root,
            VisualElement target)
        {
            OldState = oldState;
            NewState = newState;
            Root = root;
            Target = target;
        }

        /// <summary>
        ///     Previous visual state in <see cref="VisualStateGroup" />.
        /// </summary>
        public VisualState OldState { get; }

        /// <summary>
        ///     Current visual state in <see cref="VisualStateGroup" />.
        /// </summary>
        public VisualState NewState { get; }

        /// <summary>
        ///     <see cref="VisualElement" />, which is <see cref="VisualStateGroup" /> owner.
        /// </summary>
        public VisualElement Root { get; }

        /// <summary>
        ///     <see cref="VisualElement" />, which is animation target.
        /// </summary>
        public VisualElement Target { get; }
    }
}