using UnityEngine;

namespace VRIF_URP.Pipes
{
    public struct PlugsView
    {
        public Transform PlugsTransform;
        public Collider PlugsCollider;
        public bool PlugsIsUse;
    }
    public class InteractableView : MonoBehaviour
    {
        //[SerializeField] private PlugsView[] plugsViews;
        public Transform ConnectionTransform;
        public Collider PlugsCollider;
        public bool PlugsIsUse;
    }
}