using System.Collections.Generic;
using UnityEngine;
using VRIF_URP.Pipes;

namespace VRIF_URP.Room
{
    public class RoomView : SceneObject.SceneObject
    {
        [SerializeField] private List<PipeConnectionObject> allPipeConnectionObject;

        public List<PipeConnectionPlace> GetEmptyPlace()
        {
            var allpoint = new List<PipeConnectionPlace>();
            foreach (var pipe in allPipeConnectionObject)
            {
                var list = pipe.GetEmptyPlaceFromObject();
                
                foreach (var point in list)
                {
                    allpoint.Add(point);
                }
            }

            return allpoint;
        }
    }
}