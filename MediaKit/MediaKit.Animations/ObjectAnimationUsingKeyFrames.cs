using MediaKit.Animations.Abstractions;

namespace MediaKit.Animations
{
    /// <summary>
    ///     KeyFrame animation which proceed raw <see cref="object" /> values.
    /// </summary>
    public sealed class ObjectAnimationUsingKeyFrames : KeyFrameAnimation<object>
    {
    }

    /// <summary>
    ///     Discrete animation key frame which maintain raw <see cref="object" /> values.
    /// </summary>
    public sealed class DiscreteObjectKeyFrame : DiscreteKeyFrame<object>
    {
    }
}