using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionCardboard : MonoBehaviour
{
    Vector3 baseScale;

    private void Start()
    {
        baseScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var playerCarboard = other.GetComponent<CompteurCardboard>();
            if (playerCarboard.CanPickupPizzaBox())
            {
                playerCarboard.PickupPizzaBox(transform.parent);

                var iTween = GetComponent<iTween>();

                transform.rotation = Quaternion.Euler(Vector3.zero);
                transform.localScale = baseScale;

                Destroy(iTween);
                Destroy(this);
            }
        }
    }
}
