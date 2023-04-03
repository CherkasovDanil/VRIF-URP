using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using VRIF_URP.Player;
using VRIF_URP.Room;
using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP.Pipes
{
    public class PipeService
    {
        private readonly PipeConfig _config;
        private readonly IInstantiator _instantiator;
        private readonly VectorDirectionController _vectorDirectionController;

        private Dictionary<int, GameObject> _pipeStorage = new Dictionary<int, GameObject>();
        private Dictionary<int, Transform> _connectionPlaceDictionary = new Dictionary<int, Transform>();
        
        private RoomView _roomView;
        private Color _defaultColor;
        private MeshRenderer _meshRenderer;
        
        private bool _animationIsRunning;
        private int _maxId;
        private int id;

        public PipeService(
            PipeConfig pipeConfig,
            IInstantiator instantiator,
            SceneHolder sceneHolder,
            VectorDirectionController vectorDirectionController)
        {
            _config = pipeConfig;
            _instantiator = instantiator;
            _vectorDirectionController = vectorDirectionController;

            _roomView = sceneHolder.Get<RoomView>();

            _maxId = _config.GetPrefabsCount();
        }

        [CanBeNull]
        public GameObject TrySpawnPipe(GameObject currentPipeObject, Direction _currentDirection)
        {
            var list = _roomView.GetEmptyPlace();
            
            var theLowestDistance = 100f;
            var index = -1;
            
            for (int i = 0; i < list.Count; i++)
            {
                var distance = Vector3.Distance(currentPipeObject.transform.position, list[i].transform.position);

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

            var _angle = 0f;
            
            if (theLowestDistance != 100f)
            {
                currentPipeObject.transform.position = list[index].transform.position;
                _angle =  _vectorDirectionController.GetAngle(_currentDirection, list[index].GetPipePlaceDirection);
               
                currentPipeObject
                    .transform
                    .DORotate(new Vector3(_angle,0,0) , 0)
                    .SetEase(Ease.Linear); 
                
                   
                if (_roomView.GetEmptyPlace().Count == 0)
                {
                    return null;
                }
                    
                return Spawn();
            }

            ErrorAnimation(currentPipeObject);

            return null;
        }

        public GameObject Spawn()
        {
            var model = _config.Get(id);
            
            id++;
            if (id >= _maxId)
            {
                id = 0;
            }

            var prefab = _instantiator.InstantiatePrefab(model.Value.Prafab);

            _pipeStorage.Add(prefab.gameObject.GetInstanceID(), prefab);

            return prefab;
        }

        private void ErrorAnimation(GameObject gameObject)
        {
            //TODO Remove get component
            gameObject.GetComponent<MeshRenderer>().material.DOColor(Color.red, .5f)
                .OnComplete(() =>
                    {
                        gameObject.GetComponent<MeshRenderer>().material.DOColor(Color.white, .5f);
                    });
        }
    }
}