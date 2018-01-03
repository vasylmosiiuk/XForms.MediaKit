using System;
using Xamarin.Forms;

namespace MediaKit.Core
{
    public interface IAnimation : IApplicable
    {
        BindableProperty TargetProperty { get; set; }
        FillBehavior FillBehavior { get; set; }
        Easing Easing { get; set; }
        TimeSpan BeginTime { get; set; }
        Duration Duration { get; set; }
        RepeatBehavior RepeatBehavior { get; set; }
        bool AutoReverse { get; set; }
        object StoreAnimationState(BindableObject target);
        void Update(double x, object state);
        void RestoreAnimationState(BindableObject target, object state);
    }
}