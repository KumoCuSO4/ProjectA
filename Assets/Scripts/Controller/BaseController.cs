using System;
using UnityEditor.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Controller
{
    public abstract class BaseController : IDisposable
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

        public void Dispose()
        {
            Object.Destroy(gameObject);
        }
    }
}