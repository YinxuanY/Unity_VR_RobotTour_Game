using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControllerEvolab : SceneController
{

    public List<Transform> tourWaypoints = new List<Transform>();
    private RobotAI robotAI;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            WebCamTexture webcam = GameObject.Find("Image").GetComponent<PlayMovieTextureOnUI>().getWebCamTexture();
            if (webcam != null)
                webcam.Play();
        }
        catch
        {
            Debug.Log("NO WEBCAM");
        }
        Debug.Log("Starting Evolab Scene");
        MovePlayerToPosition(playerSpawn.position);

        PlaySounds(backgroundSounds);

        robot = LoadRobot("Robot", robotSpawn.position);
        robotAI = robot.GetComponent<RobotAI>();
        Debug.Log("Sending waypoints to robot " + robotAI);
        robotAI.SetWaypoints(tourWaypoints);
        DebugGUI.ForceReinitializeAttributes();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
