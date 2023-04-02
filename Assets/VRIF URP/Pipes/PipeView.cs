using UnityEngine;
using Zenject;

namespace VRIF_URP.Pipes
{
    public class PipeView : MonoBehaviour
    {
        private IMemoryPool _pool;

        private int _prefabId;
        
        /*
        [Inject]
        private void Inject(
            PipeConfig pipeConfig,
            IInstantiator instantiator)
        {
            var prefab = pipeConfig.Get(_prefabId);
            if (prefab.HasValue)
            {
               var go =  instantiator.InstantiatePrefab(prefab.Value.Prafab);
               go.gameObject.transform.SetParent(transform);
            }
        }*/

        public class Factory : PlaceholderFactory<int, PipeView>
        { }
    }
}