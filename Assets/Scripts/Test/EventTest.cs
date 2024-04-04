using UnityEngine;
using Utils;

namespace Test
{
    public class EventTest
    {
        private EventManager _eventManager;
        
        public EventTest()
        {
            _eventManager = EventManager.Get();
            EventManager.Func f = values => Test((int)values[0], (string)values[1]);
            _eventManager.AddInnerListener(Events.TEST, f);
            _eventManager.DispatchEvent(Events.TEST, 123, "aaa");
        }
        
        private void Test(int a, string b)
        {
            LogManager.Log(a);
            LogManager.Log(b);
        }
    }
}