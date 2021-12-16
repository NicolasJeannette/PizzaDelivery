using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;
        public DemarrageJeu gameManager;
        public bool shouldWaitAtStart = true;
        public float waitTime = 1f;
        private bool isWaiting;
        private bool hasWaited;
        void Start() 
        {
            gameManager = GameObject.FindWithTag("GameManager").GetComponent<DemarrageJeu>();
            if (gameManager.gamePressed)
            {
                if (pathCreator != null)
                {
                    //Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                    pathCreator.pathUpdated += OnPathChanged;
                }
            }
        }


        void Update()
        {
            if (gameManager.gamePressed == false)
                return;

            if (shouldWaitAtStart && hasWaited == false)
            {
                if (isWaiting == false)
                    StartCoroutine(StartGame());

                return;
            }

            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }
        private IEnumerator StartGame()
        {
            isWaiting = true;

            yield return new WaitForSeconds(waitTime);

            hasWaited = true;
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}