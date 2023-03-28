using UnityEngine;

namespace VRIF_URP.Player
{
    public class PlayerView : SceneObject.SceneObject
    {
        public OVRScreenFade ScreenFade => screenFade;
        public OVRCameraRig CameraRig => cameraRig;
        
        [SerializeField] private OVRScreenFade screenFade;
        [SerializeField] private OVRCameraRig cameraRig;
    }
}