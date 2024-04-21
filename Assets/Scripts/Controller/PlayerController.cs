using System;
using Cinemachine;
using Event;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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
        private string _playerName;
        private Canvas _canvas;
        private Text _nameText;
        
        public PlayerController(Transform transform) : base(transform)
        {
            _rb = transform.GetComponent<Rigidbody>();
            _playerNetworkBehavior = base.transform.GetComponent<PlayerNetworkBehavior>();
            _playerNetworkBehavior.SetPlayerController(this);
            _canvas = base.transform.Find("Canvas").GetComponent<Canvas>();
            _nameText = base.transform.Find("Canvas/name").GetComponent<Text>();
        }

        public void OnPlayerSpawn(bool isLocalPlayer)
        {
            if (init) return;
            init = true;
            this.isLocalPlayer = isLocalPlayer;
            _virtualCamera = Object.FindObjectOfType<CinemachineVirtualCamera>();
            if (isLocalPlayer)
            {
                _virtualCamera.Follow = transform;
                _virtualCamera.LookAt = transform;
                EventManager.Get().AddListener(Events.UPDATE, LocalPlayerUpdate);
                _playerManager.AddLocalPlayer(this);
            }
            else
            {
                EventManager.Get().AddListener(Events.UPDATE, OtherPlayerUpdate);
                _playerManager.AddOtherPlayer(this);
            }
        }

        private void LocalPlayerUpdate()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
            _rb.velocity = movement * moveSpeed;

            if (Input.GetKeyDown(KeyCode.C))
            {
                SetName(Random.Range(0, 100).ToString());
            }
            _canvas.transform.LookAt(_virtualCamera.transform);
        }

        private void OtherPlayerUpdate()
        {
            _canvas.transform.LookAt(_virtualCamera.transform);
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
            _playerNetworkBehavior.CmdChangeName(name);
        }
        
        [Server]
        public void ServerSetupPlayer(string name)
        {
            _playerNetworkBehavior.ServerSetupPlayer(name);
        }

        public void OnPlayerNameChanged(string oldStr, string newStr)
        {
            _playerName = newStr;
            transform.name = _playerName;
            _nameText.text = _playerName;
        }
    }
}