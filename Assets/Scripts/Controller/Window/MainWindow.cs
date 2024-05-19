using Event;
using Org.BouncyCastle.Bcpg;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.Window
{
    public class MainWindow : BaseWindow
    {
        private Button placeBtn;
        
        public MainWindow(GameObject gameObject, string windowName) : base(gameObject, windowName)
        {
        }

        public override void InitWindow()
        {
            base.InitWindow();
            placeBtn = transform.Find("place_btn").GetComponent<Button>();
            // placeBtn.onClick.AddListener(() =>
            // {
            //     WindowManager.Get().OpenWindow("place_window");
            //     RefreshPlaceBtn();
            // });
            RefreshPlaceBtn();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            EventManager.Get().AddListener(Events.ON_PLACE_GRID_CHANGE, RefreshPlaceBtn);
        }

        public void RefreshPlaceBtn()
        {
            bool active = true;
            var placeWindow = WindowManager.Get().GetWindow("place_window");
            if (placeWindow != null)
            {
                active = false;
            }
            var curPlaceGrid = Utils.GetLocalPlayer()?.GetCurPlaceGrid();
            if (curPlaceGrid == null)
            {
                active = false;
            }
            placeBtn.gameObject.SetActive(active);
        }
    }
}