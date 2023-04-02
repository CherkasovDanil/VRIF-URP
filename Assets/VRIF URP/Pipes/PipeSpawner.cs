using UnityEngine;

namespace VRIF_URP.Pipes
{
    public class PipeSpawner
    {
        private readonly PipeView.Factory _pipeFactory;
        private readonly PipeConfig _pipeConfig;

        private Transform _lastPos;

        private int _maxCount;
        private int _iterator;

        public PipeSpawner(
            PipeView.Factory pipeFactory,
            PipeConfig pipeConfig)
        {
            _pipeFactory = pipeFactory;
            _pipeConfig = pipeConfig;

            _maxCount = pipeConfig.GetPrefabsCount();
        }
        
        public PipeView SpawnPipe()
        {
            _iterator++;
            if (_iterator>_maxCount)
            {
                _iterator = 0;
            }
            return _pipeFactory.Create(_iterator);
        }
    }
}