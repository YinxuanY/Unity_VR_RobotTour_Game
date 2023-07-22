using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GripTracker : MonoBehaviour
{

    public SteamVR_Action_Single squeezeAction;
    private string filePath;
    private string csv;

    // Start is called before the first frame update
    void Start()
    {
        filePath = "E:\\OASM\\Assets";

    }

    // Update is called once per frame
    void Update()
    {
        if (squeezeAction.axis == 0.0f)
            return;

       // Debug.Log("Squeeze: " + squeezeAction.axis);


    }

    public float getGrip()
    {
        return squeezeAction.axis;
    } 
}
