using UnityEngine;
using VRIF_URP.Player;
using Zenject;

namespace VRIF_URP.Pipes
{
    public class PipeView : MonoBehaviour
    {
        //TODO add change direction
        [SerializeField] private PipeDirection pipeDirection;
        [SerializeField] private int id;
        
        public bool IgnoreUpDownDiretion;
        public bool IgnoreLeftRightDiretion;

        public PipeDirection GetPipeDirection()
        {
            return pipeDirection;
        }

        public int GetPipeViewID()
        {
            return id;
        }
        
        public class Factory : PlaceholderFactory<PipeView>
        { }
    }
}