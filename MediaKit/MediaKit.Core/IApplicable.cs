using System;
using MediaKit.Core.Extensions;

namespace MediaKit.Core
{
    /// <summary>
    ///     Interface that specifies freezable type contract. Any type, implementing it can be freezed by calling
    ///     <see cref="Apply" /> method, and <see cref="IsApplied" /> will be set to true. Next calls of this method will cause
    ///     <see cref="InvalidOperationException" />. You also may use <see cref="ApplicableExtensions.ApplySafety" /> to
    ///     freeze object in case, object isn't freezed yet, otherwise do nothing.
    /// </summary>
    public interface IApplicable
    {
        /// <summary>
        ///     Flag, specifies, this object is in freezed state
        /// </summary>
        bool IsApplied { get; }

        /// <summary>
        ///     Method, which switches this object into freezed state
        /// </summary>
        /// <exception cref="InvalidOperationException">In case this object is freezed right now</exception>
        void Apply();
    }
}