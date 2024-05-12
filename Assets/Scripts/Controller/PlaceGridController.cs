using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class PlaceGridController : BaseController
    {
        private int length = 20, width = 20;
        private readonly int CELL_SIZE = 1;
        private MeshRenderer _meshRenderer;
        private MeshFilter _meshFilter;
        private Mesh _mesh;
        
        public PlaceGridController(GameObject gameObject) : base(gameObject)
        {
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _mesh = new Mesh();
            _meshFilter.mesh = _mesh;
            DrawGrid();
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
        
    }
}