using System.Threading.Tasks;
using DG.Tweening;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using VRIF_URP.Player;
using VRIF_URP.Room;
using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP.Pipes
{
    public class PipeMovementController : ITickable
    {
        private readonly PipeService _pipeService;
        private readonly VectorDirectionController _vectorDirectionController;
        private readonly PlayerInputController _playerInputController;
        private readonly PipeConfig _pipeConfig;

        private const float RotationAnimationDuration = 0.2f;

        private PlayerView _playerView;
        
        private GameObject _currentPipeObject;
        private Direction _currentPipeDirection;
        private GameObject _tempGO;

        private float _currentDistanceFromPlayer = 2f;
        private bool _isRotatable = true;
        private float _angle;

        private bool _stateRayCastInput = true;
        
        public PipeMovementController(
            TickableManager tickableManager,
            PipeService pipeService,
            SceneHolder sceneHolder,
            VectorDirectionController vectorDirectionController,
            PlayerInputController playerInputController)
        {
            _pipeService = pipeService;
            _vectorDirectionController = vectorDirectionController;
            _playerInputController = playerInputController;
            _playerView = sceneHolder.Get<PlayerView>();

            _playerInputController.SetActiveRayCastInput(_stateRayCastInput);
            
            tickableManager.Add(this); }
        
        public void Tick()
        {
            if (_stateRayCastInput)
            {
                _tempGO = _playerInputController.GetRayCastInput();
                if (_tempGO.tag == "Copy" && _playerInputController.GetSecondaryIndexTrigger())
                {
                    var prefab = _pipeService.Spawn(_tempGO.GetComponent<CanvasRoomPipeElement>().ID);
                    _currentPipeObject = prefab.gameObject;
                    _currentPipeDirection = prefab.GetPipeDirection();
                    
                    _stateRayCastInput = false;
                
                    _playerInputController.SetActiveRayCastInput(_stateRayCastInput);
                }
            }
            else
            {
                StandartMovment();
            }
        }

        private void StandartMovment()
        {
            if (_playerInputController.GetPrimaryIndexTrigger())
            {
                _stateRayCastInput = true;
                _playerInputController.SetActiveRayCastInput(_stateRayCastInput);
                Object.Destroy(_currentPipeObject);
                return;
            }
            
            var currentAxisRightThumbStick = _playerInputController.GetRightThumbstickControllerInput();
            
            var grabButtonIsPressed = _playerInputController.GetGripButtonRightControllerInput();

            RotationMovement(currentAxisRightThumbStick, grabButtonIsPressed);

            if (!TryVisualizePrefab())
            {
                _currentPipeObject.transform.position =
                    _playerView.RightHand.transform.position + _playerView.RightHand.transform.forward * _currentDistanceFromPlayer;
            }

            if (grabButtonIsPressed)
            {
                return;
            }
            
            ForwardBackMovement(currentAxisRightThumbStick);
        }

        private bool TryVisualizePrefab()
        {
            var list = _pipeService.GetEmptyPlace();
            
            var theLowestDistance = 100f;
            var index = -1;
            
            for (int i = 0; i < list.Count; i++)
            {
                var distance = Vector3.Distance(_playerView.RightHand.transform.position + 
                                                _playerView.RightHand.transform.forward * 
                                                _currentDistanceFromPlayer, 
                                            list[i].transform.position);

                if (distance > 1f)
                {
                    break;
                }
                
                if ( distance < theLowestDistance)
                {
                    theLowestDistance = distance;
                    index = i;
                }
            }
            
            if (theLowestDistance != 100f)
            {
                if (_currentPipeDirection == list[index].GetPipePlaceDirection )
                {
                    _currentPipeObject.transform.position = list[index].transform.position;
                    return true;
                }
            }
            return false;
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
                        switch (_currentPipeDirection)
                        {
                            case Direction.Up:
                                _currentPipeDirection = Direction.Left;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Up, Direction.Left);
                                break;
                            case Direction.Left:
                                _currentPipeDirection = Direction.Down;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Left, Direction.Down);
                                break;
                            case Direction.Down:
                                _currentPipeDirection = Direction.Right;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Down, Direction.Right);
                                break;
                            case Direction.Right:
                                _currentPipeDirection = Direction.Up;
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
                        switch (_currentPipeDirection)
                        {
                            case Direction.Up:
                                _currentPipeDirection = Direction.Right;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Up, Direction.Right);
                                break;
                            case Direction.Right:
                                _currentPipeDirection = Direction.Down;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Right, Direction.Down);
                                break;
                            case Direction.Down:
                                _currentPipeDirection = Direction.Left;
                                _angle =  _vectorDirectionController.GetAngle(Direction.Down, Direction.Left);
                                break;
                            case Direction.Left:
                                _currentPipeDirection = Direction.Up;
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