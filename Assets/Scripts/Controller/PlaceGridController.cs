using System;
using System.Collections.Generic;
using Controller.Placeable;
using Table;
using UnityEditor.UI;
using UnityEngine;

namespace Controller
{
    public class PlaceGridController : BaseController
    {
        private int length = 20, width = 20; // length -> x  width -> y
        private Vector3 startPos = new Vector3(-1, 0, -1);
        private readonly int CELL_SIZE = 1;
        private GameObject _grid;
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        private Mesh _mesh;
        private List<BasePlaceable> _placeables = new List<BasePlaceable>();
        private List<BasePlaceable> _placing = new List<BasePlaceable>();
        private Dictionary<int, int> occupied = new Dictionary<int, int>();
        private int nextID = 100;
        
        public PlaceGridController(GameObject gameObject) : base(gameObject)
        {
            _grid = Utils.AddChild(base.gameObject, "grid");
            gameObject.transform.localPosition = startPos;
            _meshRenderer = _grid.AddComponent<MeshRenderer>();
            _meshFilter = _grid.AddComponent<MeshFilter>();
            _mesh = new Mesh();
            _meshFilter.mesh = _mesh;
            DrawGrid();
            HideGrid();
        }

        private void DrawGrid()
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            for (int i = 0; i <= length; i++)
            {
                for (int j = 0; j <= width; j++)
                {
                    vertices.Add(new Vector3(i * CELL_SIZE, 0, j * CELL_SIZE));
                    int curIndice = i * (width + 1) + j;
                    int leftIndice = curIndice - 1;
                    int topIndice = curIndice - width - 1;
                    if (i > 0)
                    {
                        indices.Add(topIndice);
                        indices.Add(curIndice);
                    }

                    if (j > 0)
                    {
                        indices.Add(leftIndice);
                        indices.Add(curIndice);
                    }
                }
            }
            _mesh.vertices = vertices.ToArray();
            _mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
            _meshRenderer.material = new Material(Shader.Find("Unlit"));
            _meshRenderer.material.color = Color.red;
        }

        public bool IsPositionInside(Vector3 position)
        {
            if (position.x >= startPos.x 
                && position.x <= startPos.x + length * CELL_SIZE 
                && position.z >= startPos.z 
                && position.z <= startPos.z + width * CELL_SIZE)
            {
                return true;
            }

            return false;
        }
        public bool IsPosXYInside(Vector2Int pos)
        {
            if (pos.x >= 0
                && pos.x < length
                && pos.y >= 0 
                && pos.y < width)
            {
                return true;
            }

            return false;
        }

        public void ShowGird()
        {
            _grid.SetActive(true);
        }

        public void HideGrid()
        {
            _grid.SetActive(false);
        }

        public BasePlaceable CreatePlaceable(int placeableID)
        {
            PlaceableData placeableData = PlaceableTable.Get().GetPlaceableData(placeableID);
            string className = placeableData.className;
            if (className == null)
            {
                className = "BasePlaceable";
            }
            Type type = Type.GetType("Controller.Placeable." + className);
            if (type == null)
            {
                LogManager.LogError("Placeable类不存在", className);
                return null;
            }
        
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Placeable/" + placeableData.prefabName);
            GameObject obj = Utils.AddChild(gameObject, prefab);
            object[] constructorArgs = { obj, placeableID, this };
            BasePlaceable placeable = Activator.CreateInstance(type, constructorArgs) as BasePlaceable;
            _placing.Add(placeable);
            return placeable;
        }

        public bool Place(BasePlaceable placeable)
        {
            if (placeable.GetPlaceGrid() != this)
            {
                return false;
            }
            if (placeable.status != BasePlaceable.Status.PLACING)
            {
                return false;
            }
            if (!CheckCanPlace(placeable))
            {
                return false;
            }

            _placing.Remove(placeable);
            _placeables.Add(placeable);
            int placeID = nextID;
            nextID++;
            placeable.SetPlaced(placeID);
            
            Vector2Int pos = placeable.posXY;
            for (int i = pos.x; i < pos.x + placeable.length; i++)
            {
                for (int j = pos.y; j < pos.y + placeable.width; j++)
                {
                    int index = GetXYToIndex(i, j);
                    occupied[index] = placeID;
                }
            }
            
            return true;
        }
        
        public bool PlaceCancel(BasePlaceable placeable)
        {
            _placing.Remove(placeable);
            placeable.Dispose();
            return true;
        }
        
        public Vector3 GetXYToOffset(Vector2Int posXY)
        {
            Vector3 offset = posXY.x * Vector3.right * CELL_SIZE + posXY.y * Vector3.forward * CELL_SIZE;
            return offset;
        }
        
        public Vector3 GetXYToOffset(Vector2 posXY)
        {
            Vector3 offset = posXY.x * Vector3.right * CELL_SIZE + posXY.y * Vector3.forward * CELL_SIZE;
            return offset;
        }
        
        public Vector3 GetXYToPos(Vector2Int posXY)
        {
            Vector3 pos = startPos + GetXYToOffset(posXY);
            return pos;
        }

        public Vector2Int GetPosToXY(Vector3 pos)
        {
            Vector3 offset = pos - startPos;
            int x = (int)(offset.x / CELL_SIZE);
            int y = (int)(offset.z / CELL_SIZE);
            return new Vector2Int(x, y);
        }
        
        private int GetXYToIndex(int x, int y)
        {
            return x * width + y;
        }

        public bool CheckCanPlace(BasePlaceable placeable)
        {
            Vector2Int pos = placeable.posXY;
            for (int i = pos.x; i < pos.x + placeable.length; i++)
            {
                for (int j = pos.y; j < pos.y + placeable.width; j++)
                {
                    Vector2Int posXY = new Vector2Int(i, j);
                    if (CheckPosOccupied(posXY))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckPosOccupied(Vector2Int posXY)
        {
            if (!IsPosXYInside(posXY))
            {
                return true;
            }
            int index = GetXYToIndex(posXY.x, posXY.y);
            if (occupied.TryGetValue(index, out var value))
            {
                if (value == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
    }
}