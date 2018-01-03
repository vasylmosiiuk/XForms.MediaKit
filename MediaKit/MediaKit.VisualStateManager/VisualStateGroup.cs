using System;
using System.Collections.Generic;
using System.Linq;
using MediaKit.Core;
using MediaKit.Core.Extensions;
using Xamarin.Forms;

namespace MediaKit.VisualStateManager
{
    [ContentProperty(nameof(States))]
    public class VisualStateGroup : BindableObject, IApplicable
    {
        private static readonly BindablePropertyKey TransitionsPropertyKey =
            BindableProperty.CreateReadOnly(nameof(Transitions), typeof(VisualTransitionCollection),
                typeof(VisualStateGroup), null, defaultValueCreator: (_) => new VisualTransitionCollection());

        public static readonly BindableProperty TransitionsProperty = TransitionsPropertyKey.BindableProperty;

        private static readonly BindablePropertyKey StatesPropertyKey =
            BindableProperty.CreateReadOnly(nameof(States), typeof(VisualStateCollection), typeof(VisualStateGroup),
                null, defaultValueCreator: (_) => new VisualStateCollection());

        public static readonly BindableProperty StatesProperty = StatesPropertyKey.BindableProperty;

        public static readonly BindableProperty NameProperty =
            BindableProperty.Create(nameof(Name), typeof(string), typeof(VisualStateGroup), default(string),
                propertyChanged: OnPropertyChanged);

        private static readonly BindablePropertyKey CurrentStatePropertyKey = BindableProperty.CreateReadOnly(
            nameof(CurrentState),
            typeof(VisualState), typeof(VisualStateGroup), default(VisualState));

        public static readonly BindableProperty CurrentStateProperty = CurrentStatePropertyKey.BindableProperty;
        private readonly List<VisualTransition> _generatedTransitions = new List<VisualTransition>();
        private bool _isApplied;

        public VisualState CurrentState
        {
            get => (VisualState) GetValue(CurrentStateProperty);
            private set => SetValue(CurrentStatePropertyKey, value);
        }

        public VisualStateCollection States
        {
            get => (VisualStateCollection) GetValue(StatesProperty);
            private set => SetValue(StatesPropertyKey, value);
        }

        public VisualTransitionCollection Transitions
        {
            get => (VisualTransitionCollection) GetValue(TransitionsProperty);
            private set => SetValue(TransitionsPropertyKey, value);
        }

        public string Name
        {
            get => (string) GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public bool IsApplied
        {
            get => _isApplied;
            private set
            {
                this.ThrowIfApplied();
                _isApplied = value;
            }
        }

        public void Apply()
        {
            Transitions.ForEach(x => x.ApplySafety());
            States.ForEach(x => x.ApplySafety());
            IsApplied = true;
        }

        private static void OnPropertyChanged(BindableObject bindable, object oldvalue, object newvalue) =>
            ((IApplicable) bindable).ThrowIfApplied();


        public event EventHandler<VisualStateChangedEventArgs> CurrentStateChanging;
        public event EventHandler<VisualStateChangedEventArgs> CurrentStateChanged;

        internal void GoToState(VisualElement root, VisualElement target, string stateName, bool useTransitions)
        {
            var currentState = CurrentState;
            var currentStateName = currentState?.Name;
            if (currentStateName == stateName) return;

            var availableStates = States;
            var stateToSet = availableStates.FirstOrDefault(x => x.Name == stateName);
            if (stateToSet == null) return;

            var handler = VisualStateManager.GetAnimationHandler(target);
            var animationGroupHandler = $"{handler}_{Name}";

            target.AbortAnimation(animationGroupHandler);

            RaiseCurrentStateChanging(currentState, stateToSet, root, target);

            void StateSet()
            {
                CurrentState = stateToSet;
                RaiseCurrentStateChanged(currentState, stateToSet, root, target);
            }

            void RunAnimation()
            {
                stateToSet.Storyboard.Begin(target, animationGroupHandler, (animation, x, cancelled) =>
                {
                    if (cancelled)
                        animation.GetCallback()(1.0);
                });
            }

            if (useTransitions)
            {
                var transition = ResolveVisualTransition(currentStateName, stateName);
                transition = transition ?? GenerateVisualTransition(target, currentStateName, stateName);
                if (transition?.Storyboard != null)
                {
                    void TransitionAnimationFinished(Animation animation, double x, bool cancelled)
                    {
                        if (cancelled)
                        {
                            animation.GetCallback()(1.0);
                            return;
                        }

                        RunAnimation();
                        StateSet();
                    }

                    transition.Storyboard.Begin(target, animationGroupHandler, TransitionAnimationFinished);
                    return;
                }
            }

            RunAnimation();
            StateSet();
        }

        private void RaiseCurrentStateChanging(VisualState fromState, VisualState toState, VisualElement root,
            VisualElement element)
        {
            CurrentStateChanging?.Invoke(this, new VisualStateChangedEventArgs(fromState, toState, root, element));
        }

        private void RaiseCurrentStateChanged(VisualState fromState, VisualState toState, VisualElement root,
            VisualElement element)
        {
            CurrentStateChanged?.Invoke(this, new VisualStateChangedEventArgs(fromState, toState, root, element));
        }

        private VisualTransition GenerateVisualTransition(VisualElement target, string fromState,
            string toState)
        {
            var transition = new VisualTransition
            {
                From = fromState,
                To = toState,
                Storyboard = GenerateVisualTransitionStoryboard(target, fromState, toState)
            };

            _generatedTransitions.Add(transition);

            return transition;
        }

        protected virtual Storyboard GenerateVisualTransitionStoryboard(VisualElement target, string fromState,
            string toState)
        {
            return new Storyboard();
        }

        protected virtual VisualTransition ResolveVisualTransition(string fromVisualState, string toVisualState)
        {
            bool ExplicitTransitionsFilter(VisualTransition t) =>
                t.From == fromVisualState || t.To == toVisualState || t.From == null && t.To == null;

            bool GeneratedTransitionsFilter(VisualTransition t) => t.From == fromVisualState && t.To == toVisualState;
            int KeySelector(VisualTransition t) => CalculateTransitionWeight(t, fromVisualState, toVisualState);

            var explicitTransitions = Transitions.Where(ExplicitTransitionsFilter).OrderByDescending(KeySelector);

            var transition = explicitTransitions.FirstOrDefault();
            transition = transition ?? _generatedTransitions.FirstOrDefault(GeneratedTransitionsFilter);

            return transition;
        }

        protected virtual int CalculateTransitionWeight(VisualTransition transition, string fromVisualState,
            string toVisualState)
        {
            if (transition.From == null && transition.To == null)
                return 0; //Explicit default transition

            var weight = 0;
            if (transition.From == fromVisualState) weight += 2; //Equality of 'from' state is more powerful
            if (transition.To == toVisualState) weight += 1;

            //so, possible to retrive 0 - default, 1 - when to states equals, 2 - when from states equals, 3 - full equal
            return weight;
        }
    }

    public sealed class VisualStateCollection : List<VisualState>
    {
        internal VisualStateCollection(IEnumerable<VisualState> enumerable) : base(enumerable)
        {
        }

        public VisualStateCollection()
        {
        }
    }

    public sealed class VisualTransitionCollection : List<VisualTransition>
    {
    }
}