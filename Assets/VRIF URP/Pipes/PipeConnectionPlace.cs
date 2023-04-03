using System;
using UnityEngine;
using VRIF_URP.Player;

namespace VRIF_URP.Pipes
{
    public class PipeConnectionPlace : MonoBehaviour
    {
        public Direction GetPipePlaceDirection => direction;
        public bool IsEmpty => _isEmpty;
        
        public Transform GetConnectionPlace => transform;

        [SerializeField] private Direction direction;
        
        private bool _isEmpty = true;

        private void OnTriggerStay(Collider other)
        {
            _isEmpty = false;
            Debug.Log(_isEmpty);
        }

        private void OnTriggerExit(Collider other)
        {
            _isEmpty = true;
        }

    }
}