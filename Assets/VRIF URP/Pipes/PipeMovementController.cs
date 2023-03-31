using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VRIF_URP.Player;
using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP.Pipes
{
    public class PipeMovementController : ITickable
    {
        private readonly PipeSpawner _pipeSpawner;
        private readonly PlayerInputController _playerInputController;

        private GameObject _currentPipeObject;
        private PlayerView _playerView;

        private float _currentDistanceFromPlayer = 2f;
        private bool _isRotatable = true;
        
        public PipeMovementController(
            TickableManager tickableManager,
            PipeSpawner pipeSpawner,
            SceneHolder sceneHolder,
            PlayerInputController playerInputController)
        {
            _pipeSpawner = pipeSpawner;
            _playerInputController = playerInputController;
            _playerView = sceneHolder.Get<PlayerView>();

            tickableManager.Add(this);
        }
        
        public void Tick()
        {
            if (_currentPipeObject == null)
            {
                _currentPipeObject = _pipeSpawner.SpawnPipe().gameObject;
            }

            var currentAxisRightThumbStick = _playerInputController.GetRightThumbstickControllerInput();
            
            var grabButtonIsPressed = _playerInputController.GetGripButtonRightControllerInput();

            RotationMovement(currentAxisRightThumbStick, grabButtonIsPressed);
            
            _currentPipeObject.transform.position =
                _playerView.RightHand.transform.position + _playerView.RightHand.transform.forward * _currentDistanceFromPlayer;
            
            if (grabButtonIsPressed)
            {
                return;
            }
            
            ForwardBackMovement(currentAxisRightThumbStick);
        }

        private void ForwardBackMovement(Vector2 axis)
        {
            if (axis.y > 0.8f)
            {
                _currentDistanceFromPlayer += 0.025f;

                if (_currentDistanceFromPlayer > 5)
                {
                    _currentDistanceFromPlayer = 5;
                }
            } 
            else if (axis.y < -0.8f)
            {
                _currentDistanceFromPlayer -= 0.025f;
               
                if (_currentDistanceFromPlayer < 0.5f)
                {
                    _currentDistanceFromPlayer = 0.5f;
                }
            }
        }

        private void RotationMovement(Vector2 axis, bool grabButtonIsPressed)
        {
            if (_isRotatable)
            {
                if (grabButtonIsPressed)
                {
                    if (axis.y > 0.8f)
                    {
                        var rt = _currentPipeObject.transform.rotation.eulerAngles;
                        Debug.Log("current "+ rt);
                        var p = new Vector3(rt.x += 90f, rt.y, rt.z);
                        Debug.Log(" will add" + p);
                        _currentPipeObject.transform.DORotate(p, 0f);
                        Debug.Log("end " + rt);
                        ModiferTimer();
                    } 
                    else if (axis.y < -0.8f)
                    {
                        var rt = _currentPipeObject.transform.rotation.eulerAngles;
                        _currentPipeObject.transform.DORotate( rt -= new Vector3(90f,rt.y,rt.z), 0.2f);
                        ModiferTimer();
                    }
                }
                else
                {
                    if (axis.x > 0.8f)
                    {
                        var rt = _currentPipeObject.transform.rotation.eulerAngles;
                        _currentPipeObject.transform.DORotate( rt += new Vector3(0,90f,0), 0.2f);
                        ModiferTimer();
                    } 
                    else if (axis.x < -0.8f)
                    {
                        var rt = _currentPipeObject.transform.rotation.eulerAngles;
                        _currentPipeObject.transform.DORotate( rt += new Vector3(0,-90f,0), 0.2f);
                        ModiferTimer();
                    } 
                }
            }
        }
        
        private async void ModiferTimer()
        {
            _isRotatable = false;
            await Task.Delay(500);
            _isRotatable = true;
        }
    }
}