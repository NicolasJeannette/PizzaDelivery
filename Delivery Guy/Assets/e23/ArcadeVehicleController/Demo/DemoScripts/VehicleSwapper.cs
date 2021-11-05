using System.Collections.Generic;
using UnityEngine;

namespace e23.VehicleController.Demo
{
    public class VehicleSwapper : MonoBehaviour
    {
        [SerializeField] List<SwapData> vehiclesToSwap = null;
        [SerializeField] int startVehicle = 0;

        private int activeVehicle = 0;

        private void Awake() 
        {
            for (int i = 0; i < vehiclesToSwap.Count; i++)
            {
                Transform vehcileTransform = vehiclesToSwap[i].input.transform;
                vehiclesToSwap[i].ResetPos = vehcileTransform.position;
            }
        }

        private void Start() 
        {
            SwapVehicle(startVehicle);
        }

        public void SwapVehicle(int index)
        {
            for (int i = 0; i < vehiclesToSwap.Count; i++)
            {
                if (i == index)
                {
                    activeVehicle = index;
                    SetDataState(vehiclesToSwap[i], true);
                }
                else
                {
                    SetDataState(vehiclesToSwap[i], false);
                }
            }
        }

        private void SetDataState(SwapData swapData, bool active)
        {
            swapData.cameraParent.SetActive(active);
            swapData.input.enabled = active;
        }

        private void ResetVehiclePosition()
        {
            vehiclesToSwap[activeVehicle].vehicleBehaviour.SetPosition(vehiclesToSwap[activeVehicle].ResetPos, Quaternion.identity);
        }

        [System.Serializable]
        public class SwapData
        {
            public Examples.ExampleInput input;
            public bool isFollowCam = false;
            public GameObject cameraParent;
            public bool isHelicopter = false;

            private Vector3 startingPosition;
            public VehicleBehaviour vehicleBehaviour { get { return input.GetComponent<VehicleBehaviour>(); } }
            public Vector3 ResetPos { get { return startingPosition; } set { startingPosition = value; } }
        }
    }
}