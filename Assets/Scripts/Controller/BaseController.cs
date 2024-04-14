using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Controller
{
    // 控制一个物体
    public abstract class BaseController : BaseBehavior
    {
        protected Transform transform;
        protected GameObject gameObject;
        
        public BaseController(Transform transform)
        {
            this.transform = transform;
            gameObject = this.transform.gameObject;
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