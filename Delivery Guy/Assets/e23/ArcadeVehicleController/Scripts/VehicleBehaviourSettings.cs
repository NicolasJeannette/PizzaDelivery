using UnityEngine;

namespace e23.VehicleController
{
    [CreateAssetMenu(fileName = nameof(VehicleBehaviourSettings), menuName = "e23/Vehicle Settings", order = 3)]
    public class VehicleBehaviourSettings : ScriptableObject
    {
        [Header("Parameters")]
        [Range(1.0f, 100.0f)] public float acceleration = 5f;
        [Range(5.0f, 1000.0f)] public float maxSpeed = 30f;
        [Range(1.0f, 15.0f)] public float breakSpeed = 5f;
        [Range(5.0f, 200.0f)] public float boostSpeed = 60f;
        [Range(1.0f, 500.0f)] public float maxSpeedToStartReverse = 150f;
        [Range(20.0f, 160.0f)] public float steering = 80f;
        [Range(1.0f, 40.0f)] public float maxStrafingSpeed = 15f;
        [Range(0.0f, 20.0f)] public float gravity = 10f;
        [Range(0.0f, 1.0f)] public float drift = 1f;
        [Range(0.0f, 3.0f)] public float vehicleBodyTilt = 0f;
        [Range(1.0f, 10.0f)] public float forwardTilt = 8f;
        [Range(1.0f, 50.0f)] public float straf = 8f;

        [Header("Switches")]
        public bool turnInAir = true;
        public bool turnWhenStationary = true;
        public bool twoWheelTilt = false;
        public bool stopSlopeSlide = true;

        [Header("Ground Layer")]
        public LayerMask groundMask = 1 << 0;
    }
}