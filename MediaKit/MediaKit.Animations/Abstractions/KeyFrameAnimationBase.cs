using System;
using System.Collections.Generic;
using MediaKit.Core;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.Animations.Abstractions
{
    [ContentProperty(nameof(KeyFrames))]
    public abstract class
        KeyFrameAnimationBase<TAnimationValue> : AnimationBase<KeyFrameAnimationState<TAnimationValue>>
    {
        public KeyFrameCollection<TAnimationValue> KeyFrames { get; } = new KeyFrameCollection<TAnimationValue>();

        protected override KeyFrameAnimationState<TAnimationValue> StoreAnimationState(BindableObject target)
        {
            var currentValue = (TAnimationValue) target.GetValue(TargetProperty);
            return new KeyFrameAnimationState<TAnimationValue>(target, TargetProperty, currentValue);
        }

        protected override void RestoreAnimationState(BindableObject target,
            KeyFrameAnimationState<TAnimationValue> state)
        {
            if (FillBehavior == FillBehavior.Stop)
            {
                state.Target.SetValue(TargetProperty, state.StoredValue);
            }
        }

        protected override void Apply()
        {
            base.Apply();

            KeyFrames.Sort((x1, x2) => TimeSpan.Compare(x1.KeyTime, x2.KeyTime));
            KeyFrames.ForEach(x => x.Apply());
        }

        protected override void Update(double xGlobal, KeyFrameAnimationState<TAnimationValue> state)
        {
            var animationDuration = Duration.ToTimeSpan().TotalMilliseconds;
            for (var i = 0; i < KeyFrames.Count; i++)
            {
                var keyFrame = KeyFrames[i];
                var x1 = keyFrame.KeyTime.TotalMilliseconds / animationDuration;
                if (xGlobal < x1) continue;
                if (i == 1)
                {
                }
                var x2 = (i < KeyFrames.Count - 1
                             ? KeyFrames[i + 1].KeyTime.TotalMilliseconds
                             : animationDuration) / animationDuration;
                //: RepeatBehavior.RepeatEnabled
                //    ? keyFrame.KeyTime.Add(KeyFrames[0].KeyTime).TotalMilliseconds
                //    : animationDuration) / animationDuration;
                if (xGlobal > x2) continue;
                var x = (xGlobal - x1) / (x2 - x1);

                state.PreviousKeyFrameValue = i > 0 ? KeyFrames[i - 1].Value : state.StoredValue;

                keyFrame.Update(x, state);
            }
        }
    }

    public class KeyFrameCollection<TValue> : List<KeyFrame<TValue>>
    {
    }
}