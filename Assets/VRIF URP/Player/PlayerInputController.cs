using UnityEngine;
using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP.Player
{
    public class PlayerInputController : ITickable
    {
        private const float RayLineLenght = 5;

        private PlayerView _playerView;

        private bool _isRotatable = true;
        private bool _raycastInput;

        private float _moveX;
        private float _moveZ;

        private GameObject _tempGameObject = new GameObject();        

        public PlayerInputController(
            TickableManager tickableManager,
            SceneHolder sceneHolder)
        {
            _playerView = sceneHolder.Get<PlayerView>();
            
            tickableManager.Add(this);
        }

        public void Tick()
        {
            GetLeftThumbstickControllerInput();

            GetRightThumbstickControllerInput();

            if (_raycastInput)
            {
                RayCastInput();
            }
        }

        public Vector2 GetRightThumbstickControllerInput()
        {
            return OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        }

        public Vector2 GetLeftThumbstickControllerInput()
        {
            return OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        }

        public bool GetGripButtonRightControllerInput()
        {
            return OVRInput.Get(OVRInput.RawButton.RHandTrigger);
        }
        
        public bool GetPrimaryIndexTrigger()
        {
            return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
        }

        public bool GetSecondaryIndexTrigger()
        {
            return OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
        }

        public void SetActiveRayCastInput(bool state)
        {
            _raycastInput = state;
        }

        public GameObject GetRayCastInput()
        {
            if (_raycastInput)
            {
                return RayCastInput();
            }

            return _tempGameObject;
        }

        private GameObject RayCastInput()
        {
            var ray = new Ray(
                _playerView.RightHand.transform.position,
                _playerView.RightHand.transform.TransformDirection(Vector3.forward * RayLineLenght));

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject!=null)
                {
                   return hitInfo.collider.gameObject;
                }
            }

            return _tempGameObject;
        }
    }
}