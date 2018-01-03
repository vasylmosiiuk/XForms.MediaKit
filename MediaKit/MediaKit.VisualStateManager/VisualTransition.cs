using MediaKit.Core;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.VisualStateManager
{
    [ContentProperty(nameof(Storyboard))]
    public class VisualTransition : BindableObject, IApplicable
    {
        public static readonly BindableProperty StoryboardProperty =
            BindableProperty.Create(nameof(Storyboard), typeof(Storyboard), typeof(VisualTransition),
                default(Storyboard), coerceValue: EnsureStoryboardCorrect,
                defaultValueCreator: (_) => new Storyboard(),
                propertyChanged: OnPropertyChanged);

        public static readonly BindableProperty FromProperty =
            BindableProperty.Create(nameof(From), typeof(string), typeof(VisualTransition), default(string),
                propertyChanged: OnPropertyChanged);


        public static readonly BindableProperty ToProperty =
            BindableProperty.Create(nameof(To), typeof(string), typeof(VisualTransition), default(string),
                propertyChanged: OnPropertyChanged);

        private bool _isApplied;

        public Storyboard Storyboard
        {
            get => (Storyboard) GetValue(StoryboardProperty);
            set => SetValue(StoryboardProperty, value);
        }

        public string From
        {
            get => (string) GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        public string To
        {
            get => (string) GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        public bool IsApplied
        {
            get => _isApplied;
            private set
            {
                this.ThrowIfApplied();
                _isApplied = value;
            }
        }

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