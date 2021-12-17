using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScriptNote : MonoBehaviour
{
    public static int nbCarton;
    public int noteNecessaire;
    public int prochainObjectif;
    public bool lastCrown;
    public TMP_Text texte;
    public GameObject image;
    public List<GameObject> textePrecedent;
    // Start is called before the first frame update
    void Start()
    {
        if (nbCarton >= noteNecessaire)
        {
            image.SetActive(true);
            if (lastCrown == false)
            {
                texte.text = $"Prochain objectif : {prochainObjectif}";
                
            }
            else { texte.text = "Vous avez rempli tous les objectifs ! Bravo !"; }

            foreach (var item in textePrecedent)
            {
                item.SetActive(false);
            }
        }
        else
        {
            image.SetActive(false);
        }
    }
}
