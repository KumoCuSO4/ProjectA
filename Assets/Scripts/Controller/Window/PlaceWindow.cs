using Controller.Placeable;
using Event;
using Manager;
using Table;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.Window
{
    public class PlaceWindow : BaseWindow
    {
        private PlaceableTable _placeableTable = PlaceableTable.Get();
        private PlaceGridController _placeGrid;
        private bool _isPlacing = false;
        
        private BasePlaceable _placingPlaceable;
        private Transform _selectPart;
        private GridLayoutGroup _gridLayoutGroup;
        private Transform _item;
        
        public PlaceWindow(GameObject gameObject, string windowName, PlaceGridController placeGrid) : base(gameObject, windowName)
        {
            _placeGrid = placeGrid;
        }

        public override void InitWindow()
        {
            base.InitWindow();
            _selectPart = transform.Find("select_part");
            _gridLayoutGroup = _selectPart.Find("scroll/Viewport/Content").GetComponent<GridLayoutGroup>();
            var data = _placeableTable.GetAllData();
            _item = _gridLayoutGroup.transform.Find("item");
            foreach (var placeableData in data)
            {
                int id = placeableData.Key;
                string name = placeableData.Value.name;
                GameObject go = Utils.AddChild(_gridLayoutGroup.gameObject, _item.gameObject);
                go.name = id.ToString();
                go.transform.Find("name").GetComponent<Text>().text = name;
                go.GetComponent<Button>().onClick.AddListener(() =>
                {
                    _placingPlaceable = _placeGrid.CreatePlaceable(id);
                    _isPlacing = true;
                    _selectPart.gameObject.SetActive(false);
                });
                go.SetActive(true);
            }
            
            if (_placeGrid == null)
            {
                WindowManager.Get().CloseWindow(this);
                return;
            }
            _placeGrid.ShowGird();

            var mainWindow = (MainWindow)WindowManager.Get().GetWindow("main_window");
            mainWindow?.RefreshPlaceBtn();

            RegisterEvents();
        }

        private void OnClickItem()
        {
            _placingPlaceable = _placeGrid.CreatePlaceable(1);
        }
        
        private void RegisterEvents()
        {
            EventManager.Get().AddListener(Events.UPDATE, Update);
        }

        private void Update()
        {
            if (_isPlacing)
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
                        HandleClose();
                    }
                }
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                HandleClose();
            }
        }

        private void HandleClose()
        {
            if (_isPlacing)
            {
                _isPlacing = false;
                _selectPart.gameObject.SetActive(true);
            }
            else
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