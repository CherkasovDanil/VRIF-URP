using System.Threading.Tasks;
using UnityEngine;
using VRIF_URP.Pipes;
using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP.Player
{
    public class PlayerMovementController : ITickable
    {
        private readonly PlayerInputConfig _playerInputConfig;
        private readonly PipeSpawner _pipeSpawner;

        private const string InteractableTag = "CanInteractable";
        private const float RayLineLenght = 5;

        private PlayerView _playerView;
        
        private bool _isRotatable = true;
        
        private float _moveX;
        private float _moveZ;

        public PlayerMovementController(
            TickableManager tickableManager,
            PlayerInputConfig playerInputConfig,
            SceneHolder sceneHolder,
            PipeSpawner pipeSpawner)
        {
            _playerInputConfig = playerInputConfig;
            _pipeSpawner = pipeSpawner;

            _playerView = sceneHolder.Get<PlayerView>();
            
            tickableManager.Add(this);
        }

        public void Tick()
        {
            GetControllerInput();
            
            RayCastInput();

            DirectionModifier();
        }

        private void RayCastInput()
        {
            var ray = new Ray(
                _playerView.RightHand.transform.position,
                _playerView.RightHand.transform.TransformDirection(Vector3.forward * RayLineLenght));

            Debug.DrawRay(_playerView.RightHand.transform.position,
                _playerView.RightHand.transform.TransformDirection(Vector3.forward * RayLineLenght));

            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && Physics.Raycast(ray, out var hit, RayLineLenght))
            {
                if (hit.collider.CompareTag(InteractableTag))
                {
                    _pipeSpawner.SpawnPipe(hit.transform);
                }
            }
        }

        private void DirectionModifier()
        {
            var axis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
          
            if (_isRotatable)
            {
                if (axis.x > _playerInputConfig.TopPrimaryThumbstickRotateLim)
                {
                    _playerView.gameObject.transform.Rotate(0f,_playerInputConfig.RotateModifier,0f);
                    ModiferTimer();
                } 
                else if (axis.x < _playerInputConfig.LowPrimaryThumbstickRotateLim)
                {
                    _playerView.gameObject.transform.Rotate(0f,-_playerInputConfig.RotateModifier,0f);
                    ModiferTimer();
                }
            }
        }

        private void GetControllerInput()
        {
            var axis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

            var playerView = _playerView.gameObject;
            var rawDirection = (playerView.transform.right * axis.x) + (playerView.transform.forward * axis.y);
            
            _playerView.CharacterController.Move(rawDirection * Time.deltaTime);
        }
        
        private async void ModiferTimer()
        {
            _isRotatable = false;
            await Task.Delay(_playerInputConfig.DelayPerRotate);
            _isRotatable = true;
        }
    }
}