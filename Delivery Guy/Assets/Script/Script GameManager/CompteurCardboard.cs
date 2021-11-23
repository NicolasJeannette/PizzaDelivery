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

    private void Update()
    {
        for (int i = 0; i < pizzaBoxAnchor.childCount; i++)
        {
            var pizzaBox = pizzaBoxAnchor.GetChild(i);

            var pizzaEfficiencyPercentage = (float)i / (float)pizzaBoxAnchor.childCount;

            var pizzaBoxPos = pizzaBoxAnchor.position;
            pizzaBoxPos.y += i * pizzaBox.GetChild(0).localScale.y;
            pizzaBoxPos.z += -exampleInput.curRot * 0.1f * posRotOffset * pizzaEfficiencyPercentage;


            pizzaBox.rotation = pizzaBoxAnchor.rotation;
            var currentEfficiency = Mathf.Lerp(minRotEfficiency, maxRotEfficiency, pizzaEfficiencyPercentage);
            pizzaBox.Rotate(0, 0, -exampleInput.curRot * currentEfficiency, Space.Self);

            pizzaBox.position = pizzaBoxPos;
        }
    }

    public bool CanPickupPizzaBox()
    {
        return nbCarton < 15;
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
