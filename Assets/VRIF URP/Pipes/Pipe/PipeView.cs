using UnityEngine;
using VRIF_URP.Player;
using Zenject;

public enum PipeType
{
    Line,
    LForm
}

namespace VRIF_URP.Pipes
{
    public class PipeView : MonoBehaviour
    {
        public PipeDirection PipeDirection;
        public PipeConnectionObject PipeConnectionObject;
        public MeshRenderer MeshRenderer;

        [SerializeField] private PipeType pipeType;

        public PipeType GetPipeType()
        {
            return pipeType;
        }

        public class Factory : PlaceholderFactory<PipeView>
        { }
    }
}