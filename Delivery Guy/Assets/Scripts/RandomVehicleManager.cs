using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVehicleManager : MonoBehaviour
{
    public RandomVehicle rv;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var vehicle = collision.GetComponent<RandomVehicle>();

        if (vehicle == rv)
        {
            vehicle.ResetPos();
        }
    }
}
