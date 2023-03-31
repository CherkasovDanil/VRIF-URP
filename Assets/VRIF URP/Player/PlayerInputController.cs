using System.Threading.Tasks;
using UnityEngine;
using VRIF_URP.Pipes;
using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP.Player
{
    public class PlayerInputController : ITickable
    {
        private const float RayLineLenght = 5;

        private PlayerView _playerView;

        private bool _isRotatable = true;

        private float _moveX;
        private float _moveZ;

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

            RayCastInput();
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

        private void RayCastInput()
        {
            var ray = new Ray(
                _playerView.RightHand.transform.position,
                _playerView.RightHand.transform.TransformDirection(Vector3.forward * RayLineLenght));

            Debug.DrawRay(_playerView.RightHand.transform.position,
                _playerView.RightHand.transform.TransformDirection(Vector3.forward * RayLineLenght));
        }
    }
}