using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    /// <summary>
    /// KeyFrame animation which proceed <see cref="bool"/> values.
    /// </summary>
    public sealed class BooleanAnimationUsingKeyFrames : KeyFrameAnimation<bool>
    {

    }

    /// <summary>
    /// Discrete animation key frame which maintain <see cref="bool"/> values.
    /// </summary>
    public sealed class DiscreteBooleanKeyFrame : DiscreteKeyFrame<bool>
    {

    }
}