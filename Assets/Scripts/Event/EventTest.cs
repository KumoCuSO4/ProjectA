using System;
using Event;
using UnityEngine;
using Utils;

namespace Event
{
    public class EventTest : IMyDisposable
    {
        private EventManager _eventManager;
        private int num = 100;
        private bool isDisposed = false;
        
        public EventTest()
        {
            _eventManager = EventManager.Get();
            _eventManager.AddListener<int>(Events.TEST, Test);
            _eventManager.AddListener<int>(Events.TEST, Test);
            _eventManager.AddListener<string>(Events.TEST, Test);
            _eventManager.TriggerEvent(Events.TEST, 123);
            num = 200;
            _eventManager.TriggerEvent(Events.TEST, 234);
        }

        ~EventTest()
        {
            ReleaseUnmanagedResources();
        }

        private void AddListener<T>(Events eventType, Action<T> function)
        {
            if (_eventManager.AddListener<T>(eventType, function))
            {
                
            }
        }
        
        public void Test(int a)
        {
            LogManager.LogError(a, num);
        }
        
        private void Test(string a)
        {
            LogManager.LogError(a);
        }

        private void ReleaseUnmanagedResources()
        {
            LogManager.LogError("Dispose");
            isDisposed = true;
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        public bool IsDisposed()
        {
            return isDisposed;
        }
    }
}