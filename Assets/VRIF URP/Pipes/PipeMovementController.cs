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
        private readonly VectorDirectionController _vectorDirectionController;
        private readonly PlayerInputController _playerInputController;
        private readonly PipeConfig _pipeConfig;

        private const float RotationAnimationDuration = 0.2f;

        private GameObject _currentPipeObject;
        private PlayerView _playerView;
        
        private Direction _currentDirection = Direction.Up;
        private Direction _targetDirection;

        private float _currentDistanceFromPlayer = 2f;
        private bool _isRotatable = true;
        private float _angle;
        
        public PipeMovementController(
            TickableManager tickableManager,
            PipeSpawner pipeSpawner,
            SceneHolder sceneHolder,
            VectorDirectionController vectorDirectionController,
            PlayerInputController playerInputController)
        {
            _pipeSpawner = pipeSpawner;
            _vectorDirectionController = vectorDirectionController;
            _playerInputController = playerInputController;
            _playerView = sceneHolder.Get<PlayerView>();

            tickableManager.Add(this);
        }
        
        public void Tick()
        {
            if (_currentPipeObject == null)
            {
                _currentPipeObject = _pipeSpawner.SpawnPipe();
            }

            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                _currentPipeObject = null;
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
                        switch (_currentDirection)
                        {
                            case Direction.Up:
                                _currentDirection = Direction.Left;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Up, Direction.Left);
                                break;
                            case Direction.Left:
                                _currentDirection = Direction.Down;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Left, Direction.Down);
                                break;
                            case Direction.Down:
                                _currentDirection = Direction.Right;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Down, Direction.Right);
                                break;
                            case Direction.Right:
                                _currentDirection = Direction.Up;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Right, Direction.Up);
                                break;
                        }
                        
                        _currentPipeObject
                            .transform
                            .DORotate(new Vector3(_angle,0,0) , RotationAnimationDuration)
                            .SetEase(Ease.Linear); 
                        
                        ModiferTimer();
                    } 
                    else if (axis.y < -0.8f)
                    {
                        switch (_currentDirection)
                        {
                            case Direction.Up:
                                _currentDirection = Direction.Right;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Up, Direction.Right);
                                break;
                            case Direction.Right:
                                _currentDirection = Direction.Down;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Right, Direction.Down);
                                break;
                            case Direction.Down:
                                _currentDirection = Direction.Left;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Down, Direction.Left);
                                break;
                            case Direction.Left:
                                _currentDirection = Direction.Up;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Left, Direction.Up);
                                break;
                        }
                        
                        _currentPipeObject
                            .transform
                            .DORotate(new Vector3(_angle,0,0) , RotationAnimationDuration)
                            .SetEase(Ease.Linear); 
                        
                        ModiferTimer();
                    }
                }
                else
                {
                    var rt = Vector3.zero;
                    if (axis.x > 0.8f)
                    {
                        rt = _currentPipeObject.transform.rotation.eulerAngles + new Vector3(0,90f,0);
                        _currentPipeObject.transform.DORotate( rt, 0.2f);
                        ModiferTimer();
                    } 
                    else if (axis.x < -0.8f)
                    {
                        rt = _currentPipeObject.transform.rotation.eulerAngles + new Vector3(0,-90f,0);
                        _currentPipeObject.transform.DORotate( rt, 0.2f);
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