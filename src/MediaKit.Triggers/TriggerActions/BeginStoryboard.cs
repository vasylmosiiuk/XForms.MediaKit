using MediaKit.Core;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.Triggers.TriggerActions
{
    /// <summary>
    ///     Trigger action, activates specified <see cref="Storyboard" /> with trigger's owner as an animation root.
    /// </summary>
    public sealed class BeginStoryboard : TriggerAction<VisualElement>
    {
        /// <summary>
        ///     <see cref="MediaKit.Core.Storyboard" /> object, which is trigger's action target.
        /// </summary>
        public Storyboard Storyboard { get; set; }

        /// <summary>
        ///     Application developers override this method to provide the action that is performed when the trigger condition
        ///     is met.
        /// </summary>
        /// <param name="target">The object on which to invoke the trigger action.</param>
        protected override void Invoke(VisualElement target)
        {
            Storyboard?.Begin(target);
        }
    }
}