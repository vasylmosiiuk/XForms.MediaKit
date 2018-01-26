using System;
using MediaKit.Core;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.VisualStateManager
{
    /// <summary>
    ///     Contains information about visual state assigned name and storyboard.
    /// </summary>
    [ContentProperty(nameof(Storyboard))]
    public sealed class VisualState : BindableObject, IApplicable
    {
        /// <summary>
        ///     <see cref="Core.Storyboard" />, assigned to visual state, scheduled to execute when <see cref="VisualElement" />
        ///     will be switched on the state with the same <see cref="Name" />.
        /// </summary>
        public static readonly BindableProperty StoryboardProperty =
            BindableProperty.Create(nameof(Storyboard), typeof(Storyboard), typeof(VisualState),
                propertyChanged: OnPropertyChanged);

        /// <summary>
        ///     The name, assigned to this VisualState.
        /// </summary>
        public static readonly BindableProperty NameProperty =
            BindableProperty.Create(nameof(Name), typeof(string), typeof(VisualState), null,
                propertyChanged: OnPropertyChanged);

        private bool _isApplied;

        /// <inheritdoc cref="StoryboardProperty" />
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public Storyboard Storyboard
        {
            get => (Storyboard) GetValue(StoryboardProperty);
            set => SetValue(StoryboardProperty, value);
        }

        /// <inheritdoc cref="NameProperty" />
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public string Name
        {
            get => (string) GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        /// <summary>Flag, specifies, this object is in freezed state</summary>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public bool IsApplied
        {
            get => _isApplied;
            private set
            {
                this.ThrowIfApplied();
                _isApplied = value;
            }
        }

        /// <summary>
        ///     Method, which switches this object into freezed state
        /// </summary>
        public void Apply()
        {
            Storyboard?.ApplySafety();
            IsApplied = true;
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldvalue, object newvalue) =>
            ((IApplicable) bindable).ThrowIfApplied();
    }
}