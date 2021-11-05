using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionCardboard : MonoBehaviour
{
    // Start is called before the first frame update

    CompteurCardboard compteur;
    void Awake()
    {
        compteur = GameObject.FindWithTag("Player").GetComponent<CompteurCardboard>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (compteur.nbCarton < 15)
            {
                compteur.nbCarton = compteur.nbCarton + 1;
            }
            Destroy(gameObject);
        }
    }
}
