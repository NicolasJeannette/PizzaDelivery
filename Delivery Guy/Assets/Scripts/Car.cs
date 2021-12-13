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
            var cc = collision.gameObject.GetComponent<CompteurCardboard>();
            cc.LooseOnePizza();

            //this.GetComponent<Rigidbody>().AddForce(Vector3.up * collideForce, ForceMode.Impulse);

            speed = 0;

            StartCoroutine(Recall());
        }
    }

    private IEnumerator Recall()
    {
        pathFollower.speed *= -1;
        yield return new WaitForSeconds(0.3f);
        pathFollower.speed *= -1;
    }
}
