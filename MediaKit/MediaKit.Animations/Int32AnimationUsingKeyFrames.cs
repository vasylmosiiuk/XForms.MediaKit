using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    public class Int32AnimationUsingKeyFrames : KeyFrameAnimation<int>
    {
    }

    public class DiscreteInt32KeyFrame : DiscreteKeyFrame<int>
    {
    }

    public class LinearInt32KeyFrame : LinearKeyFrame<int>
    {
        protected override int GetInterpolatedValue(double x, int initialValue)
        {
            return (int)(initialValue + (Value - initialValue) * x);
        }
    }
}