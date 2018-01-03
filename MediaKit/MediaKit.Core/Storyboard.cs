using System;
using System.Collections.Generic;
using System.Linq;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.Core
{
    [ContentProperty(nameof(Animations))]
    public sealed class Storyboard : BindableObject, IApplicable
    {
        private Duration _duration = Duration.Automatic;
        private bool _isApplied;
        private VisualElement _target;
        public Guid Id { get; } = Guid.NewGuid();

        public Duration Duration
        {
            get
            {
                if (_duration == Duration.Automatic)
                    return new Duration(Animations.Any()
                        ? Animations.Max(x =>
                            x.CalculateExactAnimationDuration().ToTimeSpan().SumSafety(x.BeginTime))
                        : TimeSpan.Zero);

                return _duration;
            }
            set
            {
                this.ThrowIfApplied();
                _duration = value;
            }
        }

        public List<IAnimation> Animations { get; } = new List<IAnimation>();

        public VisualElement Target
        {
            get => _target;
            set
            {
                this.ThrowIfApplied();
                _target = value;
            }
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
            Animations.ForEach(x => x.ApplySafety());
            IsApplied = true;
        }
    }
}