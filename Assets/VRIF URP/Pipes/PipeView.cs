using UnityEngine;
using VRIF_URP.Player;
using Zenject;

namespace VRIF_URP.Pipes
{
    public class PipeView : MonoBehaviour
    {
        public PipeDirection PipeDirection;

        public PipeConnectionObject PipeConnectionObject;

        public MeshRenderer MeshRenderer;
        
        [SerializeField] private int id;

        public bool IgnoreUpDownDiretion;
        public bool IgnoreLeftRightDiretion;

        public int GetPipeViewID()
        {
            return id;
        }
        
        public class Factory : PlaceholderFactory<PipeView>
        { }
    }
}