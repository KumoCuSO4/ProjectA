using System.Collections.Generic;
using Controller.Item;
using Controller.Placeable;
using Event;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace Controller.Scene
{
    public class BaseScene : BaseController
    {
        private ScenePlaceGrid _scenePlaceGrid;
        private SceneItem _sceneItem;
        private SceneCustomer _sceneCustomer;
        
        private GameObject ground;
        public BaseScene(GameObject gameObject) : base(gameObject)
        {
            ground = Resources.Load<GameObject>("Prefabs/ground");
            GameObject goGround = Utils.AddChild(gameObject, ground);
            goGround.transform.localPosition = new Vector3(0, -0.5f, 0);
            GameObject go1 = Utils.AddChild(base.gameObject, "scene_place_grid");
            _scenePlaceGrid = new ScenePlaceGrid(go1);
            GameObject go2 = Utils.AddChild(base.gameObject, "scene_item");
            _sceneItem = new SceneItem(go2);

            NavMeshData navMeshData = GenerateNavMeshData();
            NavMesh.AddNavMeshData(navMeshData);
            GameObject go3 = Utils.AddChild(base.gameObject, "scene_customer");
            _sceneCustomer = new SceneCustomer(go3);
        }
        
        NavMeshData GenerateNavMeshData()
        {
            NavMeshBuildSettings settings = new NavMeshBuildSettings
            {
                agentRadius = 0.5f,
                agentHeight = 2.0f,
                agentSlope = 45.0f,
                agentClimb = 0.1f,
                agentTypeID = 0, // Default agent type
                overrideTileSize = true,
                tileSize = 10
            };

            List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
            NavMeshBuildSource source = new NavMeshBuildSource();
            source.transform = ground.transform.localToWorldMatrix;
            source.shape = NavMeshBuildSourceShape.Box;
            source.size = new Vector3(100.0f, 0.1f, 100.0f);
            sources.Add(source);

            Bounds bounds = new Bounds(Vector3.zero, new Vector3(100f, 50f, 100f));
            return NavMeshBuilder.BuildNavMeshData(settings, sources, bounds, Vector3.zero, Quaternion.identity);
        }

        public void RefreshNavMesh()
        {
            
        }

        public void Exit()
        {
            Dispose();
        }

        public override void Dispose()
        {
            _scenePlaceGrid.Dispose();
            _sceneItem.Dispose();
            base.Dispose();
        }

        public BaseItem CreateItem(int itemID)
        {
            return _sceneItem.CreateItem(itemID);
        }

        public ScenePlaceGrid GetScenePlaceGrid()
        {
            return _scenePlaceGrid;
        }
    }
}