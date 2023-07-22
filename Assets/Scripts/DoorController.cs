using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorController : MonoBehaviour
{
    public bool slidingDoor = false;
    public bool doNotLock = false;
    public bool flippedRotation = false;
    public bool debug = false;

    private bool locked;
    private bool keepDoorOpen;
    private float keepOpenUntil;

    private Rigidbody rb;
    private HingeJoint hinge;

    private float minAngle, maxAngle, range;

    private int rotFactor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hinge = gameObject.GetComponent<HingeJoint>();
        JointLimits l = hinge.limits;

        minAngle = flippedRotation == true? l.max: l.min;
        maxAngle = flippedRotation == true ? l.min : l.max;
        range = Mathf.Abs(minAngle - maxAngle);

        rotFactor = flippedRotation == true ? -1 : 1;
    }

    void Start()
    {

    }

    void Update()
    {
        if (keepDoorOpen)
        {
            if (keepOpenUntil - Time.time > 0.25)
            {
                //Debug.Log(hinge.angle +" "+maxAngle+" "+Mathf.Abs(hinge.angle - maxAngle));
                if (Mathf.Abs(hinge.angle-minAngle) < range) //door closed
                {
                    if (debug)
                    {
                        //Debug.Log("Adding helping torque");
                    }

                    EnableSpring(false);
                    HelpingTorque(0.1f);
                }
            }
            else
            {
                //Debug.Log("Done helping");
                EnableSpring(true);
                keepDoorOpen = false;
            }
        }

        if (debug)
        {
           // Debug.Log(hinge.angle);
        }
    }

    void LockDoor(bool state)
    {/*
        if (state == true && !doNotLockThis)
        {
            //anim.SetBool("open", false);
            isLocked = true;
        }
        else
        {
            col1.enabled = true;
        }
        */
    }


    void OnCollisionEnter(Collision col)
    {

        //if (isLocked && col.gameObject.CompareTag("Shooter"))
        //    return;

    }

    public void HelpingTorque(float val)
    {
        rb.AddRelativeTorque(0, val * rotFactor, 0, ForceMode.Impulse);
    }

    public void EnableSpring(bool state)
    {
        hinge.useSpring = state;
    }


    public void OpenDoorForSeconds(float t)
    {
        keepDoorOpen = true;
        keepOpenUntil = Time.time + t;
    }

    public bool OpensTowardsMe(Vector3 pos){
        var v1 = pos - hinge.transform.position;
        var v2 = Vector3.right;
        var a = Vector3.SignedAngle(v1, v2, hinge.axis);
        a *= rotFactor;
        if(a > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
       
    }

    void OnTriggerEnter(Collider col)
    {
        //if (isClosing == true)
        //{
        //    return;
        //}
       
        //Debug.Log("TRIGGER"+""+gameObject.name);


        if (col.CompareTag("Shooter") || col.CompareTag("Victim") || col.CompareTag("Player"))
        {
            //isClosing = true;
            OpenDoorForSeconds(2);

            if (debug)
            {
                Debug.Log("Helping open door");
            }
        }
    }

    // Waiting time between the player/victims leave the collider and the door closes.
    IEnumerator doorLockWait(float timer)
    {
        yield return new WaitForSeconds(timer);
        //anim.SetBool("open", false);
        //isClosing = false;
    }
}
