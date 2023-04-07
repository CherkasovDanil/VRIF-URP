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
        
        private PipeView _currentPipeView;
        private GameObject _tempGO;

        private float _currentDistanceFromPlayer = 2f;
        private bool _isRotatable = true;
        private float _angle;

        private bool _stateRayCastInput = true;

        private int _lastPipeID;
        
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
                    _lastPipeID = _tempGO.GetComponent<CanvasRoomPipeElement>().ID;
                    
                    var prefab = _pipeService.Spawn(_lastPipeID);
                    
                    _currentPipeView = prefab;
                  
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
                Object.Destroy(_currentPipeView);
                return;
            }
            
            var currentAxisRightThumbStick = _playerInputController.GetRightThumbstickControllerInput();
            
            var grabButtonIsPressed = _playerInputController.GetGripButtonRightControllerInput();

            RotationMovement(currentAxisRightThumbStick, grabButtonIsPressed);
            
            if (grabButtonIsPressed)
            {
                return;
            }
            
            ForwardBackMovement(currentAxisRightThumbStick);


            if (!TryVisualizePrefab())
            {
                _currentPipeView.transform.position =
                    _playerView.RightHand.transform.position + _playerView.RightHand.transform.forward * _currentDistanceFromPlayer;
                
                if (_playerInputController.GetSecondaryIndexTrigger())
                {
                    _pipeService.AnimatopLastSpawnedPipeView(Color.red);
                }
            }
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

                if (distance > 0.7f)
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
                if (_currentPipeView.PipeDirection == list[index].GetPipePlacePipeDirection )
                {
                    _currentPipeView.transform.position = list[index].transform.position;
                    
                    if (_playerInputController.GetSecondaryIndexTrigger())
                    {
                        _pipeService.AnimatopLastSpawnedPipeView(Color.green);
                        
                        _currentPipeView =  _pipeService.Spawn(_lastPipeID);
                        _currentPipeView.transform.localScale = Vector3.zero;
                        _currentPipeView.transform.DOScale(Vector3.one, 0.7f);
                        _currentDistanceFromPlayer = 0.6f;
                    }
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
                        switch (_currentPipeView.PipeDirection)
                        {
                            case PipeDirection.Up:
                                _currentPipeView.PipeDirection = PipeDirection.Left;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Up, PipeDirection.Left);
                                break;
                            case PipeDirection.Left:
                                _currentPipeView.PipeDirection = PipeDirection.Down;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Left, PipeDirection.Down);
                                break;
                            case PipeDirection.Down:
                                _currentPipeView.PipeDirection = PipeDirection.Right;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Down, PipeDirection.Right);
                                break;
                            case PipeDirection.Right:
                                _currentPipeView.PipeDirection = PipeDirection.Up;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Right, PipeDirection.Up);
                                break;
                        }
                        
                        _currentPipeView
                            .transform
                            .DORotate(new Vector3(_angle,0,0) , RotationAnimationDuration)
                            .SetEase(Ease.Linear); 
                        
                        ModiferTimer();
                    } 
                    else if (axis.y < -0.8f)
                    {
                        switch (_currentPipeView.PipeDirection)
                        {
                            case PipeDirection.Up:
                                _currentPipeView.PipeDirection = PipeDirection.Right;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Up, PipeDirection.Right);
                                break;
                            case PipeDirection.Right:
                                _currentPipeView.PipeDirection = PipeDirection.Down;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Right, PipeDirection.Down);
                                break;
                            case PipeDirection.Down:
                                _currentPipeView.PipeDirection = PipeDirection.Left;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Down, PipeDirection.Left);
                                break;
                            case PipeDirection.Left:
                                _currentPipeView.PipeDirection = PipeDirection.Up;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Left, PipeDirection.Up);
                                break;
                        }
                        
                        _currentPipeView
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
                        rt = _currentPipeView.transform.rotation.eulerAngles + new Vector3(0,90f,0);
                        _currentPipeView.transform.DORotate( rt, 0.2f);
                        ModiferTimer();
                    } 
                    else if (axis.x < -0.8f)
                    {
                        rt = _currentPipeView.transform.rotation.eulerAngles + new Vector3(0,-90f,0);
                        _currentPipeView.transform.DORotate( rt, 0.2f);
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