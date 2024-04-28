using System;
using Manager;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Controller
{
    // 控制一个物体
    public abstract class BaseController : BaseBehavior
    {
        protected Transform transform;
        protected GameObject gameObject;
        protected ControllerManager _controllerManager;
        
        public BaseController(GameObject gameObject)
        {
            _controllerManager = ControllerManager.Get();
            this.gameObject = gameObject;
            this.transform = this.gameObject.transform;
            _controllerManager.SetController(gameObject, this);
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public override void Dispose()
        {
            base.Dispose();
            Object.Destroy(gameObject);
            transform = null;
            gameObject = null;
        }
    }
}