using System.Threading.Tasks;
using UnityEngine;
using VRIF_URP.Player;
using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP.Pipes
{
    public class PlayerMovementController : ITickable
    {
        private readonly PlayerInputController _playerInputController;
        private readonly PlayerView _playerView;
        private readonly PlayerInputConfig _playerInputConfig;

        private bool _isRotatable = true;

        public PlayerMovementController(
            TickableManager tickableManager,
            PlayerInputController playerInputController,
            SceneHolder sceneHolder,
            PlayerInputConfig playerInputConfig)
        {
            _playerInputController = playerInputController;
            _playerView = sceneHolder.Get<PlayerView>();
            _playerInputConfig = playerInputConfig;
            tickableManager.Add(this);
        }
        
        public void Tick()
        {
            MoveXZDirection();
        }

        private void MoveXZDirection()
        {
            var axis = _playerInputController.GetLeftThumbstickControllerInput();
            
            var playerView = _playerView.gameObject;
            var rawDirection = (playerView.transform.right * axis.x) + (playerView.transform.forward * axis.y);
            
            _playerView.CharacterController.Move(rawDirection * Time.deltaTime);
        }

        private void MoveRotation()
        {
            var axis = _playerInputController.GetLeftThumbstickControllerInput();
            
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
        
        private async void ModiferTimer()
        {
            _isRotatable = false;
            await Task.Delay(_playerInputConfig.DelayPerRotate);
            _isRotatable = true;
        }
    }
}