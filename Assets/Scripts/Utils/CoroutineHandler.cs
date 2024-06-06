using System;
using System.Collections;
using Manager;
using UnityEngine;

public class CoroutineHandler : MonoBehaviour
{
    private void Start()
    {
        Coroutines.CoroutineHandler = this;
    }

    public Coroutine WaitForSeconds(float time, Action action)
    {
        return StartCoroutine(IEWaitForSeconds(time, action));
    }
    
    IEnumerator IEWaitForSeconds(float time, Action action)
    {
        float leftTime = time;
        while (leftTime > 0)
        {
            leftTime -= TimeManager.Get().GetDeltaTime();
            yield return null;
        }
        action();
    }
}