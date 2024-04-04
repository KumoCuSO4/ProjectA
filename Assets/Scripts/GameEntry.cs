using System;
using System.Collections;
using System.Collections.Generic;
using Test;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public class GameEntry : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerManager _playerManager;
    private EventManager _eventManager;
    private FpsCounter _fpsCounter;
    
    void Start()
    {
        _playerManager = PlayerManager.Get();
        _eventManager = EventManager.Get();
        //_fpsCounter = new FpsCounter();
        
        // EventTest eventTest = new EventTest();
        
        _eventManager.DispatchEvent(Events.START);
        LogManager.Log("Game start");
    }

    private void Update()
    {
        //_eventManager.DispatchEvent(Events.UPDATE);
    }

    private static GameEntry _instance;
    public static GameEntry Get()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<GameEntry>();
        }
        return _instance;
    }
}