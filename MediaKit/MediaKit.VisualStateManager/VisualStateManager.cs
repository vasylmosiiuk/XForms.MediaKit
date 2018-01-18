using System;
using System.Collections.Generic;
using System.Linq;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.VisualStateManager
{
    /// <summary>
    ///     Allows to manage visual states groups and visual states for VisualElements.
    /// </summary>
    public class VisualStateManager : BindableObject
    {
        private static readonly BindablePropertyKey AnimationHandlerPropertyKey = BindableProperty.CreateReadOnly(
            "AnimationHandler",
            typeof(string), typeof(VisualStateManager), null,
            defaultValueCreator: GenerateAnimationHandler);

        internal static readonly BindableProperty AnimationHandlerProperty =
            AnimationHandlerPropertyKey.BindableProperty;

        /// <summary>
        ///     Collection, which contains a sequence of <see cref="VisualStateGroup" />, assigned to specific
        ///     <see cref="VisualElement" />.
        /// </summary>
        public static readonly BindableProperty VisualStateGroupsProperty = BindableProperty.CreateAttached(
            "VisualStateGroups", typeof(VisualStateGroupCollection), typeof(VisualStateManager), null,
            propertyChanged: OnVisualStateGroupsChanged);

        private static object GenerateAnimationHandler(BindableObject bindable)
        {
            if (bindable is Element element)
                return element.Id.ToString();
            return Guid.NewGuid().ToString();
        }

        private static void OnVisualStateGroupsChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (newvalue is VisualStateGroupCollection visualStateGroups)
                visualStateGroups.ForEach(x => x.ApplySafety());
        }

        internal static string GetAnimationHandler(BindableObject bindable) =>
            (string) bindable.GetValue(AnimationHandlerProperty);

        /// <summary>
        ///     Retrieve a <see cref="VisualStateGroupCollection" /> of assigned visual states groups for specified bindable.
        /// </summary>
        /// <param name="bindable">Target bindable.</param>
        /// <returns>Collection of assigned to target bindable visual states groups.</returns>
        public static VisualStateGroupCollection GetVisualStateGroups(BindableObject bindable)
        {
            return (VisualStateGroupCollection) bindable.GetValue(VisualStateGroupsProperty);
        }

        /// <summary>
        ///     Assign a <see cref="VisualStateGroupCollection" /> of visual states groups to specified bindable.
        /// </summary>
        /// <param name="bindable">Target bindable.</param>
        /// <param name="value"><see cref="VisualStateGroup" /> collection, should be assigned to <paramref name="bindable" />.</param>
        public static void SetVisualStateGroups(BindableObject bindable, VisualStateGroupCollection value)
        {
            bindable.SetValue(VisualStateGroupsProperty, value);
        }

        /// <summary>
        ///     Schedules <see cref="VisualState" /> changing for specified by <paramref name="element" /> target visual.
        /// </summary>
        /// <param name="element">Target visual element.</param>
        /// <param name="stateName">Visual state name to apply.</param>
        /// <param name="useTransitions">Flag, which marks, will use any transitions or apply visual state immediately.</param>
        public static void GoToState(VisualElement element, string stateName, bool useTransitions)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var target = element;
            if (element is TemplatedView templatedView && templatedView.ControlTemplate != null)
            {
                var controlTemplateRoot = templatedView.Children.FirstOrDefault() as VisualElement;
                if (GetVisualStateGroups(controlTemplateRoot) != null)
                    target = controlTemplateRoot;
            }

            var visualStateGroup = GetVisualStateGroups(target)
                .FirstOrDefault(x => x.States.Any(y => y.Name == stateName));

            visualStateGroup?.GoToState(element, target, stateName, useTransitions);
        }
    }

    /// <summary>
    ///     Collection which stores <see cref="VisualStateGroup" /> objects sequence.
    /// </summary>
    public class VisualStateGroupCollection : List<VisualStateGroup>
    {
    }
}