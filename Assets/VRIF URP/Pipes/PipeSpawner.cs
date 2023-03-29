using UnityEngine;

namespace VRIF_URP.Pipes
{
    public class PipeSpawner
    {
        private readonly Pipe.Factory _pipeFactory;

        private Transform _lastPos;

        public PipeSpawner(Pipe.Factory pipeFactory)
        {
            _pipeFactory = pipeFactory;
        }
        
        public void SpawnPipe(Transform transform)
        {
            var pipe = _pipeFactory.Create();
            
            pipe.transform.position = transform.position;
        }
    }
}