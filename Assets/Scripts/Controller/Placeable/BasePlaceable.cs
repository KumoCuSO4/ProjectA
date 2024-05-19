using System;
using System.Collections.Generic;
using System.ComponentModel;
using Table;
using UnityEditor.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Controller.Placeable
{
    public class BasePlaceable : BaseController
    {
        public int length { protected set; get; }
        public int width { protected set; get; }  // length -> x  width -> y
        
        public enum Direction
        {
            EAST = 0,
            SOUTH,
            WEST,
            NORTH,
        }

        private Direction _direction;
        public Direction direction
        {
            protected set
            {
                if (!Enum.IsDefined(typeof(Direction), value))
                    throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(Direction));
                if(_direction != value)
                {
                    _direction = value;
                    OnRotated();
                }
            }
            get => _direction;
        } // east -> x+  north -> z+

        public enum Status
        {
            PLACING,
            NORMAL,
        }

        public Status status
        {
            protected set;
            get;
        }
        private PlaceGridController _placeGrid;
        private Vector2Int _posXY = Vector2Int.zero;
        public Vector2Int posXY
        {
            protected set
            {
                if (value.x != _posXY.x || value.y != _posXY.y)
                {
                    _posXY = value;
                    OnPositionChange();
                }
            }
            get
            {
                return _posXY;
            }
        }

        private Vector2 _centerOffsetV, _centerOffsetH;

        private Vector2 centerOffset
        {
            set
            {
                _centerOffsetH = value;
                _centerOffsetV = new Vector2(value.y, value.x);
            }
            get
            {
                if (direction is Direction.EAST or Direction.WEST)
                {
                    return _centerOffsetH;
                }
                else
                {
                    return _centerOffsetV;
                }
            }
        }
        private int _tableID;

        public int id
        {
            protected set;
            get;
        }
        private Collider _collider;
        private Dictionary<int, GameObject> indicators = new Dictionary<int, GameObject>();
        private Dictionary<int, Material> indicatorMaterials = new Dictionary<int, Material>();
        private GameObject indicatorsGo;
        
        public BasePlaceable(GameObject gameObject, int tableID, PlaceGridController placeGrid) : base(gameObject)
        {
            this.direction = direction;
            this._placeGrid = placeGrid;
            this._tableID = tableID;
            PlaceableData placeableData = PlaceableTable.Get().GetPlaceableData(tableID);
            this.length = placeableData.length;
            this.width = placeableData.width;
            centerOffset = new Vector2((float)length / 2 - 0.5f, (float)width / 2 - 0.5f);
            status = Status.PLACING;
            _collider = base.transform.Find("model").GetComponent<Collider>();
            _collider.enabled = false;
            
            AddIndicators();
            OnPositionChange();
        }

        private void AddIndicators()
        {
            GameObject indicatorPrefab = Resources.Load<GameObject>("Prefabs/Common/indicator");
            indicatorsGo = Utils.AddChild(gameObject, "indicators");
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    GameObject go = Utils.AddChild(indicatorsGo, indicatorPrefab);
                    go.name = $"indicator_{i}_{j}";
                    Vector3 offset = _placeGrid.GetXYToOffset(new Vector2(i - centerOffset.x, j - centerOffset.y));
                    offset.y = 0.1f;
                    go.transform.localPosition = offset;
                    indicators[GetXYToIndex(i, j)] = go;
                    indicatorMaterials[GetXYToIndex(i, j)] =
                        go.transform.Find("model").GetComponent<MeshRenderer>().material;
                }
            }

            RefreshIndicators();
        }

        private void RefreshIndicators()
        {
            if (status != Status.PLACING)
            {
                return;
            }

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    GameObject go = indicators[GetXYToIndex(i, j)];
                    Material material = indicatorMaterials[GetXYToIndex(i, j)];
                    if (!_placeGrid.CheckPosOccupied(posXY + GetXYRotated(i, j)))
                    {
                        material.color = Color.green;
                    }
                    else
                    {
                        material.color = Color.red;
                    }
                }
            }
        }

        private void RemoveIndicators()
        {
            Object.Destroy(indicatorsGo);
        }
        private Vector2Int GetXYRotated(int x, int y)
        {
            if (direction == Direction.EAST)
            {
                return new Vector2Int(x, y);
            }
            else if(direction == Direction.SOUTH)
            {
                return new Vector2Int(y, length - x - 1);
            }
            else if (direction == Direction.WEST)
            {
                return new Vector2Int(length - x - 1, width - y - 1);
            }
            else
            {
                return new Vector2Int(width - y - 1, x);
            }
        }
        private int GetXYToIndex(int x, int y)
        {
            return x * width + y;
        }
        private Vector2Int GetIndexToXY(int index)
        {
            return new Vector2Int(index / width, index % width);
        }
        public void RotateClockwise()
        {
            direction = (Direction)(((int)direction + 1) % 4);
        }
        
        public void RotateAnticlockwise()
        {
            direction = (Direction)(((int)direction - 1 + 4) % 4);
        }

        private void OnRotated()
        {
            transform.localRotation = Quaternion.Euler(0, (int)direction * 90, 0);
            OnPositionChange();
        }

        public void SetPosition(Vector3 pos)
        {
            if (status == Status.PLACING)
            {
                this.posXY = GetPosToXY(pos);
            }
        }

        public Vector2Int GetPosToXY(Vector3 pos)
        {
            pos -= _placeGrid.GetXYToOffset(centerOffset);
            return _placeGrid.GetPosToXY(pos);
        }
        
        private void OnPositionChange()
        {
            // LogManager.Log(posXY);
            transform.localPosition = _placeGrid.GetXYToOffset(posXY + centerOffset + Vector2.one * 0.5f);
            RefreshIndicators();
        }

        public bool Place()
        {
            return _placeGrid.Place(this);
        }

        public PlaceGridController GetPlaceGrid()
        {
            return _placeGrid;
        }

        public void SetPlaced(int placeID)
        {
            status = Status.NORMAL;
            _collider.enabled = true;
            id = placeID;
            RemoveIndicators();
        }
        
        
    }
}