using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class GameEntry : MonoBehaviour
{
    // Start is called before the first frame update
    private EventManager eventManager;
    void Start()
    {
        eventManager = EventManager.Get();
        eventManager.AddListener(Events.UPDATE, Hello);
        eventManager.DispatchEvent(Events.UPDATE, 1, 2, "name", new MyClass());
    }

    private void Update()
    {
        //eventManager.DispatchEvent(Events.UPDATE, 1);
    }

    void Hello(params object[] values)
    {
        Debug.Log("Hello");
        foreach (var value in values)
        {
            Debug.Log(value + " " + value.GetType());
        }
    }
}

class MyClass
{
    public override string ToString()
    {
        return "MyClass";
    }
}
