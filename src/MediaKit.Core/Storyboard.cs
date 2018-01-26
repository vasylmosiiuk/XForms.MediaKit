using System;
using System.Collections.Generic;
using System.Linq;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.Core
{
    /// <summary>
    ///     Base object to manipulate animation sequences, group them and schedule their running for specific target.
    ///     The main purpose of using Storyboards instead plain Xamarin Forms animation objects is declarative way of defining
    ///     animation sequences and possibility to define storyboard inside XAML files.
    /// </summary>
    [ContentProperty(nameof(Animations))]
    public sealed class Storyboard : BindableObject, IApplicable
    {
        private Duration _duration = Duration.Automatic;
        private bool _isApplied;
        private VisualElement _target;

        /// <summary>
        ///     Unique storyboard identificator, used as Xamarin Forms animation handle as default and no explicit handle specified
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        ///     Calculated duration of storyboard. Value, produced by addition durations and offsets of animations .
        ///     <returns>
        ///         <see cref="Core.Duration.Forever" /> or explicit timespan duration will be returned.
        ///         In case <see cref="Duration" /> specified explicitly with <see cref="Core.Duration.Forever" /> or
        ///         <see cref="TimeSpan" /> it overrides calculated value. Explicit value may be used to schedule forever and
        ///         shorter than calculated animations execution.
        ///     </returns>
        /// </summary>
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

        /// <summary>
        ///     <see cref="IAnimation" /> sequence, scheduled to be executed in storyboard's scope
        /// </summary>
        public List<IAnimation> Animations { get; } = new List<IAnimation>();

        /// <summary>
        ///     <see cref="VisualElement" /> Target view, which will be animation's owner if specified. Otherwise it should be
        ///     specified externally with 'animationRoot' param
        ///     of one of <see cref="StoryboardExtensions.Begin(Storyboard, StoryboardFinishedCallback)" />
        ///     extension methods.
        /// </summary>
        public VisualElement Target
        {
            get => _target;
            set
            {
                this.ThrowIfApplied();
                _target = value;
            }
        }

        /// <summary>
        ///     Specifies, does this object freezed for new changes.
        /// </summary>
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
        ///     Allows to freeze object for new changes.
        /// </summary>
        public void Apply()
        {
            Animations.ForEach(x => x.ApplySafety());
            IsApplied = true;
        }
    }
}