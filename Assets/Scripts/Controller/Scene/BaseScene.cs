using Controller.Item;
using UnityEngine;

namespace Controller.Scene
{
    public class BaseScene : BaseController
    {
        private ScenePlaceGrid _scenePlaceGrid;
        private SceneItem _sceneItem;
        public BaseScene(GameObject gameObject) : base(gameObject)
        {
            GameObject ground = Resources.Load<GameObject>("Prefabs/ground");
            GameObject goGround = Utils.AddChild(gameObject, ground);
            goGround.transform.localPosition = new Vector3(0, -0.5f, 0);
            GameObject go1 = Utils.AddChild(base.gameObject, "scene_place_grid");
            _scenePlaceGrid = new ScenePlaceGrid(go1);
            GameObject go2 = Utils.AddChild(base.gameObject, "scene_item");
            _sceneItem = new SceneItem(go2);
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
    }
}