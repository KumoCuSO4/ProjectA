using System;
using Cinemachine;
using Mirror;
using UnityEngine;
using Utils;

namespace Controller
{
    public class PlayerController : BaseController
    {
        public float moveSpeed = 5f;
        
        private Rigidbody _rb;
        private CinemachineVirtualCamera _virtualCamera;
        private PlayerManager _playerManager = PlayerManager.Get();
        private int _index = -1;
        
        public PlayerController(Transform transform, bool isLocalPlayer) : base(transform)
        {
            _rb = transform.GetComponent<Rigidbody>();
            if (isLocalPlayer)
            {
                _virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
                _virtualCamera.Follow = transform;
                _virtualCamera.LookAt = transform;
                EventManager.Get().AddOuterListener(Events.UPDATE, Update);
            }
            else
            {
                
            }
        }

        private void Update(params object[] values)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
            _rb.velocity = movement * moveSpeed;
        }

        public void SetIndex(int index)
        {
            _index = index;
        }

        public void SetName(string name)
        {
            transform.name = name;
        }
    }
}