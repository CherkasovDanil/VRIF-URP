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
        
        public void SpawnPipe(GameObject gameObject)
        {
            var view = gameObject.GetComponent<InteractableView>(); 
            var pipe = _pipeFactory.Create();
            
            pipe.transform.position = view.transform.position;
        }
    }
}