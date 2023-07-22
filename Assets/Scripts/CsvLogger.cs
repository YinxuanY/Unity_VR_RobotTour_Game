using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CsvLogger : MonoBehaviour
{
    // Start is called before the first frame update
    public string filepath;

    public List<Transform> tracklist = new List<Transform>();
    [DebugGUIGraph(min: 0, max: 1, r: 0, g: 1, b: 0, autoScale: true)]
    public float grip;
    public float Forwardspeed;
    public float SidewaysSpeed;
    public float Speed;
    protected Transform tracked;

    public void SaveTabletData(int[] responses)
    {
        var s = filepath.Split('.');
        var fpath = s[0] + "_Tablet+" + "." + s[1];

        string output = Time.time.ToString() + ", ";
        //string output = "";
        foreach (int r in responses)
        {
            output += r.ToString() + ",";
        }

        output += System.Environment.NewLine;

        if (!File.Exists(fpath))
            File.WriteAllText(fpath, output);
        else
            File.AppendAllText(fpath, output);
    }

    void Start()
    {
        var playerTransform = GameObject.Find("SceneController").GetComponent<SceneController>().GetPlayerTransform();
        var robotTransform = GameObject.Find("SceneController").GetComponent<SceneController>().GetRobotTransform();
        var mollikTransform = GameObject.Find("SceneController").GetComponent<SceneController>().GetMollikTransform();

        // if VR player
        var playerLeftHandTransform = playerTransform.Find("SteamVRObjects/LeftHand");
        if (playerLeftHandTransform != null)
        {
            var playerRightHandTransform = playerTransform.Find("SteamVRObjects/RightHand").transform;
            var playerHeadTransform = playerTransform.Find("SteamVRObjects/VRCamera").transform;
            tracklist.Add(playerLeftHandTransform.transform);
            tracklist.Add(playerRightHandTransform);
            tracklist.Add(playerHeadTransform);
        }

        // non VR player and VR player
        tracklist.Add(playerTransform);
        tracklist.Add(robotTransform);
        tracklist.Add(mollikTransform);

        if (File.Exists(filepath))
            File.Delete(filepath);

        var s = filepath.Split('.');
        filepath = s[0] + "_" + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "." + s[1];

        //DebugGUI.ForceReinitializeAttributes();

    }

    void Update()
    {
        //float[] output = new float[];
        string output = null;
        int temp = tracklist.Count;
        //Debug.Log("the list contains " + temp.ToString() + " items.");
        //for (int i = 0; i < temp - 1; i++)
        output = output + Time.time.ToString() + ", ";
        foreach (Transform i in tracklist)
        {
            //Debug.Log("looking at item " + i.ToString() + " called " + i.name.ToString());
            output = output + i.name.ToString() + ", " +
                i.position.x.ToString() + ", " +
                i.position.y.ToString() + ", " +
                i.position.z.ToString() + ", " +
                i.rotation.eulerAngles.x.ToString() + ", " +
                i.rotation.eulerAngles.y.ToString() + ", " +
                i.rotation.eulerAngles.z.ToString() + ", ";
        }
        //output = output + GameObject.Find("SceneController").GetComponent<GripTracker>().getGrip();
        var playerTransform = GameObject.Find("SceneController").GetComponent<SceneController>().GetPlayerTransform();
        //if VR Player
        var playerLeftHandTransform = playerTransform.Find("SteamVRObjects/LeftHand");
        if (playerLeftHandTransform != null)
        {
            output = output + "Grip: ," + playerTransform.GetComponent<GripTracker>().getGrip() + ", ";
            output = output + "ForwardSpeed: ," + playerTransform.GetComponent<PlayerMovement>().getForwardSpeed() + ", ";
            output = output + "SidewaySpeed: ," + playerTransform.GetComponent<PlayerMovement>().getSidewaysSpeed() + ", ";
            output = output + "Speed: ," + playerTransform.GetComponent<PlayerMovement>().getSpeed();
            grip = playerTransform.GetComponent<GripTracker>().getGrip();
            Forwardspeed = playerTransform.GetComponent<PlayerMovement>().getForwardSpeed();
            SidewaysSpeed = playerTransform.GetComponent<PlayerMovement>().getSidewaysSpeed();
            Speed = playerTransform.GetComponent<PlayerMovement>().getSpeed();

        }
        output = output + "\n";

        if (filepath != "")
        {
            if (!File.Exists(filepath))
                File.WriteAllText(filepath, output);
            else
                File.AppendAllText(filepath, output);
        }
    }
}
