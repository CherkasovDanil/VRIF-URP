using System.Collections.Generic;
using System.Linq;
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

        private Dictionary<int, PipeView> _pipeStorage = new Dictionary<int, PipeView>();
        private List<PipeView> _pipeStorageList = new List<PipeView>();
        private Dictionary<int, Transform> _connectionPlaceDictionary = new Dictionary<int, Transform>();
        
        private RoomView _roomView;
        private Color _defaultColor;
        private MeshRenderer _meshRenderer;
        
        private bool _animationIsRunning;
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
        }

        public List<PipeConnectionPlace> GetEmptyPlace()
        {
            return _roomView.GetEmptyPlace();
        }

        public GameObject TrySpawnPipe(GameObject currentPipeObject, PipeDirection currentPipeDirection)
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
                _angle =  _vectorDirectionController.GetAngle(currentPipeDirection, list[index].GetPipePlacePipeDirection);
               
                currentPipeObject
                    .transform
                    .DORotate(new Vector3(_angle,0,0) , 0)
                    .SetEase(Ease.Linear); 
                
                   
                if (_roomView.GetEmptyPlace().Count == 0)
                {
                    return null;
                }
                    
                return Spawn(GetLastSpawnedPipeView().GetPipeViewID()).gameObject;
            }

            ErrorAnimation(currentPipeObject);

            return null;
        }
        

        public PipeView Spawn(int id)
        {
            var model = _config.Get(id);

            var prefab = _instantiator.InstantiatePrefabForComponent<PipeView>(model.Value.Prafab);

            _pipeStorage.Add(prefab.gameObject.GetInstanceID(), prefab);
            _pipeStorageList.Add(prefab);

            return prefab;
        }

        public PipeView GetLastSpawnedPipeView()
        {
            return _pipeStorageList.Last();
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