using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    //Config
    [SerializeField] float pathSpeed = 2f;
    [SerializeField] WaveConfig waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWayPoints();
        transform.position = waypoints[waypointIndex].position;
    }

    // Update is called once per frame
    void Update()
    {
        // Manages all pathing behaviour and destroys gameObject upon completion
        PathingBehaviour();
    }

    private void PathingBehaviour()
    {
        if (waypointIndex <= waypoints.Count -1)
        {
            //Sets and stores waypoint position
            var targetPosition = waypoints[waypointIndex].position;
            //makes pathing speed framerate independent
            var pathDelta = pathSpeed * Time.deltaTime;
            // Moves towards the next waypoint
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, pathDelta);

            //Sets the next waypoint as the target once the current one has been reached
            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            // Destroys gameobject when pathing is complete
            Destroy(gameObject);
        }
    }
}
