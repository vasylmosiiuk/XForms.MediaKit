using MediaKit.Core;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.Triggers.TriggerActions
{
    public class StopStoryboard : TriggerAction<VisualElement>
    {
        public Storyboard Storyboard { get; set; }
        protected override void Invoke(VisualElement target)
        {
            Storyboard?.Stop(target);
        }
    }
}