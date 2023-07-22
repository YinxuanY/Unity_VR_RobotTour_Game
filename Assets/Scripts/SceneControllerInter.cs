using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControllerInter : SceneController
{

    public List<Transform> tourWaypoints = new List<Transform>();
    private RobotAI robotAI;

    // Start is called before the first frame update
    void Start()
    {
        try {
            WebCamTexture webcam = GameObject.Find("Image").GetComponent<PlayMovieTextureOnUI>().getWebCamTexture();
            if (webcam != null)
                webcam.Play();
        }
        catch
        {
            Debug.Log("NO WEBCAM");
        }
        
        Debug.Log("Starting Intermediate Scene");
        MovePlayerToPosition(playerSpawn.position);

        robot = LoadRobot("Robot", robotSpawn.position);
        robotAI = robot.GetComponent<RobotAI>();
        Debug.Log("Sending waypoints to robot " + robotAI);
        robotAI.SetWaypoints(tourWaypoints);

        PlaySounds(backgroundSounds);
        DisableExits();
        DebugGUI.ForceReinitializeAttributes();

    }

    // Update is called once per frame
    void Update()
    {
        if (robotAI.GetCurrentWaypointIdx() == 8)
        {
            EnableExits();
        }
    }

}
