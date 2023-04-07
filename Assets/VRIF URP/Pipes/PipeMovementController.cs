using System.Collections.Generic;
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
        private readonly PipeService _pipeService;
        private readonly VectorDirectionController _vectorDirectionController;
        private readonly PlayerInputController _playerInputController;
        private readonly PipeConfig _pipeConfig;

        private const float RotationAnimationDuration = 0.2f;

        private PlayerView _playerView;
        
        private PipeView _currentPipeView;
        private GameObject _tempGO;
        private List<PipeConnectionPlace> _list;

        private float _currentDistanceFromPlayer = 2f;
        private bool _isRotatable = true;
        private float _angle;
        private PipeDirection _currentPipeDirection;

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
            
            _list = _pipeService.GetEmptyPlace();
            
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
            var theLowestDistance = 100f;
            var index = -1;
            
            for (int i = 0; i < _list.Count; i++)
            {
                var distance = Vector3.Distance(_playerView.RightHand.transform.position + 
                                                _playerView.RightHand.transform.forward * 
                                                _currentDistanceFromPlayer, 
                    _list[i].transform.position);

                if (distance > 0.7f)
                {
                    continue;
                }
                
                if ( distance < theLowestDistance)
                {
                    theLowestDistance = distance;
                    index = i;
                }
            }
            
            if (theLowestDistance != 100f)
            {
                if (_currentPipeView.PipeDirection == _list[index].GetPipePlacePipeDirection )
                {
                    _currentPipeView.transform.position = _list[index].transform.position;
                    
                    if (_playerInputController.GetSecondaryIndexTrigger())
                    {
                        var indexDist = 0;
                        if (_currentPipeView.PipeConnectionObject.GetEmptyPlaceFromObject().Count != 0)
                        {
                         
                            foreach (var place in _currentPipeView.PipeConnectionObject.GetEmptyPlaceFromObject())
                            {
                                var distance = 100f;
                                var tempDistance = Vector3.Distance(_list[index].transform.position,
                                    place.gameObject.transform.position);
                                if (distance < tempDistance)
                                {
                                    distance = tempDistance;
                                    indexDist = index;
                                }
                            }
                            
                            _currentPipeView.PipeConnectionObject.GetEmptyPlaceFromObject()[indexDist].SetStateBlockPipePlacesForConnection(true);
                        }

                        _list[index].SetStateBlockPipePlacesForConnection(true);

                        _pipeService.AnimatopLastSpawnedPipeView(Color.green);
                        
                        _list = _pipeService.GetEmptyPlace();
                        
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
                                _currentPipeDirection = PipeDirection.Left;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Up, PipeDirection.Left);
                                break;
                            case PipeDirection.Left:
                                _currentPipeDirection = PipeDirection.Down;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Left, PipeDirection.Down);
                                break;
                            case PipeDirection.Down:
                                _currentPipeDirection = PipeDirection.Right;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Down, PipeDirection.Right);
                                break;
                            case PipeDirection.Right:
                                _currentPipeDirection = PipeDirection.Up;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Right, PipeDirection.Up);
                                break;
                        }
                    } 
                    else if (axis.y < -0.8f)
                    {
                        switch (_currentPipeView.PipeDirection)
                        {
                            case PipeDirection.Up:
                                _currentPipeDirection = PipeDirection.Right;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Up, PipeDirection.Right);
                                break;
                            case PipeDirection.Right:
                                _currentPipeDirection = PipeDirection.Down;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Right, PipeDirection.Down);
                                break;
                            case PipeDirection.Down:
                                _currentPipeDirection = PipeDirection.Left;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Down, PipeDirection.Left);
                                break;
                            case PipeDirection.Left:
                                _currentPipeDirection = PipeDirection.Up;
                                _angle =  _vectorDirectionController.GetAngle(PipeDirection.Left, PipeDirection.Up);
                                break;
                        }
                    }

                    if (axis.y > 0.8f || axis.y < -0.8f)
                    {
                        _currentPipeView.PipeDirection = _currentPipeDirection;
                        
                        _currentPipeView
                            .transform
                            .DORotate(new Vector3(_angle,0,0) , RotationAnimationDuration)
                            .SetEase(Ease.Linear);
                    
                        if (_currentPipeView.GetPipeType() == PipeType.Line)
                        {
                            foreach (var pipeConnectionPlace in _currentPipeView.PipeConnectionObject.GetEmptyPlaceFromObject())
                            {
                                pipeConnectionPlace.SetPipeConnectionDirection(_currentPipeDirection);
                            }
                        }
                        
                        ModiferTimer();
                    }
                }
                /*else
                {
                if (grabButtonIsPressed)
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
                }*/
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