using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVehicle : MonoBehaviour
{
    public float speed = 10;

    private Vector3 basePos;

    void Start()
    {
        basePos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-transform.forward * speed * Time.deltaTime, Space.World);
    }

    public void ResetPos()
    {
        transform.position = basePos;
    }
}
