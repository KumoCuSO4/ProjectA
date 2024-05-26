using Cinemachine;
using Event;
using UnityEngine;

namespace Player
{
    public class PlayerCamera : BaseBehavior
    {
        private PlayerController _playerController;
        private CinemachineVirtualCamera _virtualCamera;
        private CinemachineTransposer _transposer;
        
        
        private float _yaw = 0f; // 偏航角，绕Y轴旋转
        private float _pitch = Mathf.PI / 4; // 俯仰角，绕X轴旋转
        private float _minPitch = 0f;
        private float _maxPitch = Mathf.PI / 2;
        
        private float _followDistance = 20;
        private float _minFollowDistance = 5;
        private float _maxFollowDistance = 20;

        public Vector3 followOffset
        {
            get
            {
                float x = Mathf.Cos(_yaw) * Mathf.Cos(_pitch);
                float y = Mathf.Sin(_pitch);
                float z = Mathf.Sin(_yaw) * Mathf.Cos(_pitch);

                return new Vector3(x, y, z) * _followDistance;
            }
        }
        
        public float zoomSpeed = 2.0f;
        public float rotateSpeed = 0.1f;
        
        public PlayerCamera(PlayerController playerController)
        {
            this._playerController = playerController;
            _virtualCamera = Object.FindObjectOfType<CinemachineVirtualCamera>();
            _virtualCamera.Follow = _playerController.GetTransform();
            _virtualCamera.LookAt = _playerController.GetTransform();
            _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            _transposer.m_FollowOffset = followOffset;
            EventManager.Get().AddListener(Events.UPDATE, Update);
        }

        private void Update()
        {
            bool changed = false;

            if (Input.GetMouseButton(2))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                if (mouseX != 0f)
                {
                    changed = true;
                    _yaw += -mouseX * rotateSpeed;
                }

                if (mouseY != 0f)
                {
                    changed = true;
                    _pitch += -mouseY * rotateSpeed;
                    if (_pitch < _minPitch) _pitch = _minPitch;
                    if (_pitch > _maxPitch) _pitch = _maxPitch;
                }
            }
            
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                changed = true;
                _followDistance += -scroll * zoomSpeed;
                if (_followDistance < _minFollowDistance) _followDistance = _minFollowDistance;
                if (_followDistance > _maxFollowDistance) _followDistance = _maxFollowDistance;
            }

            if (changed)
            {
                _transposer.m_FollowOffset = followOffset;
            }
        }
        
        public float GetRotationY()
        {
            return _virtualCamera.transform.rotation.eulerAngles.y;
        }
    }
}