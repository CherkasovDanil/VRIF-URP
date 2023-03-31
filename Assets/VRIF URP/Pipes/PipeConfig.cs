using System;
using UnityEngine;

namespace VRIF_URP.Pipes
{
    [Serializable]
    public struct PipeModel
    {
       
    }

    [CreateAssetMenu(fileName = "PipeConfig", menuName = "Configs/PipeConfig", order = 0)]
    public class PipeConfig : ScriptableObject
    {
        [SerializeField] private PipeModel[] models;
        
        public PipeModel Get(int id)
        {
            return id < models.Length ? models[id] : new PipeModel();
        }
    }
}