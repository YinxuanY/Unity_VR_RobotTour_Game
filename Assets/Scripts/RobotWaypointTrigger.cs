using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RogoDigital.Lipsync;
using System.Diagnostics;
//using UnityEngine.Events;

public class RobotWaypointTrigger : MonoBehaviour
{
    //public UnityEvent onTrigger; 
    private bool triggered;
    public AudioSource audioSource;
    public LipSyncData Lips;
    public LipSync characterMollik;
    Collider collider;
    private GameObject robot;
    private Animator ani;


    public enum Detect
    {
        Player,
        Shooter,
        Victim,
        Robot,
        Manual
    }

    public Detect type;

    void Awake()
    {
        triggered = false;
        collider = GetComponent<Collider>();
    }

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        robot = GameObject.Find("Robot");
        ani = robot.GetComponent<Animator>();
    }

    public bool IsTriggered()
    {
        return triggered;
    }

    public void Trigger()
    {
        triggered = true;
        UnityEngine.Debug.Log("triggered waypoint: "+transform.name);
        
    }

    public void ResetTrigger()
    {
        triggered = false;
    }

    public void DestroyCollider()
    {
        Destroy(collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("this is the tag: " + other.tag);
        //onTrigger.Invoke();
        switch (type)
        {
            case Detect.Manual:
                return;
            
            case Detect.Player:
                if (other.tag != "Player")
                    return;
            break;
            case Detect.Shooter:
                if (other.tag != "Shooter")
                    return;
                break;
            case Detect.Robot:
                if (other.tag != "Robot")
                    return;
                break;
            case Detect.Victim:
                if (other.tag != "Victim")
                    return;
                break;
        }

        UnityEngine.Debug.Log(other.name + " triggered " + gameObject.name);
        triggered = true;
       // ani.SetBool("ContinuousPoint", true);
        audioSource.Play();
        characterMollik.Play(Lips);
    }


}
