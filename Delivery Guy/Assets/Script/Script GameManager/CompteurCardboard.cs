using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompteurCardboard : MonoBehaviour
{
    [SerializeField] int nbCarton = 0;
    public int NbCarton { get => nbCarton; }
    public Transform pizzaBoxAnchor;
    public ExampleInput exampleInput;

    public float minRotEfficiency = 0;
    public float maxRotEfficiency = 50;
    public float posRotOffset = 1;
    public float pizzaBoxY;
    public AnimationCurve rotCurve;
    public AnimationCurve posCurve;
    public float evaluateSmoothness = 0.01f;

    private void Update()
    {
        for (int i = 0; i < pizzaBoxAnchor.childCount; i++)
        {
            var pizzaBox = pizzaBoxAnchor.GetChild(i);

            var pizzaEfficiencyPercentage = (float)i / (float)pizzaBoxAnchor.childCount;

            var pizzaBoxPos = pizzaBoxAnchor.position;
            pizzaBoxPos += transform.right * -exampleInput.curRot * posRotOffset * posCurve.Evaluate(pizzaEfficiencyPercentage);
            pizzaBoxPos.y = pizzaBoxAnchor.position.y + i * pizzaBoxY;

            pizzaBox.position = pizzaBoxPos;
            pizzaBox.rotation = pizzaBoxAnchor.rotation;

            var efficiency = rotCurve.Evaluate(i * evaluateSmoothness);
            pizzaBox.Rotate(0, 0, exampleInput.curRot * efficiency, Space.Self);
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

    private void OnTriggerEnter(Collider other)
    {
        // TODO : Enlever des cartons tant qu'on collide avec une voiture
        if (other.gameObject.tag == "Ennemy")
        {
            if (nbCarton > 0)
            {
                nbCarton -= 1;
                Destroy(pizzaBoxAnchor.GetChild(nbCarton).gameObject);
            }
        }
    }
}
