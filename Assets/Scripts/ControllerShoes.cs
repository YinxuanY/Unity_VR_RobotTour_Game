using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControllerShoes : MonoBehaviour
{
    //public SteamVR_Action_Vector2 input;
    public SteamVR_Action_Vector2 moveAction;
    public float cybershoesSpeed = 5;
    private CharacterController characterController;
    public SteamVR_Input_Sources treadmillSource = SteamVR_Input_Sources.Treadmill;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //MOVEMENT
        
        float forwardInput = moveAction.GetAxis(treadmillSource).y;
        float sidewaysInput = moveAction.GetAxis(treadmillSource).x;
       
        if (Mathf.Abs(forwardInput) > 1e-3)
        {
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(sidewaysInput, 0, forwardInput));
            characterController.Move(cybershoesSpeed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up)- new Vector3(0, 9.81f, 0) * Time.deltaTime);
        }        
    }
}

