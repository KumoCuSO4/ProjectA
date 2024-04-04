using System;
using Cinemachine;
using Mirror;
using UnityEngine;

namespace Controller
{
    public class PlayerController : NetworkBehaviour
    {
        public float moveSpeed = 5f;
        
        private Rigidbody _rb;
        private CinemachineVirtualCamera _virtualCamera;
        
        public override void OnStartLocalPlayer()
        {
            _virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
            _virtualCamera.Follow = transform;
            _virtualCamera.LookAt = transform;
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
            _rb.velocity = movement * moveSpeed;
        }
        
    }
}