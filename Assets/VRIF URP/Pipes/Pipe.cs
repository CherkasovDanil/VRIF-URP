using UnityEngine;
using Zenject;

namespace VRIF_URP.Pipes
{
    public class Pipe : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<Pipe>
        { }
    }
}