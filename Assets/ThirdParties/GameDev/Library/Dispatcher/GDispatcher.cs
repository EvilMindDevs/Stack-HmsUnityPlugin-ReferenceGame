using System.Collections.Generic;
using System;
using UnityEngine;

namespace GameDev.Library
{

    /*
		!!! Event and Delegate differentiation
		
		Event is only a modifier on delegate which brings restriction that 
		compiler enforces. Additional values for Event.

		1) Events and Interface
		An event can be included in an interface declaration, a field
		- this includes delegates - can not.

		2) Event Invocation
		Event only can be invoked from withing the class that declared it.
		To call it on derived class, event invocation code must be in a virtual method which
		could be overrided on derived class.
		
		3) Event Accessors
		There are accessor method of event like conventional getter-setter methods.
	*/

    /*
		!!! UnityEvent class
		
		UnityEvent is a abstract class which wraps and manages the event/delegate data and 
		gives interface to AddListener/RemoveListener/Invoke methods to handle 
		callback mechanism on its own way. In my opinion, wrapping event delegate
		variables is both unjustifiable and costly; there is not any benefits from
		using it.
	*/

    public delegate void EventHandler<TEventData>(object sender, GEvent<TEventData> eventData);
    public class GDispatcher : GSingletonDontDestroy<GDispatcher>
    {
        private Dictionary<string, Dictionary<object, Delegate>> _listeners = new Dictionary<string, Dictionary<object, Delegate>>();
        private static readonly object _GlobalListener = new object();

        // TODO: Delegate object has its Target property which defined the delegate. You could use it instead of sending 
        // Objects explicity
        public void add<TEventData>(string eventName, EventHandler<TEventData> handler)
        {
            Add(eventName, _GlobalListener, handler);
        }

        public void Add<TEventData>(string eventName, object listener, EventHandler<TEventData> handler)
        {
            if (_listeners.ContainsKey(eventName))
            {
                Dictionary<object, Delegate> innerDict = _listeners [eventName];
                if (innerDict.ContainsKey(listener))
                {
                    EventHandler<TEventData> multicastHandler = (EventHandler<TEventData>)innerDict [listener];
                    multicastHandler += handler;
                    innerDict [listener] = multicastHandler;
                }
                else
                {
                    innerDict [listener] = handler;
                }
            }
            else
            {
                Dictionary<object, Delegate> innerDict = new Dictionary<object, Delegate>();
                innerDict [listener] = handler;
                _listeners [eventName] = innerDict;
            }
        }

        public void remove<TEventData>(string eventName, EventHandler<TEventData> handler)
        {
            Remove(eventName, _GlobalListener, handler);
        }

        public void Remove<TEventData>(string eventName, object listener, EventHandler<TEventData> handler)
        {
            if (_listeners.ContainsKey(eventName))
            {
                Dictionary<object, Delegate> innerDict = _listeners [eventName];
                if (innerDict.ContainsKey(listener))
                {
                    EventHandler<TEventData> multicastHandler = (EventHandler<TEventData>)innerDict [listener];

                    if (multicastHandler == null)
                    {
                        throw new Exception("[Dispatcher::remove<TEventData>] multicastHandler is null, cannot operate subtutive operation.");
                    }
                    multicastHandler -= handler;
                    if (multicastHandler == null)
                    {
                        innerDict.Remove(listener);
                    }
                    else
                    {
                        innerDict [listener] = multicastHandler;
                    }
                }

                if (_listeners [eventName].Count <= 0)
                {
                    _listeners.Remove(eventName);
                }
            }
        }

        // Local propogation true means send event only to components whose are part of the event dispatcher entity.
        public void Dispatch<TEventData>(object sender, GEvent<TEventData> eventData, bool localPropagation = false)
        {
            object listener = (localPropagation == true) ? (sender as Component).gameObject : _GlobalListener;
            if (_listeners.ContainsKey(eventData.EventName))
            {
                Dictionary<object, Delegate> innerDict = _listeners [eventData.EventName];
                if (localPropagation == false)
                {
                    EventHandler<TEventData> multicastDelegate = null;
                    List<object> keys = new List<object>(innerDict.Keys);
                    foreach (object key in keys)
                    {
                        try
                        {
                            multicastDelegate = (EventHandler<TEventData>)innerDict [key];
                            if (multicastDelegate.GetInvocationList().Length <= 0)
                            {
                                Debug.LogError("[Dispatcher::dispatch] Why try to call multicastDeelgate with zero invocation list?");
                            }
                            else
                            {
                                multicastDelegate(sender, eventData);
                            }

                        }
                        catch (Exception ex)
                        {
                            // Listener method'un ikinci parametresi uyusmadiginda bu hatayi aliyo.
                            // Type-check yapmak lazim
                            Debug.LogError(ex);
                        }
                    }
                }
                else
                {
                    if (innerDict.ContainsKey(listener))
                    {
                        try
                        {
                            EventHandler<TEventData> multicastDelegate = (EventHandler<TEventData>)innerDict [listener];
                            multicastDelegate(sender, eventData);
                        }
                        catch (Exception ex)
                        {
                            // Listener method'un ikinci parametresi uyusmadiginda bu hatayi aliyo.
                            // Type-check yapmak lazim
                            Debug.LogError(ex);
                        }
                    }
                }
            }
        }

        // Propagate event through ancaster
        // TODO: Need to be refactored.
        public void Propagate<TEventData>(object sender, GEvent<TEventData> eventData)
        {
            //Transform parent = (sender as Component).transform.parent;
            Transform parent = (sender as Component).transform;
            if (parent == null) { return; }

            if (_listeners.ContainsKey(eventData.EventName))
            {
                Dictionary<object, Delegate> innerDict = _listeners [eventData.EventName];
                while (parent != null)
                {
                    if (innerDict.ContainsKey(parent.gameObject))
                    {
                        try
                        {
                            EventHandler<TEventData> multicastDelegate = (EventHandler<TEventData>)innerDict [parent.gameObject];
                            multicastDelegate(parent.transform, eventData);

                            if (parent.parent != null)
                            {
                                propagateRecursively(innerDict, multicastDelegate, parent.parent, eventData);
                                parent = null;
                            }
                            else
                            {
                                parent = parent.parent;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Listener method'un ikinci parametresi uyusmadiginda bu hatayi aliyo.
                            // Type-check yapmak lazim
                            Debug.LogError(ex);
                            break;
                        }
                    }
                    else
                    {
                        parent = parent.parent;
                    }
                }
            }
        }

        // TODO: Need to be refactored.
        void propagateRecursively<TEventData>(Dictionary<object, Delegate> innerDict,
                                             EventHandler<TEventData> multicastDelegate,
                                             Transform listener,
                                             GEvent<TEventData> eventData)
        {

            if (innerDict.ContainsKey(listener))
            {
                multicastDelegate(listener, eventData);
                if (listener.parent != null)
                {
                    propagateRecursively(innerDict, multicastDelegate, listener.parent, eventData);
                }
            }
        }
    }
}