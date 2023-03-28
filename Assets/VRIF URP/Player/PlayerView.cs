using UnityEngine;
using Zenject;

namespace VRIF_URP.Player
{
    public class PlayerView : SceneObject.SceneObject
    {
        public CharacterController CharacterController => characterController;
        public OVRScreenFade ScreenFade => screenFade;
        public OVRCameraRig CameraRig => cameraRig;
        
        [SerializeField] private OVRScreenFade screenFade;
        [SerializeField] private OVRCameraRig cameraRig;
        [SerializeField] private CharacterController characterController;
    }
}