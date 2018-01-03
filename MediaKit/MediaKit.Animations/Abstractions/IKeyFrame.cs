using System;
using MediaKit.Core;

namespace MediaKit.Animations.Abstractions
{
    public interface IKeyFrame<TValue> : IApplicable
    {
        TimeSpan KeyTime { get; set; }
        TValue Value { get; set; }
    }
}