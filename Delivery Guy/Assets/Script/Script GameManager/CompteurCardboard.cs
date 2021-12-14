using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CompteurCardboard : MonoBehaviour
{
    public const float MAX_BOX = 400;

    [SerializeField] int nbCarton = 0;
    public int NbCarton { get => nbCarton; }
    public Transform pizzaBoxAnchor;
    public Transform pizzaBoxScooter;
    public ExampleInput exampleInput;

    [Header("Control")]
    public float minRotEfficiency = 0;
    public float maxRotEfficiency = 50;
    public float pizzaBoxY;
    public AnimationCurve rotCurve;
    public AnimationCurve posCurve;
    public float evaluateSmoothness = 0.01f;
    public float speed = 1;

    [Header("")]
    public float pizzaDisapearTime = 0.7f;
    public float pizzaDisapearRot = 90;
    public AnimationCurve pizzaScaleCurve;
    public float x;

    public float mediant = 3f;
    public float mediantMultiplicateur = 0.2f;
    public AnimationCurve stackCurve;

    public PathCreation.Examples.PathFollower[] pathFollowers;

    public float timeToDestroyPizza = 1.3f;
    public float destroyedPizzaScale = 1.4f;
    public float destroyedPizzaScaleTime = 1.4f;
    public float variableRotate = 30f;
    public float variableRotateTps = 1f;

    private void LateUpdate()
    {
        var firstBox = pizzaBoxAnchor.GetChild(0);
        firstBox.position = pizzaBoxScooter.position;
        firstBox.localPosition = new Vector3(firstBox.localPosition.x + x, firstBox.localPosition.y, firstBox.localPosition.z);

        for (int i = 1; i < pizzaBoxAnchor.childCount; i++)
        {
            Transform previousPizzaBox = pizzaBoxAnchor.GetChild(i - 1);
            Transform curPizzaBox = pizzaBoxAnchor.GetChild(i);

            //curPizzaBox.position = previousPizzaBox.position;

            //curPizzaBox.localPosition = Vector3.Lerp(curPizzaBox.localPosition, previousPizzaBox.localPosition, speed * Time.deltaTime);

            var pos = curPizzaBox.localPosition;
            pos.x = Mathf.Lerp(curPizzaBox.localPosition.x, previousPizzaBox.localPosition.x, speed * Time.deltaTime);
            pos.y = firstBox.localPosition.y + i * pizzaBoxY;
            curPizzaBox.localPosition = pos;
        }
    }

    public bool CanPickupPizzaBox()
    {
        return nbCarton < MAX_BOX;
    }

    public int GrabPizzaBox()
    {
        nbCarton++;
        return nbCarton;
    }

    public void PickupPizzaBox(Transform pizzaBox)
    {
        pizzaBox.SetParent(pizzaBoxAnchor);

        var newPos = Vector3.zero;
        newPos.y = pizzaBoxAnchor.GetChild(0).localPosition.y + nbCarton * pizzaBoxY;
        pizzaBox.localPosition = newPos;
        pizzaBox.localRotation = Quaternion.identity;
    }

    public void LooseOnePizza()
    {
        for (int i = 0; i < 3; i++) {
            if (nbCarton > 0) {
                var pizzaToDestroy = pizzaBoxAnchor.GetChild(pizzaBoxAnchor.childCount - 1);
                pizzaToDestroy.parent = null;

                nbCarton -= 1;

                StartCoroutine(LoosePizza(pizzaToDestroy));
            }
        }
    }

    private IEnumerator LoosePizza(Transform pizzaToDestroy)
    {
        var sphereRay = Random.onUnitSphere;
        sphereRay.y = -1;

        Vector3 targetTransform = pizzaToDestroy.position + sphereRay * 10f;

        Vector3 initialTransform = pizzaToDestroy.position;


        Vector3 halfPoint = new Vector3(
            (targetTransform.x + initialTransform.x) / 2,
            initialTransform.y + 6f,
            (targetTransform.z + initialTransform.z) / 2
        );


        float _t = 0;
        float _targetT = timeToDestroyPizza;

        pizzaToDestroy.DOScale(destroyedPizzaScale, destroyedPizzaScaleTime);
        pizzaToDestroy.DORotate(new Vector3(30, 30, 30) * variableRotate, variableRotateTps, RotateMode.WorldAxisAdd).SetEase(Ease.Linear);

        while (_t < _targetT)
        {
            float _ratioToEnd = _t / _targetT;
            pizzaToDestroy.position = BezierCurve.QuadraticCurve(initialTransform, halfPoint, targetTransform, _ratioToEnd);


            _t += Time.deltaTime;
            yield return null;
        }
    }
}
