using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    /// <summary>
    /// KeyFrame animation which proceed <see cref="string"/> values.
    /// </summary>
    public sealed class StringAnimationUsingKeyFrames : KeyFrameAnimation<string>
    {
    }

    /// <summary>
    /// Discrete animation key frame which maintain <see cref="string"/> values.
    /// </summary>
    public sealed class DiscreteStringKeyFrame : DiscreteKeyFrame<string>
    {
    }
}
