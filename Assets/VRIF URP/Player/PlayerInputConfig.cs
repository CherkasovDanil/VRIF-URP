using UnityEngine;

namespace VRIF_URP.Player
{
    [CreateAssetMenu(fileName = "PlayerInputConfig", menuName = "Configs/PlayerInputConfig")]
    public class PlayerInputConfig : ScriptableObject
    {
        public int RotateModifier;
        public int DelayPerRotate;
        public float TopPrimaryThumbstickRotateLim;
        public float LowPrimaryThumbstickRotateLim;
    }
}