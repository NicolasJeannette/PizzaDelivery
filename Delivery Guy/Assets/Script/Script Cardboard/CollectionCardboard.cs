using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectionCardboard : MonoBehaviour
{
    public AnimationCurve scaleCurve;
    Vector3 baseScale;
    public float timeToFly = 2f;
    public float localY = 1f;
    public const float mediantY = 3f;

    private void Start()
    {
        baseScale = transform.GetChild(0).localScale;
        timeToFly = 0.9f;
        localY = 0.3f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var playerCarboard = other.GetComponent<CompteurCardboard>();
            if (playerCarboard.CanPickupPizzaBox())
            {
                StartCoroutine(JumpInBagRoutine(playerCarboard));
            }
        }
    }

    private IEnumerator JumpInBagRoutine(CompteurCardboard compteurCardboard)
    {
        var pizzaBoxNb = compteurCardboard.GrabPizzaBox();

        transform.SetParent(null);

        Vector3 targetTransform = compteurCardboard.pizzaBoxScooter.position;

        Vector3 initialTransform = this.transform.position;


        Vector3 halfPoint = new Vector3(
            (targetTransform.x + initialTransform.x) / 2,
            initialTransform.y + compteurCardboard.mediant + pizzaBoxNb * compteurCardboard.stackCurve.Evaluate(pizzaBoxNb / CompteurCardboard.MAX_BOX),
            (targetTransform.z + initialTransform.z) / 2
        );


        float _t = 0;
        float _targetT = timeToFly;

        while (_t < _targetT)
        {
            float _ratioToEnd = _t / _targetT;
            targetTransform = compteurCardboard.pizzaBoxScooter.position;
            this.transform.position = BezierCurve.QuadraticCurve(initialTransform, halfPoint, targetTransform, _ratioToEnd);


            _t += Time.deltaTime;
            yield return null;
        }

        var child = transform.GetChild(0);
        child.DOKill();

        child.localRotation = Quaternion.Euler(new Vector3(-90, 0, 180));
        child.localScale = baseScale;

        compteurCardboard.PickupPizzaBox(transform);

        Destroy(this);
    }
}
