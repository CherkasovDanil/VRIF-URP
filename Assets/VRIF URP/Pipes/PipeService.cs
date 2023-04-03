using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace VRIF_URP.Pipes
{
    public class PipeService
    {
        private readonly PipeConfig _config;
        private readonly IInstantiator _instantiator;

        private int _maxId;
        private int id;
        
        private Dictionary<int, GameObject> _pipeStorage = new Dictionary<int, GameObject>();
        private Dictionary<int, Transform> _connectionPlaceDictionary = new Dictionary<int, Transform>();

        public PipeService(
            PipeConfig pipeConfig,
            IInstantiator instantiator)
        {
            _config = pipeConfig;
            _instantiator = instantiator;

            _maxId = _config.GetPrefabsCount();
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
    }
}