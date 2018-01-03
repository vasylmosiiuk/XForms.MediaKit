using MediaKit.Animations.Abstractions;
using Xamarin.Forms;

namespace MediaKit.Animations
{
    public class ThicknessAnimationUsingKeyFrames : KeyFrameAnimation<Thickness>
    {
    }

    public class DiscreteThicknessKeyFrame : DiscreteKeyFrame<Thickness>
    {
    }

    public class LinearThicknessKeyFrame : LinearKeyFrame<Thickness>
    {
        protected override Thickness GetInterpolatedValue(double x, Thickness initialValue)
        {
            var left = initialValue.Left + (Value.Left - initialValue.Left) * x;
            var right = initialValue.Right + (Value.Right - initialValue.Right) * x;
            var top = initialValue.Top + (Value.Top - initialValue.Top) * x;
            var bottom = initialValue.Bottom + (Value.Bottom - initialValue.Bottom) * x;

            return new Thickness(left, top, right, bottom);
        }
    }
}