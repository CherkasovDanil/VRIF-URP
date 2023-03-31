using UnityEngine;

namespace VRIF_URP.Pipes
{
    public class PipeSpawner
    {
        private readonly PipeView.Factory _pipeFactory;

        private Transform _lastPos;

        public PipeSpawner(PipeView.Factory pipeFactory)
        {
            _pipeFactory = pipeFactory;
        }
        
        public PipeView SpawnPipe()
        {
           return _pipeFactory.Create();
        }
    }
}