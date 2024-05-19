using UnityEngine;
using UnityEngine.UI;

namespace Controller.Window
{
    public class ConnectWindow : BaseWindow
    {
        private Button hostBtn;
        private Button clientBtn;
        
        public ConnectWindow(GameObject gameObject, string windowName) : base(gameObject, windowName)
        {
        }

        public override void InitWindow()
        {
            base.InitWindow();
            hostBtn = transform.Find("host_btn").GetComponent<Button>();
            clientBtn = transform.Find("client_btn").GetComponent<Button>();
            hostBtn.onClick.AddListener(() =>
            {
                ServerManager.Get().StartHost();
                WindowManager.Get().OpenWindow("main_window");
                WindowManager.Get().CloseWindow(this);
            });
            clientBtn.onClick.AddListener(() =>
            {
                ServerManager.Get().StartClient();
                WindowManager.Get().OpenWindow("main_window");
                WindowManager.Get().CloseWindow(this);
            });
        }
    }
}