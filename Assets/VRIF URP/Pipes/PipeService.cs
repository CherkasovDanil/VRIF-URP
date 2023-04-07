using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using VRIF_URP.Room;
using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP.Pipes
{
    public class PipeService
    {
        private readonly PipeConfig _config;
        private readonly IInstantiator _instantiator;

        private Dictionary<int, PipeView> _pipeStorage = new Dictionary<int, PipeView>();
        private List<PipeView> _pipeStorageList = new List<PipeView>();
        
        private RoomView _roomView;
        private Color _defaultColor;
        private MeshRenderer _meshRenderer;
        
        private bool _animationIsRunning;
        private int id;

        public PipeService(
            PipeConfig pipeConfig,
            IInstantiator instantiator,
            SceneHolder sceneHolder)
        {
            _config = pipeConfig;
            _instantiator = instantiator;

            _roomView = sceneHolder.Get<RoomView>();
        }

        public List<PipeConnectionPlace> GetEmptyPlace()
        {
            var list = new List<PipeConnectionPlace>();
            
            foreach (var roomConnectionObject in _roomView.AllPipeConnectionObject)
            {
                foreach (var place in roomConnectionObject.GetEmptyPlaceFromObject())
                {
                    list.Add(place); 
                }
            }

            if (_pipeStorage.Count > 0)
            {
                foreach (var pipeElement in _pipeStorageList)
                {
                    foreach (var connectionPlace in pipeElement.PipeConnectionObject.GetEmptyPlaceFromObject())
                    {
                        list.Add(connectionPlace);
                    }
                }
            }

            return list;
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

        public void AnimatopLastSpawnedPipeView(Color color)
        {
            var lastPipe = GetLastSpawnedPipeView();

            lastPipe.MeshRenderer.material.DOColor(color, .5f)
                .OnComplete(() =>
                    {
                        lastPipe.MeshRenderer.material.DOColor(Color.white, .5f);
                    });
        }
    }
}