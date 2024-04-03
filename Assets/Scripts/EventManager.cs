using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EventManager : Singleton<EventManager>
{
    public delegate void Func(params object[] values);
    private readonly Dictionary<Events, List<Func>> _eventListeners = new Dictionary<Events, List<Func>>();
    
    public EventManager()
    {
        Debug.Log("EventManager created");
        foreach (Events eventType in Enum.GetValues(typeof(Events)))
        {
            _eventListeners[eventType] = new List<Func>();
        }
    }
    
    public void AddListener(Events eventType, Func func)
    {
        _eventListeners[eventType].Add(func);
    }
    
    public void RemoveListener(Events eventType, Func func)
    {
        _eventListeners[eventType].Remove(func);
    }
    
    public void DispatchEvent(Events eventType, params object[] values)
    {
        foreach (var func in _eventListeners[eventType])
        {
            func(values);
        }
    }
}
