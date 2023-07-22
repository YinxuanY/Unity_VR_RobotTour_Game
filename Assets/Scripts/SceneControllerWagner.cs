using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControllerWagner : SceneController
{

    public List<Transform> tourWaypoints = new List<Transform>();
    public List<Transform> Molliktour = new List<Transform>();
    private RobotAI robotAI;
    private RobotMollik robotMollik;
 
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
        Debug.Log("Starting PostSurvey");
        MovePlayerToPosition(playerSpawn.position);

        PlaySounds(backgroundSounds);

        robot = LoadRobot("Robot", robotSpawn.position);
        robotAI = robot.GetComponent<RobotAI>();
        Debug.Log("Sending waypoints to robot " + robotAI);
        robotAI.SetWaypoints(tourWaypoints);

        mollik = LoadMollik("Mollik", mollikSpawn.position);
        robotMollik = mollik.GetComponent<RobotMollik>();
        robotMollik.SetWaypoints(Molliktour);

        //EnableExits();
        DebugGUI.ForceReinitializeAttributes();
    }

    public Transform GetMollikTransform()
    {
        return robotMollik.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
