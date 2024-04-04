using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GameEntry : MonoBehaviour
{
    // Start is called before the first frame update
    private EventManager _eventManager;
    private FpsCounter _fpsCounter;
    void Start()
    {
        _eventManager = EventManager.Get();
        
        //_fpsCounter = new FpsCounter();
    }

    private void Update()
    {
        _eventManager.DispatchEvent(Events.UPDATE);
    }
}