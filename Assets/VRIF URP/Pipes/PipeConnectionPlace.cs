using UnityEngine;
using VRIF_URP.Player;

namespace VRIF_URP.Pipes
{
    public class PipeConnectionPlace : MonoBehaviour
    {
        public bool BlockPlace = true;
        
        public PipeDirection GetPipePlacePipeDirection => pipeDirection;
       
        [SerializeField] private PipeDirection pipeDirection;
        
        public void SetPipePlacesForConnection(bool state)
        {
            BlockPlace = state;
        }
    }
}