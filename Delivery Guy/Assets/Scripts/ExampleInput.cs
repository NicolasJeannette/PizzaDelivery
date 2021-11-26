using UnityEngine;

public class ExampleInput : MonoBehaviour
{


	[Header("Controls")]
#pragma warning disable 0649
	[SerializeField] KeyCode strafeLeft;
	[SerializeField] KeyCode strafeRight;

	[SerializeField] Joystick strafeJoystick;
#pragma warning restore 0649

	[Header("Settings")]
	[SerializeField] Transform centerAnchor;
	[SerializeField] Transform rightAnchor;
	[SerializeField] float clampX = 5;
	[SerializeField] float strafSpeed;
	[SerializeField] float strafRotateSpeed = 15;
	[SerializeField] float counterStrafRotateSpeed = 25;
	[SerializeField] public float maxStrafRot = 30;
	public float curRot = 0;
	
	float strafDir;

    private void Update()
	{
        // Strafing
        strafDir = GetStarfDirection();
        //strafDir = strafeJoystick.Horizontal *-1;


		if ((transform.localPosition.x > -clampX && strafDir > 0) || (transform.localPosition.x < clampX && strafDir < 0))
        {
			var dir = centerAnchor.position - rightAnchor.position;

	        transform.Translate(dir.normalized * strafSpeed * strafDir * Time.deltaTime, Space.World);
        }
		
		if (strafDir != 0)
        {
			if ((curRot < maxStrafRot && strafDir > 0) || (curRot > -maxStrafRot && strafDir < 0))
            {
				var rot = strafRotateSpeed * strafDir * Time.deltaTime;

				if (curRot > 0 && strafDir < 0 || curRot < 0 && strafDir > 0)
                {
					rot *= 2;
                }

				transform.Rotate(Vector3.forward * rot, Space.Self);

				curRot += rot;
            }
        }
		else if (curRot > 1f || curRot < -1f)
        {
			var counterStrafDir = curRot > 0 ? -1 : 1;
			var counterSpeedLerped = Mathf.Lerp(0, counterStrafRotateSpeed, Mathf.Abs(curRot) / maxStrafRot);
			var rot = counterSpeedLerped * counterStrafDir * Time.deltaTime;

			transform.Rotate(Vector3.forward * rot, Space.Self);

			curRot += rot;
		}
	}

    private float GetStarfDirection()
    {
		float dir = 0;

        dir += Input.GetKey(strafeLeft) ? 1 : 0;
        dir += Input.GetKey(strafeRight) ? -1 : 0;

        return dir;

	}
}
