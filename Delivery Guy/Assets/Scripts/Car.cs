using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class Car : MonoBehaviour
{
    public float speed = 30f;
    public float collideForce = 10f;
    public float timeBump;
    public float distanceBump;
    public bool estBump;

    private Vector3 _positionDepart;
    private Vector3 _positionFin;
    private float _timeStartedLerping;
    private Rigidbody rigidbody;

    public PathCreation.Examples.PathFollower pathFollower;

    void Start()
    {
         rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.Translate(-transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
       if(collision.gameObject.tag == "Player")
        {
            var cc = collision.gameObject.GetComponent<CompteurCardboard>();
            cc.LooseOnePizza();

            

            speed = 0;
            foreach (PathFollower pf in cc.pathFollowers)
                StartCoroutine(Recall(pf));
            StartLerping();

        }
    }

    private IEnumerator Recall(PathFollower pathFollower)
    {
        pathFollower.speed *= -1;
        yield return new WaitForSeconds(0.4f);
        pathFollower.speed *= -1;

    }

    void StartLerping()
    {
        estBump = true;
        _timeStartedLerping = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _positionDepart = rigidbody.position; /*;*/
        _positionFin = new Vector3(rigidbody.position.x - distanceBump, rigidbody.position.y, rigidbody.position.z);
    }

    void FixedUpdate()
    {
        if (estBump)
        {
            
            //We want percentage = 0.0 when Time.time = _timeStartedLerping
            //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - _timeStartedLerping" is.
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeBump;

            //Perform the actual lerping.  Notice that the first two parameters will always be the same
            //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
            //to start another lerp)
            rigidbody.position = Vector3.Lerp(_positionDepart, _positionFin, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1.0f)
            {
                estBump = false;
            }
        }
    }
}
