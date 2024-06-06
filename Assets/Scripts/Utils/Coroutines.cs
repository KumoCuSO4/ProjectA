
using System;
using UnityEngine;

public static class Coroutines
{
    public static CoroutineHandler CoroutineHandler;

    public static void StopCoroutine(Coroutine coroutine)
    {
        CoroutineHandler.StopCoroutine(coroutine);
    }
    
    public static Coroutine WaitForSeconds(float time, Action action)
    {
        if (CoroutineHandler == null)
        {
            LogManager.LogError("CoroutineHandler is null");
            return null;
        }
        return CoroutineHandler.WaitForSeconds(time, action);
    }
}