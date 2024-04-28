using UnityEngine;

namespace Controller.Window
{
    // 控制一个窗口
    public abstract class BaseWindow : BaseController
    {
        private string windowName;
        
        public BaseWindow(GameObject gameObject, string windowName) : base(gameObject)
        {
            this.windowName = windowName;
            InitWindow();
        }

        public string GetWindowName()
        {
            return windowName;
        }

        protected virtual void InitWindow()
        {
            
        }
    }
}