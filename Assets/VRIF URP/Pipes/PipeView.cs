using UnityEngine;
using VRIF_URP.Player;
using Zenject;

namespace VRIF_URP.Pipes
{
    public class PipeView : MonoBehaviour
    {
        [SerializeField] private Direction direction;
        
        public bool IgnoreUpDownDiretion;
        public bool IgnoreLeftRightDiretion;

        public Direction GetPipeDirection()
        {
            return direction;
        }
        
        public class Factory : PlaceholderFactory<PipeView>
        { }
    }
}