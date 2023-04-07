using System.Collections.Generic;
using UnityEngine;

namespace VRIF_URP.Room
{
    public class RoomView : SceneObject.SceneObject
    {
        public List<PipeConnectionObject> AllPipeConnectionObject => allPipeConnectionObject;
        
        [SerializeField] private List<PipeConnectionObject> allPipeConnectionObject;
    }
}