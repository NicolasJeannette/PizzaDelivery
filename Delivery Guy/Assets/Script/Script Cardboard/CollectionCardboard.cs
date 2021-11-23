using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionCardboard : MonoBehaviour
{
    Vector3 baseScale;
    float timeToFly = 1.3f;

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
                StartCoroutine(MoveToScooter(playerCarboard));
            }
        }
    }

    private IEnumerator MoveToScooter(CompteurCardboard compteurCardboard)
    {
        float currentTime = 0;

        while (currentTime < timeToFly)
        {
            var pizzaBoxPos = compteurCardboard.pizzaBoxAnchor.position;
            pizzaBoxPos.y += compteurCardboard.pizzaBoxAnchor.childCount * transform.localScale.y;

            transform.parent.position = Vector3.Lerp(transform.parent.position, pizzaBoxPos, currentTime / timeToFly);
            yield return new WaitForEndOfFrame();
            currentTime += Time.deltaTime;

            if (Vector3.Distance(transform.parent.position, pizzaBoxPos) < 0.1f)
            {
                currentTime = timeToFly;
            }
        }

        compteurCardboard.PickupPizzaBox(transform.parent);

        var iTween = GetComponent<iTween>();

        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = baseScale;

        Destroy(iTween);
        Destroy(this);
    }
}
