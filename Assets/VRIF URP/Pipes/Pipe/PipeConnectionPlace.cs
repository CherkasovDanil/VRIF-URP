using UnityEngine;
using VRIF_URP.Player;

namespace VRIF_URP.Pipes
{
    public class PipeConnectionPlace : MonoBehaviour
    {
        public bool BlockPlace => _blockPlace;
        
        public PipeDirection GetPipePlacePipeDirection => pipeDirection;
       
        [SerializeField] private PipeDirection pipeDirection;
        
        public bool _blockPlace;

        public void SetStateBlockPipePlacesForConnection(bool state)
        {
            _blockPlace = state;
        }

        public void SetPipeConnectionDirection(PipeDirection pipeDirection)
        {
            this.pipeDirection = pipeDirection;
        }
    }
}