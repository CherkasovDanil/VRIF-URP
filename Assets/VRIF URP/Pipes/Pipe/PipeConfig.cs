using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRIF_URP.Pipes
{
    [Serializable]
    public struct PipeModel
    {
        public int ID;
        public GameObject Prafab;
    }

    [CreateAssetMenu(fileName = "PipeConfig", menuName = "Configs/PipeConfig", order = 0)]
    public class PipeConfig : ScriptableObject
    {
        [SerializeField] private PipeModel[] pipeModels;
        
        private Dictionary<int, PipeModel> _dict;
        
        [NonSerialized] private bool _isInited;
        
        public PipeModel? Get(int id)
        {
            if(!_isInited)
            {
                Init();
            }
            
            if( _dict.ContainsKey(id))
            {
                return _dict[id];
            }

            Debug.LogError($"Model with id {id} not found, returned null");
            return null;
        }
        
        private void Init()
        {
            _dict = new Dictionary<int, PipeModel>();
            foreach (var model in pipeModels)
            {
                _dict.Add(model.ID, model);
            }
            _isInited = true;
        }
    }
}