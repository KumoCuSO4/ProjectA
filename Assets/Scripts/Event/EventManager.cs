using System;
using System.Collections.Generic;
using Interface;
using Utils;

namespace Event
{
    // 事件监听
    public class EventManager : Singleton<EventManager>
    {
        public delegate void Func(params object[] values);
        
        private const int MAX_PRI = 2;
        private readonly Dictionary<Events, Delegate>[] _eventListeners = new Dictionary<Events, Delegate>[MAX_PRI];
    
        public EventManager()
        {
            LogManager.Log("EventManager created");
            for (int i = 0; i < MAX_PRI; i++)
            {
                _eventListeners[i] = new Dictionary<Events, Delegate>();
            }
        }
        
        public bool AddListener<T>(Events eventType, Action<T> function, EventPriority priority = EventPriority.OUTER)
        {
            int p = (int)priority;
            _eventListeners[p].TryAdd(eventType, null);
            if (_eventListeners[p][eventType] != null)
            {
                if (_eventListeners[p][eventType].GetType() != typeof(Action<T>))
                {
                    LogManager.LogError("添加事件失败，参数不一致", _eventListeners[p][eventType].GetType(), typeof(Action<T>));
                    return false;
                }

                Delegate[] list = _eventListeners[p][eventType].GetInvocationList();
                int index = Array.IndexOf(list, function);
                if (index > -1)
                {
                    LogManager.LogError("添加事件失败，委托已存在", _eventListeners[p][eventType].GetType(), typeof(Action<T>));
                    return false;
                }
            }
        
            _eventListeners[p][eventType] = (Action<T>)_eventListeners[p][eventType] + function;
            return true;
        }
    
        public bool AddListener(Events eventType, Action function, EventPriority priority = EventPriority.OUTER)
        {
            int p = (int)priority;
            _eventListeners[p].TryAdd(eventType, null);
            if (_eventListeners[p][eventType] != null)
            {
                if (_eventListeners[p][eventType].GetType() != typeof(Action))
                {
                    LogManager.LogError("添加事件失败，参数不一致", _eventListeners[p][eventType].GetType(), typeof(Action));
                    return false;
                }

                Delegate[] list = _eventListeners[p][eventType].GetInvocationList();
                int index = Array.IndexOf(list, function);
                if (index > -1)
                {
                    LogManager.LogError("添加事件失败，委托已存在", _eventListeners[p][eventType].GetType(), typeof(Action));
                    return false;
                }
            }
        
            _eventListeners[p][eventType] = (Action)_eventListeners[p][eventType] + function;
            return true;
        }

        public void RemoveListener<T>(Events eventType, Action<T> function, EventPriority priority = EventPriority.OUTER)
        {
            int p = (int)priority;
            if (!_eventListeners[p].ContainsKey(eventType)) return;
        
            _eventListeners[p][eventType] = (Action<T>)_eventListeners[p][eventType] - function;

            if (_eventListeners[p][eventType] == null)
            {
                _eventListeners[p].Remove(eventType);
            }
        }
    
        public void RemoveListener(Events eventType, Action function, EventPriority priority = EventPriority.OUTER)
        {
            int p = (int)priority;
            if (!_eventListeners[p].ContainsKey(eventType)) return;
        
            _eventListeners[p][eventType] = (Action)_eventListeners[p][eventType] - function;

            if (_eventListeners[p][eventType] == null)
            {
                _eventListeners[p].Remove(eventType);
            }
        }
        
        public void TriggerEvent<T>(Events eventType, T parameter)
        {
            for (int i = 0; i < MAX_PRI; i++)
            {
                if (!_eventListeners[i].TryGetValue(eventType, out var eventListener)) continue;
                if (eventListener.GetType() != typeof(Action<T>))
                {
                    LogManager.LogError("事件参数不一致", eventListener.GetType(), typeof(Action<T>));
                    continue;
                }
                Delegate[] list = eventListener.GetInvocationList();
                foreach (var function in list)
                {
                    object obj = ((Action<T>)function).Target;
                    if (obj is IMyDisposable disposable && disposable.IsDisposed())
                    {
                        LogManager.LogError("类已回收");
                        RemoveListener<T>(eventType, (Action<T>)function, (EventPriority)i);
                    }
                    else
                    {
                        ((Action<T>)function)(parameter);
                    }
                }
                        
                // ((Action<T>)eventListener)(parameter);
            }
        }
    
        public void TriggerEvent(Events eventType)
        {
            for (int i = 0; i < MAX_PRI; i++)
            {
                if (!_eventListeners[i].TryGetValue(eventType, out var eventListener)) continue;
                if (eventListener.GetType() != typeof(Action))
                {
                    LogManager.LogError("事件参数不一致", eventListener.GetType(), typeof(Action));
                    continue;
                }
                Delegate[] list = eventListener.GetInvocationList();
                foreach (var function in list)
                {
                    object obj = ((Action)function).Target;
                    if (obj is IMyDisposable disposable && disposable.IsDisposed())
                    {
                        LogManager.LogError("类已回收");
                        RemoveListener(eventType, (Action)function, (EventPriority)i);
                    }
                    else
                    {
                        ((Action)function)();
                    }
                }
                // ((Action)eventListener)();
            }
        }
    }
}
