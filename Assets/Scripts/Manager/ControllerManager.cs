using System.Collections.Generic;
using Controller;
using UnityEngine;
using Utils;

namespace Manager
{
    public class ControllerManager : Singleton<ControllerManager>
    {
        private Dictionary<GameObject, BaseController> _controllers = new Dictionary<GameObject, BaseController>();

        public void SetController(GameObject gameObject, BaseController controller)
        {
            _controllers[gameObject] = controller;
        }
        
        public BaseController GetController(GameObject gameObject)
        {
            return _controllers.TryGetValue(gameObject, out var controller) ? controller : null;
        }
    }
}