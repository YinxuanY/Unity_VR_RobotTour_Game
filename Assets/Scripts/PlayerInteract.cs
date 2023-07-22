using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem;

public class PlayerInteract : MonoBehaviour
{
    public Camera camera;

    private RaycastHit hit;
    private Ray ray;

    LineRenderer lr;
    public float rayVisibleTime;
    private float pRayTime;

    private void Awake()
    {
        lr = gameObject.AddComponent<LineRenderer>();
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        rayVisibleTime = 1.0f;
        pRayTime = Time.time;
    }
    // Start is called before the first frame update
    void Start()
    {
    }



    // Update is called once per frame
    void Update()
    {
        if(Time.time - pRayTime > rayVisibleTime)
            lr.enabled = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

            lr.SetPosition(0, ray.origin+ ray.direction*0.1f);
            lr.SetPosition(1, ray.origin+ray.direction*4.0f);
            lr.enabled = true;
            pRayTime = Time.time;


            if (Physics.Raycast(ray, out hit, 4f, LayerMask.GetMask("Interactable")))
            {
                //if tablet
                if (hit.transform.gameObject.GetComponentInChildren<TabletUIBehaviour>())
                {
                    Debug.Log("Player submitted tablet: " + hit.transform.name);
                    hit.transform.gameObject.GetComponentInChildren<TabletUIBehaviour>()
                        .buttonPressed(hit.transform.gameObject.GetComponentInChildren<TabletUIBehaviour>().submitButton.name);
                }
                if (hit.transform.gameObject.GetComponent<InteractableHoverEvents>())
                {
                    Debug.Log("Player interacted with: "+hit.transform.name);
                    hit.transform.gameObject.GetComponent<InteractableHoverEvents>().onHandHoverBegin.Invoke();
                }

                if (hit.transform.gameObject.GetComponent<RobotWaypointTrigger>())
                {
                    Debug.Log("Player triggered: " + hit.transform.name);
                    hit.transform.gameObject.GetComponent<RobotWaypointTrigger>().Trigger();
                }
                
            }

        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            GameObject.Find("SceneController").GetComponent<SceneController>().ToggleControlPanel();
        }
    }
}
