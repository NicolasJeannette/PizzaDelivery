using PathCreation.Examples;
using UnityEngine;

public class DemarrageJeu : MonoBehaviour
{
    // Start is called before the first frame update
    public bool gamePressed;
    public PathFollower pathfollower;
    public GameObject texte;
    public GameObject compteur;
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
            compteur.SetActive(true);
            //pathfollower.speed = 14f;
        }
    }
    public void OnClick()
    {
        gamePressed = true;
    }
}
