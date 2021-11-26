using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemarrageJeu : MonoBehaviour
{
    // Start is called before the first frame update
    public bool gamePressed;
    public PathFollower pathfollower;
    public GameObject texte;
    void Start()
    {
        gamePressed = false;
        texte.SetActive(true);
        pathfollower = GameObject.FindWithTag("PathJoueur").GetComponent<PathFollower>();
    }

    // Update is called once per frame
    void Update()
    {    
            if (gamePressed)
            {
                texte.SetActive(false);
                pathfollower.speed = 14f;
            }  
    }
    public void OnClick()
    {
        gamePressed = true;
    }
}
