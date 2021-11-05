using UnityEngine;

namespace e23.VehicleController
{
    public class WheelBehaviour : MonoBehaviour
    {
        [SerializeField] float maxMovement = 0.02f;
        [SerializeField] Transform wheel;

        private float wheelRadius;
        private float defaultYpos;
        private Vector3 endPos;

        private void Awake()
        {
            GetWheelRadius();
            SetDefaultYPos();
        }

        private void Test()
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = transform.parent;
            cube.transform.localScale = cube.transform.localScale * 0.1f;
            cube.transform.localPosition = transform.localPosition;
        }
        private void GetWheelRadius()
        {
            Bounds wheelBounds = GetComponentInChildren<Renderer>().bounds;
            wheelRadius = wheelBounds.size.y / 2;
        }

        private void SetDefaultYPos()
        {
            defaultYpos = wheel.localPosition.y;
        }
        RaycastHit hit;
        private void Update()
        {
            
            Vector3 distanceToObstacle = wheel.position;

            if (Physics.SphereCast(transform.position, wheelRadius, Vector3.down, out hit, wheelRadius))
            {
                Debug.Log(wheel.localPosition.y < defaultYpos + maxMovement);
                if (wheel.localPosition.y < defaultYpos + maxMovement)
                {
                    distanceToObstacle.y = hit.point.y + 1; //Mathf.Abs(hit.distance - wheelRadius);
                }
            }
            else
            {
                if (wheel.localPosition.y > defaultYpos - maxMovement)
                {
                    Debug.Log("this");
                    distanceToObstacle.y -= 0.1f;
                }
            }

            wheel.position = distanceToObstacle;
        }

        private void OnDrawGizmos()
        {
            endPos = new Vector3(transform.position.x, transform.position.y - wheelRadius, transform.position.z);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, endPos);
        }

    }
}