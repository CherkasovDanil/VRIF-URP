using UnityEngine;
using VRIF_URP.SceneObject;
using Zenject;

namespace VRIF_URP.Player
{
    public class RayVisualizer : ITickable
    {
        private PlayerView _playerView;
        private LineRenderer _lineRenderer;
        
        public RayVisualizer( 
            TickableManager tickableManager,
            SceneHolder sceneHolder)
        {
            _playerView = sceneHolder.Get<PlayerView>();
            
            _lineRenderer = _playerView.LineRenderer;
            
            tickableManager.Add(this);
        }
        
        public void Tick()
        {
            Ray ray = new Ray(_playerView.RightHand.transform.position,
                _playerView.RightHand.transform.TransformDirection(Vector3.forward * 5));
                
            Debug.DrawRay(_playerView.RightHand.transform.position,
                _playerView.RightHand.transform.TransformDirection(Vector3.forward * 5));
            
            _lineRenderer.SetPosition(0, _playerView.RightHand.transform.position);
            _lineRenderer.SetPosition(1, _playerView.RightHand.transform.TransformDirection(Vector3.forward * 5));
        }
    }
}