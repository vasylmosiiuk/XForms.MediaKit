using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    /// <summary>
    /// KeyFrame animation which proceed <see cref="char"/> values.
    /// </summary>
    public sealed class CharAnimationUsingKeyFrames : KeyFrameAnimation<char>
    {
    }

    /// <summary>
    /// Discrete animation key frame which maintain <see cref="char"/> values.
    /// </summary>
    public sealed class DiscreteCharKeyFrame : DiscreteKeyFrame<char>
    {
    }
}
