using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfPath : MonoBehaviour
{
    public float timeToReceivePizza = 0.7f;
    public Transform pizzaScooterAnchor;
    public Transform endOfPathAnchor;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var compteurCardboard = collision.GetComponent<CompteurCardboard>();
            StartCoroutine(ReceivePizzas(compteurCardboard));
        }
    }

    private IEnumerator ReceivePizzas(CompteurCardboard compteurCardboard)
    {
        while (compteurCardboard.pizzaBoxAnchor.childCount > 0)
        {
            var latestPizzaBox = compteurCardboard.pizzaBoxAnchor.GetChild(compteurCardboard.pizzaBoxAnchor.childCount - 1);
            
            latestPizzaBox.SetParent(endOfPathAnchor);
            var newPos = endOfPathAnchor.position;
            newPos.y = compteurCardboard.pizzaBoxAnchor.childCount * 0.1f;
            latestPizzaBox.position = newPos;

            yield return new WaitForSeconds(timeToReceivePizza);
        }
    }
}
