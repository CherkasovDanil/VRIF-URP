using UnityEngine;

namespace VRIF_URP.Player
{
    public class PlayerView : SceneObject.SceneObject
    {
        public GameObject LeftHand => leftHand;
        public GameObject RightHand => rightHand;
        public CharacterController CharacterController => characterController;
        public OVRScreenFade ScreenFade => screenFade;
        public OVRCameraRig CameraRig => cameraRig;

        [SerializeField] private OVRScreenFade screenFade;
        [SerializeField] private OVRCameraRig cameraRig;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private GameObject leftHand;
        [SerializeField] private GameObject rightHand;
    }
}