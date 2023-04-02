using UnityEngine;

namespace VRIF_URP.Pipes
{
    public class PipeSpawner
    {
        private readonly PipeView.Factory _pipeFactory;
        private readonly PipeConfig _pipeConfig;
        private readonly PipeService _pipeService;

        private Transform _lastPos;

        private int _maxCount;
        private int _iterator;

        public PipeSpawner(
            PipeView.Factory pipeFactory,
            PipeConfig pipeConfig,
            PipeService pipeService)
        {
            _pipeFactory = pipeFactory;
            _pipeConfig = pipeConfig;
            _pipeService = pipeService;

            _maxCount = pipeConfig.GetPrefabsCount();
        }
        
        public GameObject SpawnPipe()
        {
            return _pipeService.Spawn();
        }
    }
}