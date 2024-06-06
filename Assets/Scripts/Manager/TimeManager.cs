using System;
using Event;
using Time = UnityEngine.Time;

namespace Manager
{
    public class TimeManager : Singleton<TimeManager>
    {
        public float timeScale = 1.0f;
        public float curTime { get; private set; }
        public float curRealTime { get; private set; }
        
        public TimeManager()
        {
            EventManager.Get().AddListener(Events.FIXED_UPDATE, FixedUpdate);
        }

        public void SetTimeScale(float timeScale)
        {
            if (Math.Abs(this.timeScale - timeScale) > Const.FLOAT_TOLERANCE)
            {
                this.timeScale = timeScale;
                EventManager.Get().TriggerEvent(Events.TIME_SCALE_CHANGE);
            }
        }

        public float GetDeltaTime()
        {
            return Time.deltaTime * timeScale;
        }

        public float GetRealDeltaTime()
        {
            return Time.deltaTime;
        }

        public void WaitForSeconds(float time, Action function)
        {
            
        }

        private void FixedUpdate()
        {
            curRealTime += Time.fixedDeltaTime;
            curTime += Time.fixedDeltaTime * timeScale;
        }
    }
}