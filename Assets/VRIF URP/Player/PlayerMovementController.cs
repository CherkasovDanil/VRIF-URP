using System.Threading.Tasks;
using UnityEngine;
using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP.Player
{
    public class PlayerMovementController : ITickable
    {
        private readonly PlayerInputConfig _playerInputConfig;
       
        private PlayerView _playerView;
        
        private bool _isRotatable = true;
        
        private float _moveX;
        private float _moveZ;

        public PlayerMovementController(
            TickableManager tickableManager,
            PlayerInputConfig playerInputConfig,
            SceneHolder sceneHolder)
        {
            _playerInputConfig = playerInputConfig;

            _playerView = sceneHolder.Get<PlayerView>();
            
            tickableManager.Add(this);
        }

        public void Tick()
        {
            GetControllerInput();
            
            DirectionModifier();
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
           
            var rawDirection = (_playerView.gameObject.transform.right * axis.x) + (_playerView.gameObject.transform.forward * axis.y);
            
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