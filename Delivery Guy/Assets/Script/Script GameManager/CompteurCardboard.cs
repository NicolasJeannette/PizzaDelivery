using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompteurCardboard : MonoBehaviour
{
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

    private void Update()
    {
        pizzaBoxAnchor.GetChild(0).position = pizzaBoxScooter.position;

        for (int i = 1; i < pizzaBoxAnchor.childCount; i++)
        {
            Transform previousPizzaBox = pizzaBoxAnchor.GetChild(i-1);
            Transform curPizzaBox = pizzaBoxAnchor.GetChild(i);

            curPizzaBox.position = Vector3.Lerp(curPizzaBox.position, previousPizzaBox.position, speed * Time.deltaTime);
            var pos = curPizzaBox.position;
            pos.y = pizzaBoxAnchor.position.y + i * pizzaBoxY;
            curPizzaBox.position = pos;
        }
    }

    private void Ouaip()
    {
        for (int i = 1; i < pizzaBoxAnchor.childCount; i++)
        {
            var pizzaBox = pizzaBoxAnchor.GetChild(i);

            var pizzaEfficiencyPercentage = (float)i / (float)pizzaBoxAnchor.childCount;

            var pizzaBoxPos = pizzaBoxAnchor.position;
            pizzaBoxPos += -transform.right * exampleInput.curRot * posCurve.Evaluate(pizzaEfficiencyPercentage);
            pizzaBoxPos.y = pizzaBoxAnchor.position.y + i * pizzaBoxY;

            pizzaBox.position = pizzaBoxPos;
            pizzaBox.rotation = pizzaBoxAnchor.rotation;

            var efficiency = rotCurve.Evaluate(i * evaluateSmoothness);
            //pizzaBox.Rotate(0, 0, -exampleInput.curRot * efficiency, Space.Self);
        }
    }

    public bool CanPickupPizzaBox()
    {
        return nbCarton < 400;
    }

    public void PickupPizzaBox(Transform pizzaBox)
    {
        nbCarton++;

        pizzaBox.SetParent(pizzaBoxAnchor);
    }
    public void LooseOnePizza()
    {
        if (nbCarton > 0)
        {
            StartCoroutine(PizzaDisapear());
        }
    }

    private IEnumerator PizzaDisapear()
    {
        var pizzaToDestroy = pizzaBoxAnchor.GetChild(pizzaBoxAnchor.childCount - 1);
        var baseLocalScale = pizzaToDestroy.localScale;
        float curTime = 0;

        var rotDir = Random.Range(0f, 1f) > 0.5f ? 1 : -1;

        yield return new  WaitWhile(() =>
        {
            curTime += Time.deltaTime;

            pizzaToDestroy.localScale = baseLocalScale * pizzaScaleCurve.Evaluate(curTime / pizzaDisapearTime);
            pizzaToDestroy.Rotate(Vector3.up * pizzaDisapearRot * rotDir * pizzaScaleCurve.Evaluate(curTime / pizzaDisapearTime));

            return curTime < pizzaDisapearTime;
        });

        nbCarton -= 1;
        Destroy(pizzaToDestroy.gameObject);
    }
}
