using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControllerRec : SceneController
{
    public List<Transform> tourWaypoints = new List<Transform>();
    private RobotAI robotAI;

    private bool tourComplete;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting RecRoom Scene");
        MovePlayerToPosition(playerSpawn.position);

        robot = LoadRobot("Robot", robotSpawn.position);
        robotAI = robot.GetComponent<RobotAI>();
        Debug.Log("Sending waypoints to robot " + robotAI);
        robotAI.SetWaypoints(tourWaypoints);

        EnableExits();
    }
    // Update is called once per frame
    void Update()
    {
       
        
    }
}
