using UnityEngine;
using Zenject;

namespace VRIF_URP.Pipes
{
    public class PipeView : MonoBehaviour
    {
        
        public class Factory : PlaceholderFactory<PipeView>
        { }
    }
}