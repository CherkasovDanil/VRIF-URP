using System.Collections.Generic;
using UnityEngine;
using VRIF_URP.Pipes;

public class PipeConnectionObject : MonoBehaviour
{
    [SerializeField] private List<PipeConnectionPlace> pipeConnectionPlace;

    public List<PipeConnectionPlace> GetEmptyPlaceFromObject()
    {
        var list = new List<PipeConnectionPlace>();

        foreach (var place in pipeConnectionPlace)
        {
            if (!place.BlockPlace)
            {
                list.Add(place);
            }
        }

        return list;
    }
}
