using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Info : MonoBehaviour
{
    public CompteurCardboard donnee;
    public TMP_Text compteur;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        compteur.text = donnee.NbCarton.ToString();
    }
}
