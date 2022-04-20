using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FocalPoint.Behaviors
{
    public class EventToCommandBehavior : BehaviorBase<View>
    {
        Delegate eventHandler;

        public static readonly BindableProperty EventNameProperty = BindableProperty.Create(
            "EventName",
            typeof(string),
            typeof(EventToCommandBehavior),
            null,
            propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            "Command",
            typeof(ICommand),
            typeof(EventToCommandBehavior),
            null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            "CommandParameter",
            typeof(object),
            typeof(EventToCommandBehavior),
            null);
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create(
            "Converter",
            typeof(IValueConverter),
            typeof(EventToCommandBehavior),
            null);

        #region Properties
        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }
        #endregion

        #region Methods
        private void RegisterEvent(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);

                if (eventInfo != null)
                {
                    MethodInfo methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
                    eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
                    eventInfo.AddEventHandler(AssociatedObject, eventHandler);
                }
                else
                {
                    throw new ArgumentException(string.Format("EventToCommandBehavior: Can't register the '{0}' event.", EventName));
                }
            }
        }

        private void DeregisterEvent(string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && eventHandler != null)
            {
                EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);

                if (eventInfo != null)
                {
                    eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
                    eventHandler = null;
                }
                else
                {
                    throw new ArgumentException(string.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", EventName));
                }
            }
        }
        #endregion

        #region Overrides
        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }
        #endregion

        #region Event handlers
        private void OnEvent(object sender, object eventArgs)
        {
            if (Command != null)
            {
                object resolvedParameter;

                if (CommandParameter != null)
                {
                    resolvedParameter = CommandParameter;
                }
                else if (Converter != null)
                {
                    resolvedParameter = Converter.Convert(eventArgs, typeof(object), null, null);
                }
                else
                {
                    resolvedParameter = eventArgs;
                }

                if (Command.CanExecute(resolvedParameter))
                {
                    Command.Execute(resolvedParameter);
                }
            }
        }

        private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is EventToCommandBehavior behavior)
            {
                if (behavior.AssociatedObject != null)
                {
                    string oldEventName = (string)oldValue;
                    string newEventName = (string)newValue;

                    behavior.DeregisterEvent(oldEventName);
                    behavior.RegisterEvent(newEventName);
                }
            }
        }
        #endregion
    }
}
