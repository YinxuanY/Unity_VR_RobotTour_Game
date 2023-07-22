using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private bool triggered;
    private bool disableExit;

    private void Awake()
    {
        triggered = false;
        disableExit = true;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (disableExit)
        {
            //UnityEngine.Debug.Log("Collide");
            return;
        }

        //Debug.Log(other.name+" "+other.tag);
        if (other.CompareTag("Player") || other.name == "HeadCollider") 
        {
            if (!triggered)
            {
                triggered = true;
                GoToNextScene();
            }

        }
    }

    public void GoToNextScene()
    {
        //UnityEngine.Debug.Log("next scene");

        var sc = GameObject.Find("SceneController");
        var script = sc.GetComponent<SceneController>();
        script.GoToNextScene();
        //SceneManager.LoadScene("Wagner 3", LoadSceneMode.Additive);
        //SceneManager.UnloadScene("Wagner 1");
    }

    public void DisableExit()
    {
        disableExit = true;
    }

    public void EnableExit()
    {
        disableExit = false;
    }

}
