using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyClass : MonoBehaviour
{
    CompteurCardboard compteur;
    void Awake()
    {
        compteur = GameObject.FindWithTag("Player").GetComponent<CompteurCardboard>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
