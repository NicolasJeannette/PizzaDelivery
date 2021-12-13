using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectionCardboard : MonoBehaviour
{
    public AnimationCurve scaleCurve;
    Vector3 baseScale;
    public float timeToFly = 0.6f;
    bool verificationCollecte;
    
    private void Start()
    {
        baseScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var playerCarboard = other.GetComponent<CompteurCardboard>();
            if (playerCarboard.CanPickupPizzaBox() && verificationCollecte == false)
            {
                verificationCollecte = true;
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
            transform.localScale = baseScale * scaleCurve.Evaluate(currentTime / timeToFly);

            yield return new WaitForEndOfFrame();
            
            currentTime += Time.deltaTime;

            if (Vector3.Distance(transform.parent.position, pizzaBoxPos) < 0.1f)
            {
                currentTime = timeToFly;
            }
        }

        transform.DOKill();
        

        transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 180));
        transform.localScale = baseScale;

        compteurCardboard.PickupPizzaBox(transform.parent);

        Destroy(this);
    }
}
