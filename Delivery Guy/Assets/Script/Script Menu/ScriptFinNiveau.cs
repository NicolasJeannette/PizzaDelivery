using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptFinNiveau : MonoBehaviour
{
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
            if (compteur.NbCarton >= 10 )
            {
                ScriptNote.nbCarton = compteur.NbCarton;
                SceneManager.LoadScene("MenuWin");
            }
            else
            {
                SceneManager.LoadScene("MenuLoose");
            }
            
        }
    }
}
