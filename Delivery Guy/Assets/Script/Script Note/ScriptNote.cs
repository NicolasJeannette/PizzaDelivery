using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptNote : MonoBehaviour
{
    public static int nbCarton;
    public int noteNecessaire;
    public GameObject image;
    // Start is called before the first frame update
    void Start()
    {
        if (nbCarton >= noteNecessaire)
        {
            image.SetActive(true);
        }
        else
        {
            image.SetActive(false);
        }
    }
}
