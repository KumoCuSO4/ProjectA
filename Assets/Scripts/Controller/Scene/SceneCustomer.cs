using System;
using System.Collections.Generic;
using Controller.Item;
using Controller.NPC;
using Table;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Controller.Scene
{
    public class SceneCustomer : BaseController
    {
        private List<CustomerController> _customers = new List<CustomerController>();
        public SceneCustomer(GameObject gameObject) : base(gameObject)
        {
            Timer timer = new Timer(3, () => { CreateCustomer(); }, true);
            timer.Start();
        }
        
        public CustomerController CreateCustomer()
        {
            GameObject customerPrefab = Resources.Load<GameObject>("Prefabs/customer");
            GameObject obj = Utils.AddChild(gameObject, customerPrefab);
            obj.transform.localPosition = new Vector3(0, 0.5f, 0);
            CustomerController customerController = new CustomerController(obj);
            _customers.Add(customerController);
            return customerController;
        }

        public override void Dispose()
        {
            foreach (var customerController in _customers)
            {
                customerController.Dispose();
            }
            base.Dispose();
        }
    }
}