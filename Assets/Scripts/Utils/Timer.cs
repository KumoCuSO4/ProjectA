
using System;
using Event;
using Manager;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Timer : BaseBehavior
{
    private float endTime;
    private Action _action;
    private float _time;
    private bool _isLoop;
    private bool _isRealTime;
    private bool _isStarted = false;
    private Coroutine _coroutine = null;
    
    public Timer(float time, Action action, bool isLoop, bool isRealTime = false)
    {
        _action = action;
        _time = time;
        _isLoop = isLoop;
        _isRealTime = isRealTime;
    }

    public void Start()
    {
        if (_isStarted)
        {
            return;
        }

        _isStarted = true;
        _StartTimer();
    }
    
    public void Stop()
    {
        if (!_isStarted)
        {
            return;
        }

        if (_coroutine != null)
        {
            Coroutines.StopCoroutine(_coroutine);
        }
        
        _isStarted = false;
    }

    private void _StartTimer()
    {
        _coroutine = Coroutines.WaitForSeconds(_time, () =>
        {
            _action();
            if (_isLoop)
            {
                _StartTimer();
            }
            else
            {
                _isStarted = false;
                _coroutine = null;
            }
        });
    }
}