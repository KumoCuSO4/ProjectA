using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EventManager : Singleton<EventManager>
{
    public delegate void Func(params object[] values);
    private readonly Dictionary<Events, List<Func>> _eventListenersInner = new Dictionary<Events, List<Func>>();
    private readonly Dictionary<Events, List<Func>> _eventListenersOuter = new Dictionary<Events, List<Func>>();
    public EventManager()
    {
        LogManager.Log("EventManager created");
        foreach (Events eventType in Enum.GetValues(typeof(Events)))
        {
            _eventListenersInner[eventType] = new List<Func>();
            _eventListenersOuter[eventType] = new List<Func>();
        }
    }
    
    public void AddInnerListener(Events eventType, Func func)
    {
        _eventListenersInner[eventType].Add(func);
    }
    
    public void AddOuterListener(Events eventType, Func func)
    {
        _eventListenersOuter[eventType].Add(func);
    }
    
    public void RemoveInnerListener(Events eventType, Func func)
    {
        _eventListenersInner[eventType].Remove(func);
    }
    
    public void RemoveOuterListener(Events eventType, Func func)
    {
        _eventListenersOuter[eventType].Remove(func);
    }
    
    public void DispatchEvent(Events eventType, params object[] values)
    {
        foreach (var func in _eventListenersInner[eventType])
        {
            func(values);
        }
        
        foreach (var func in _eventListenersOuter[eventType])
        {
            func(values);
        }
    }
}
