using Event;
using UnityEngine;

namespace Utils
{
    public class FpsCounter
    {
        private int _frame = 0;
        private float _frameRate = 0f;
        private float _deltaTime = 0f;
        private float _updateDisplayTime = 0.3f;
        
        public FpsCounter()
        {
            EventManager.Get().AddListener(Events.UPDATE, Update);
        }

        public void Update()
        {
            _frame++;
            _deltaTime += Time.deltaTime;
            if (_deltaTime >= _updateDisplayTime)
            {
                _frameRate = _frame / _deltaTime;
                LogManager.Log(_frameRate + " " + _frame + " " + _deltaTime);
                _frame = 0;
                _deltaTime = 0;
            }
        }
    }
}