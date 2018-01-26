using System;
using MediaKit.Core;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.VisualStateManager
{
    /// <summary>
    /// Stores information, used by Visual transition, as a visual states names and animation storyboard.
    /// </summary>
    [ContentProperty(nameof(Storyboard))]
    public class VisualTransition : BindableObject, IApplicable
    {
        /// <summary>
        /// Storyboard, which contains animations sequence, will be executed when transition executes.
        /// </summary>
        public static readonly BindableProperty StoryboardProperty =
            BindableProperty.Create(nameof(Storyboard), typeof(Storyboard), typeof(VisualTransition),
                default(Storyboard), coerceValue: EnsureStoryboardCorrect,
                defaultValueCreator: (_) => new Storyboard(),
                propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Outcoming visual state name.
        /// </summary>
        public static readonly BindableProperty FromProperty =
            BindableProperty.Create(nameof(From), typeof(string), typeof(VisualTransition), default(string),
                propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Incoming visual state name.
        /// </summary>
        public static readonly BindableProperty ToProperty =
            BindableProperty.Create(nameof(To), typeof(string), typeof(VisualTransition), default(string),
                propertyChanged: OnPropertyChanged);

        private bool _isApplied;

        /// <inheritdoc cref="StoryboardProperty"/>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public Storyboard Storyboard
        {
            get => (Storyboard) GetValue(StoryboardProperty);
            set => SetValue(StoryboardProperty, value);
        }

        /// <inheritdoc cref="FromProperty"/>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public string From
        {
            get => (string) GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        /// <inheritdoc cref="ToProperty"/>
        /// <exception cref="InvalidOperationException">In case, object if freezed right now.</exception>
        public string To
        {
            get => (string) GetValue(ToProperty);
            set => SetValue(ToProperty, value);
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
            Storyboard.ApplySafety();
            IsApplied = true;
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldvalue, object newvalue) =>
            ((IApplicable) bindable).ThrowIfApplied();

        private static object EnsureStoryboardCorrect(BindableObject bindable, object value)
        {
            return value ?? new Storyboard();
        }
    }
}