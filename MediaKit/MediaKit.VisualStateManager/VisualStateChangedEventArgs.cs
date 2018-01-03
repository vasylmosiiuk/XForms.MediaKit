using System;
using Xamarin.Forms;

namespace MediaKit.VisualStateManager
{
    public sealed class VisualStateChangedEventArgs : EventArgs
    {
        public VisualStateChangedEventArgs(VisualState oldState, VisualState newState, VisualElement root, VisualElement target)
        {
            OldState = oldState;
            NewState = newState;
            Root = root;
            Target = target;
        }

        public VisualState OldState { get; }

        public VisualState NewState { get; }

        public VisualElement Root { get; }
        public VisualElement Target { get; }
    }
}