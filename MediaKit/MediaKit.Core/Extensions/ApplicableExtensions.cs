using System;

namespace MediaKit.Core.Extensions
{
    /// <summary>
    ///     Some useful extension methods for <see cref="IApplicable" /> types
    /// </summary>
    public static class ApplicableExtensions
    {
        /// <summary>
        ///     Throws <see cref="InvalidOperationException" /> in case <paramref name="applicable">applicable</paramref> is
        ///     freezed. Suppose to use in properties setter's body.
        /// </summary>
        /// <param name="applicable">Target freezable object reference</param>
        public static void ThrowIfApplied(this IApplicable applicable)
        {
            if (applicable.IsApplied)
                throw new InvalidOperationException(
                    $"Applicable ({applicable.GetType()}) is applied and readonly for now");
        }

        /// <summary>
        ///     Calls <see cref="IApplicable.Apply" /> method only in case <see cref="IApplicable.IsApplied" /> is false. Otherwise do nothing.
        /// </summary>
        /// <param name="applicable">Target freezable object reference</param>
        public static void ApplySafety(this IApplicable applicable)
        {
            if (!applicable.IsApplied)
                applicable.Apply();
        }
    }
}