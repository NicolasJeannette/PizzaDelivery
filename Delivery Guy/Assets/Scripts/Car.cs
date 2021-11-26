using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float speed = 30f;
    public float collideForce = 10f;
    public PathCreation.Examples.PathFollower pathFollower;

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(-transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var ei = collision.gameObject.GetComponent<ExampleInput>();
            var collideDir = collision.transform.position - transform.position;
            ei.curRot = collideDir.magnitude > 0 ? ei.maxStrafRot : -ei.maxStrafRot;
            ei.GetComponent<Rigidbody>().AddForce(collideDir * collideForce, ForceMode.Impulse);

            speed = 0;
            //pathFollower.speed = 0;
        }
    }
}
