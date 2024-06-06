using System.Collections;
using Event;
using Manager;
using UnityEngine;
using UnityEngine.AI;

namespace Controller.NPC
{
    public class CustomerController : BaseController
    {
        private Vector3 entrancePos = new Vector3(0, 0.5f, 0);
        private Vector3 shelfPos = new Vector3(5, 0.5f, 5);
        private Vector3 checkoutPos = new Vector3(1, 0.5f, 1);
        private float shoppingTime = 3f;
        private float payingTime = 2f;
        private float moveSpeed = 2f;
        private NavMeshAgent agent;

        public enum CustomerStatus
        {
            NONE,
            GO_SHOP,
            SHOPPING,
            GO_PAY,
            PAYING,
            LEAVING,
        }

        private CustomerStatus _status = CustomerStatus.NONE;
        
        public CustomerController(GameObject gameObject) : base(gameObject)
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            agent.stoppingDistance = 1f;
            agent.speed = moveSpeed * TimeManager.Get().timeScale;
            
            Init();
        }

        private void Init()
        {
            transform.position = entrancePos;
            _status = CustomerStatus.GO_SHOP;
            agent.SetDestination(shelfPos);
            
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            EventManager.Get().AddListener(Events.UPDATE, Update);
            EventManager.Get().AddListener(Events.TIME_SCALE_CHANGE, OnTimeScaleChange);
        }

        private void OnTimeScaleChange()
        {
            agent.speed = moveSpeed * TimeManager.Get().timeScale;
        }
        
        private void Update()
        {
            if (_status == CustomerStatus.GO_SHOP || _status == CustomerStatus.GO_PAY || _status == CustomerStatus.LEAVING)
            {
                // 检查是否到达目标位置
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        if (_status == CustomerStatus.GO_SHOP)
                        {
                            _status = CustomerStatus.SHOPPING;
                            Coroutines.WaitForSeconds(shoppingTime, FinishShopping);
                        }
                        else if (_status == CustomerStatus.GO_PAY)
                        {
                            _status = CustomerStatus.PAYING;
                            Coroutines.WaitForSeconds(payingTime, FinishPaying);
                        }
                        else if (_status == CustomerStatus.LEAVING)
                        {
                            _status = CustomerStatus.NONE;
                            LeaveShop();
                        }
                    }
                }
            }
        }
        
        private void FinishShopping()
        {
            // LogManager.Log("FinishShopping");
            _status = CustomerStatus.GO_PAY;
            agent.SetDestination(checkoutPos);
        }
        
        private void FinishPaying()
        {
            // LogManager.Log("FinishPaying");
            _status = CustomerStatus.LEAVING;
            agent.SetDestination(entrancePos);
        }

        private void LeaveShop()
        {
            Dispose();
        }
    }
}