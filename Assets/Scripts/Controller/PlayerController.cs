﻿using System;
using Cinemachine;
using Controller.Item;
using Event;
using JetBrains.Annotations;
using Manager;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
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

        private float aimDistance = 5f;
        private GameObject aimTarget;
        private BaseController aimTargetController;
        private BaseItem carryItem;

        public PlayerController(GameObject gameObject) : base(gameObject)
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
            #region Move

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical);
            if (direction.magnitude > 0.2f)
            {
                Vector3 movement = direction * moveSpeed * Time.deltaTime;
                Vector3 localPosition;
                Quaternion localRotation;
                transform.GetLocalPositionAndRotation(out localPosition, out localRotation);
                localRotation.SetLookRotation(direction);
                localPosition = localPosition + movement;
                transform.SetLocalPositionAndRotation(localPosition, localRotation);
            }
            
            #endregion
            

            if (Input.GetKeyDown(KeyCode.C))
            {
                SetName(Random.Range(0, 100).ToString());
            }
            
            if (Input.GetKeyDown(KeyCode.X))
            {
                ItemManager.Get().CreateItem(1);
            }
            
            _canvas.transform.LookAt(_virtualCamera.transform);

            if (carryItem == null)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;
    
                if (Physics.Raycast(ray, out hit, aimDistance))
                {
                    GameObject go = hit.collider.gameObject;
                    AimTarget(go);
                }
                else
                {
                    UnAimTarget();
                }
            }
            else
            {
                carryItem.GetTransform().position = transform.position + transform.forward;
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (carryItem == null && aimTargetController != null && aimTargetController is BaseItem item)
                {
                    if (item.Carry(this))
                    {
                        carryItem = item;
                        UnAimTarget();
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (carryItem != null && carryItem.Drop(this))
                {
                    carryItem = null;
                }
            }
        }

        private void AimTarget(GameObject go)
        {
            if (aimTarget != go)
            {
                if (aimTarget != null)
                {
                    Outline outline1 = aimTarget.GetComponent<Outline>();
                    if (outline1 == null)
                    {
                        outline1 = aimTarget.AddComponent<Outline>();
                        outline1.OutlineColor = Color.red;
                    }
                    outline1.enabled = false;
                }

                if (go != null)
                {
                    Outline outline2 = go.GetComponent<Outline>();
                    if (outline2 == null)
                    {
                        outline2 = go.AddComponent<Outline>();
                        outline2.OutlineColor = Color.red;
                    }
                    outline2.enabled = true;
                }

                aimTarget = go;
                aimTargetController = ControllerManager.Get().GetController(go);
            }
        }

        private void UnAimTarget()
        {
            if (aimTarget != null)
            {
                Outline outline1 = aimTarget.GetComponent<Outline>();
                if (outline1 == null)
                {
                    outline1 = aimTarget.AddComponent<Outline>();
                    outline1.OutlineColor = Color.red;
                }
                outline1.enabled = false;
                
                aimTarget = null;
                aimTargetController = null;
            }
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