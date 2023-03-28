using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VRIF_URP.SceneObject
{
    public class SceneHolder
    {
        private Dictionary<Type, SceneObject> _sceneObjectsStorage = new Dictionary<Type, SceneObject>();

        public void Add<T>(SceneObject obj) where T : SceneObject
        {
            var type = typeof(T);

            if (_sceneObjectsStorage.ContainsKey(type))
            {
                Debug.LogError($"Object with type \"{type}\" has already been added");
                return;
            }
            
            _sceneObjectsStorage.Add(type, obj);
        }

        public T Get<T>() where T : SceneObject
        {
            var type = typeof(T);

            if (_sceneObjectsStorage.ContainsKey(type))
            {
                return (T)_sceneObjectsStorage[type];
            }

            Debug.LogError($"Object with type \"{type}\" is not found for getting");
            return default;
        }

        public bool Contains<T>() where T : SceneObject
        {
            var type = typeof(T);

            if (_sceneObjectsStorage.ContainsKey(type))
            {
                return true;
            }
            
            return false;
        }

        public void Remove<T>() where T : SceneObject
        {
            var type = typeof(T);

            if (_sceneObjectsStorage.ContainsKey(type))
            {
                Object.Destroy(_sceneObjectsStorage[type].gameObject);
                _sceneObjectsStorage.Remove(type);
                
                return;
            }

            Debug.LogError($"Object with type \"{type}\" is not found for removing");
        }
    }
}