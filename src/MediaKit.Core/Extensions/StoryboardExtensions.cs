using System;
using System.Linq;
using Xamarin.Forms;

namespace MediaKit.Core.Extensions
{
    /// <summary>
    ///     The set of extension methods for <see cref="Storyboard" />, for converting into plain Xamarin Forms animation,
    ///     scheduling execution and controlling execution of specific storyboards.
    /// </summary>
    public static class StoryboardExtensions
    {
        /// <summary>
        ///     Provide conversion from <see cref="Storyboard" /> to <see cref="Animation" /> under specified target view
        /// </summary>
        /// <param name="storyboard">Input <see cref="Storyboard" /> object</param>
        /// <param name="target">Target view</param>
        /// <returns><see cref="Animation" /> plain Xamarin Forms animation object</returns>
        public static Animation ToAnimation(this Storyboard storyboard, VisualElement target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            double durationMs;
            if (storyboard.Duration.HasTimeSpan)
                durationMs = storyboard.Duration.TimeSpan.TotalMilliseconds;
            else
                durationMs = storyboard.Animations.Where(x => x.Duration.HasTimeSpan)
                    .Max(x => x.BeginTime + x.Duration.TimeSpan).TotalMilliseconds;

            var xFormsRootAnimation = new Animation(_ => { }, 0.0, durationMs, Easing.Linear);

            foreach (var animation in storyboard.Animations)
            {
                var animationDuration = animation.CalculateExactAnimationDuration();
                var animationDurationMs = 0.0;
                if (animationDuration.HasTimeSpan)
                    animationDurationMs = animationDuration.TimeSpan.TotalMilliseconds;
                else if (animationDuration == Duration.Automatic)
                    animationDurationMs = 0;
                else if (animationDuration == Duration.Forever)
                    animationDurationMs = durationMs;

                var beginAt = Math.Max(0, animation.BeginTime.TotalMilliseconds / durationMs);
                var finishAt = Math.Max(0, (animation.BeginTime.TotalMilliseconds + animationDurationMs) / durationMs);

                Animation xFormsAnimation;
                var animationTarget = animation.Target ?? target;
                var state = animation.StoreAnimationState(animationTarget);
                if (animation.RepeatBehavior.RepeatEnabled)
                    xFormsAnimation = new Animation(RepeatUpdate(state,
                            animation.Duration.ToTimeSpan().TotalMilliseconds / durationMs, beginAt,
                            finishAt, animation.AutoReverse, animation.Update, animation.Easing), beginAt, finishAt,
                        Easing.Linear, () => animation.RestoreAnimationState(animationTarget, state));
                else
                    xFormsAnimation = new Animation(
                        Update(state, beginAt, finishAt, animation.Update, animation.Easing),
                        beginAt, finishAt, Easing.Linear,
                        () => animation.RestoreAnimationState(animationTarget, state));
                xFormsRootAnimation = xFormsRootAnimation.WithConcurrent(xFormsAnimation, beginAt, finishAt);
            }


            return xFormsRootAnimation;
        }

        /// <summary>
        ///     Schedule storyboard execution under target view, specified in <see cref="Storyboard.Target" />
        /// </summary>
        /// <param name="storyboard">Input <see cref="Storyboard" /> object</param>
        /// <param name="finishedCallback"><see cref="StoryboardFinishedCallback" /> callback, to handle animation was finished</param>
        /// <returns>Animation handle</returns>
        public static string Begin(this Storyboard storyboard, StoryboardFinishedCallback finishedCallback = null)
        {
            var handle = storyboard.Id.ToString();
            Begin(storyboard, handle, finishedCallback);

            return handle;
        }

        /// <summary>
        ///     Schedule storyboard execution under view, specified by <paramref name="animationRoot" />
        /// </summary>
        /// <param name="storyboard">Input <see cref="Storyboard" /> object</param>
        /// <param name="animationRoot">
        ///     <see cref="VisualElement" /> target view, which is Xamarin Forms animation root and default
        ///     target for any animation without explicitly specified target
        /// </param>
        /// <param name="finishedCallback"><see cref="StoryboardFinishedCallback" /> callback, to handle animation was finished</param>
        /// <returns>Animation handle</returns>
        public static string Begin(this Storyboard storyboard, VisualElement animationRoot,
            StoryboardFinishedCallback finishedCallback = null)
        {
            var handle = storyboard.Id.ToString();
            Begin(storyboard, animationRoot, handle, finishedCallback);

            return handle;
        }

        /// <summary>
        ///     Schedule storyboard execution under target view, specified in <see cref="Storyboard.Target" />
        /// </summary>
        /// <param name="storyboard">Input <see cref="Storyboard" /> object</param>
        /// <param name="handle">Explicit Xamarin Forms animation handle</param>
        /// <param name="finishedCallback"><see cref="StoryboardFinishedCallback" /> callback, to handle animation was finished</param>
        public static void Begin(this Storyboard storyboard, string handle,
            StoryboardFinishedCallback finishedCallback = null)
        {
            if (storyboard.Target == null)
                throw new InvalidOperationException(
                    "Impossible to begin animations when animation stroryboard target null");

            Begin(storyboard, storyboard.Target, finishedCallback);
        }

        /// <summary>
        ///     Schedule storyboard execution under view, specified by <paramref name="animationRoot" />
        /// </summary>
        /// <param name="storyboard">Input <see cref="Storyboard" /> object</param>
        /// <param name="animationRoot">
        ///     <see cref="VisualElement" /> target view, which is Xamarin Forms animation root and default
        ///     target for any animation without explicitly specified target
        /// </param>
        /// <param name="handle">Explicit Xamarin Forms animation handle</param>
        /// <param name="finishedCallback"><see cref="StoryboardFinishedCallback" /> callback, to handle animation was finished</param>
        public static void Begin(this Storyboard storyboard, VisualElement animationRoot, string handle,
            StoryboardFinishedCallback finishedCallback = null)
        {
            if (animationRoot == null)
                throw new ArgumentNullException(nameof(animationRoot));
            if (string.IsNullOrWhiteSpace(handle))
                throw new InvalidOperationException("Impossible to begin animations when handle is empty");

            var duration = storyboard.Duration;
            var animationLength = (uint) duration.ToTimeSpan().TotalMilliseconds;
            var animation = storyboard.ToAnimation(storyboard.Target ?? animationRoot);

            animationRoot.Animate(handle, animation, length: animationLength,
                finished: (x, cancelled) => finishedCallback?.Invoke(storyboard, new StoryboardFinishedArgs(animation, x, cancelled)));
        }

        /// <summary>
        ///     Stops executing storyboard animation under view, specified by <paramref name="animationRoot" />
        /// </summary>
        /// <param name="storyboard">Input <see cref="Storyboard" /> object</param>
        /// <param name="animationRoot">
        ///     <see cref="VisualElement" /> target view, which is Xamarin Forms animation root and default
        ///     target for any animation without explicitly specified target
        /// </param>
        public static void Stop(this Storyboard storyboard, VisualElement animationRoot)
        {
            var handle = storyboard.Id.ToString();

            Stop(storyboard, animationRoot, handle);
        }

        /// <summary>
        ///     Stops executing storyboard animation under target view, specified in <see cref="Storyboard.Target" />
        /// </summary>
        /// <param name="storyboard">Input <see cref="Storyboard" /> object</param>
        public static void Stop(this Storyboard storyboard)
        {
            var handle = storyboard.Id.ToString();

            Stop(storyboard, handle);
        }

        /// <summary>
        ///     Stops executing storyboard animation under target view, specified in <see cref="Storyboard.Target" />
        /// </summary>
        /// <param name="storyboard">Input <see cref="Storyboard" /> object</param>
        /// <param name="handle">Explicit Xamarin Forms animation handle</param>
        public static void Stop(this Storyboard storyboard, string handle)
        {
            if (storyboard.Target == null)
                throw new InvalidOperationException(
                    "Impossible to stop animations when animation stroryboard target null");

            Stop(storyboard, storyboard.Target, handle);
        }

        /// <summary>
        ///     Stops executing storyboard animation under view, specified by <paramref name="animationRoot" />
        /// </summary>
        /// <param name="storyboard">Input <see cref="Storyboard" /> object</param>
        /// <param name="animationRoot">
        ///     <see cref="VisualElement" /> target view, which is Xamarin Forms animation root and default
        ///     target for any animation without explicitly specified target
        /// </param>
        /// <param name="handle">Explicit Xamarin Forms animation handle</param>
        public static void Stop(this Storyboard storyboard, VisualElement animationRoot, string handle)
        {
            if (string.IsNullOrWhiteSpace(handle))
                throw new InvalidOperationException("Impossible to stop animations when handle is empty");

            animationRoot.AbortAnimation(handle);
        }

        private static Action<double> RepeatUpdate(object state, double singleAnimationDuration, double beginAt,
            double finishAt,
            bool autoReverse, Action<double, object> update, Easing easing)
        {
            if (singleAnimationDuration < double.Epsilon)
                throw new ArgumentOutOfRangeException(nameof(singleAnimationDuration),
                    "Forever animation should have non-zero duration");

            return xGlobal =>
            {
                if (xGlobal < finishAt)
                {
                    var timesElapsed = (uint) ((xGlobal - beginAt) / singleAnimationDuration);
                    var x = (xGlobal - beginAt - timesElapsed * singleAnimationDuration) / singleAnimationDuration;
                    if (double.IsNaN(x)) x = 1.0;

                    if (easing != null)
                        x = easing.Ease(x);
                    if (autoReverse && timesElapsed % 2 == 1)
                        x = 0.999999 - x;
                    if (x >= 0.0 && x <= 1.0)
                        update(x, state);
                }
            };
        }

        private static Action<double> Update(object state, double beginAt, double finishAt,
            Action<double, object> update, Easing easing)
        {
            var duration = finishAt - beginAt;
            return xGlobal =>
            {
                var x = (xGlobal - beginAt) / duration;
                if (double.IsNaN(x)) x = 1.0;

                if (easing != null)
                    x = easing.Ease(x);
                if (x >= 0.0 && x <= 1.0)
                    update(x, state);
            };
        }
    }
}