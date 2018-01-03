using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    public class DoubleAnimationUsingKeyFrames : KeyFrameAnimation<double>
    {
    }

    public class DiscreteDoubleKeyFrame : DiscreteKeyFrame<double>
    {
    }

    public class LinearDoubleKeyFrame : LinearKeyFrame<double>
    {
        protected override double GetInterpolatedValue(double x, double initialValue)
        {
            return initialValue + (Value - initialValue) * x;
        }
    }
}