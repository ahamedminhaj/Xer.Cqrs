﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xer.Cqrs.EventStack.Registrations
{
    public class EventHandlerRegistration : IEventHandlerRegistration, IEventHandlerResolver
    {
        #region Declarations

        private readonly EventHandlerDelegateCollectionStore _eventHandlerDelegateStore = new EventHandlerDelegateCollectionStore();

        #endregion Declarations

        #region IEventHandlerRegistration Implementation

        /// <summary>
        /// Register event async handler as subscriber.
        /// </summary>
        /// <typeparam name="TEvent">Type of event to subscribe to.</typeparam>
        /// <param name="eventAsyncHandlerFactory">Factory which will create an instance of an event handler that handles the specified <typeparamref name="TEvent"/> event.</param>
        public void Register<TEvent>(Func<IEventAsyncHandler<TEvent>> eventAsyncHandlerFactory) where TEvent : class, IEvent
        {
            if(eventAsyncHandlerFactory == null)
            {
                throw new ArgumentNullException(nameof(eventAsyncHandlerFactory));
            }

            EventHandlerDelegate newSubscribedEventHandlerDelegate = EventHandlerDelegateBuilder.FromFactory(eventAsyncHandlerFactory);

            _eventHandlerDelegateStore.Add(typeof(TEvent), newSubscribedEventHandlerDelegate);
        }

        /// <summary>
        /// Register event handler as subscriber.
        /// </summary>
        /// <typeparam name="TEvent">Type of event to subscribe to.</typeparam>
        /// <param name="eventHandlerFactory">Factory which will create an instance of an event handler that handles the specified <typeparamref name="TEvent"/> event.</param>
        public void Register<TEvent>(Func<IEventHandler<TEvent>> eventHandlerFactory) where TEvent : class, IEvent
        {
            if (eventHandlerFactory == null)
            {
                throw new ArgumentNullException(nameof(eventHandlerFactory));
            }

            EventHandlerDelegate newSubscribedEventHandlerDelegate = EventHandlerDelegateBuilder.FromFactory(eventHandlerFactory);

            _eventHandlerDelegateStore.Add(typeof(TEvent), newSubscribedEventHandlerDelegate);
        }

        #endregion IEventHandlerRegistration Implementation

        #region IEventHandlerResolver Implementation

        /// <summary>
        /// Get registered event handler delegates which handle the event of the specified type.
        /// </summary>
        /// <typeparam name="TEvent">Type of event to be handled.</typeparam>
        /// <returns>Collection of <see cref="EventHandlerDelegate"/> which executes event handler processing.</returns>
        public IEnumerable<EventHandlerDelegate> ResolveEventHandlers<TEvent>() where TEvent : class, IEvent
        {
            return _eventHandlerDelegateStore.GetEventHandlerDelegates(typeof(TEvent));
        }

        #endregion IEventHandlerResolver Implementation
    }
}
