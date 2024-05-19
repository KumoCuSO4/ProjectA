using Controller.Placeable;
using Event;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.Window
{
    public class PlaceWindow : BaseWindow
    {
        private PlaceGridController _placeGrid;
        private BasePlaceable _placingPlaceable;
        
        public PlaceWindow(GameObject gameObject, string windowName, PlaceGridController placeGrid) : base(gameObject, windowName)
        {
            _placeGrid = placeGrid;
        }

        public override void InitWindow()
        {
            base.InitWindow();
            
            if (_placeGrid == null)
            {
                WindowManager.Get().CloseWindow(this);
                return;
            }
            _placeGrid.ShowGird();
            _placingPlaceable = _placeGrid.CreatePlaceable(1);
            
            var mainWindow = (MainWindow)WindowManager.Get().GetWindow("main_window");
            mainWindow?.RefreshPlaceBtn();

            RegisterEvents();
        }
        
        private void RegisterEvents()
        {
            EventManager.Get().AddListener(Events.UPDATE, Update);
        }

        private void Update()
        {
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 100, 1 << (int)Const.Layer.GROUND))
                {
                    //Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
                    _placingPlaceable.SetPosition(hit.point);
                }
                
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                _placingPlaceable.RotateClockwise();
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (_placingPlaceable.Place())
                {
                    _placingPlaceable = null;
                    WindowManager.Get().CloseWindow(this);
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                WindowManager.Get().CloseWindow(this);
            }
        }

        public override void WillClose()
        {
            base.WillClose();
            _placeGrid?.HideGrid();
            if (_placingPlaceable != null)
            {
                _placeGrid?.PlaceCancel(_placingPlaceable);
            }
        }
    }
}