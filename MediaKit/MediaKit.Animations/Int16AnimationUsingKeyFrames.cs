using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    public class Int16AnimationUsingKeyFrames : KeyFrameAnimation<short>
    {
    }

    public class DiscreteInt16KeyFrame : DiscreteKeyFrame<short>
    {
    }

    public class LinearInt16KeyFrame : LinearKeyFrame<short>
    {
        protected override short GetInterpolatedValue(double x, short initialValue)
        {
            return (short) (initialValue + (Value - initialValue) * x);
        }
    }
}