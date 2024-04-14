using System;
using Cinemachine;
using Event;
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
        private int _id = -1;
        private bool init = false;
        private bool isLocalPlayer = false;
        private PlayerNetworkBehavior _playerNetworkBehavior;
        
        public PlayerController(Transform transform) : base(transform)
        {
            _rb = transform.GetComponent<Rigidbody>();
            _playerNetworkBehavior = base.transform.GetComponent<PlayerNetworkBehavior>();
            _playerNetworkBehavior.SetPlayerController(this);
        }

        public void OnPlayerSpawn(bool isLocalPlayer)
        {
            if (init) return;
            init = true;
            this.isLocalPlayer = isLocalPlayer;
            if (isLocalPlayer)
            {
                _virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
                _virtualCamera.Follow = transform;
                _virtualCamera.LookAt = transform;
                EventManager.Get().AddListener(Events.UPDATE, Update);
                _playerManager.AddLocalPlayer(this);
            }
            else
            {
                _playerManager.AddOtherPlayer(this);
            }
        }

        private void Update()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
            _rb.velocity = movement * moveSpeed;
        }

        public void SetID(int id)
        {
            _id = id;
        }

        public int GetID()
        {
            return _id;
        }

        public void SetName(string name)
        {
            transform.name = name;
        }
    }
}