using UnityEngine;

namespace Controller.Window
{
    // 控制一个窗口
    public abstract class BaseWindow : BaseController
    {
        private string windowName;
        
        public BaseWindow(Transform transform, string windowName) : base(transform)
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