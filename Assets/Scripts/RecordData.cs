using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordData : MonoBehaviour
{
    private string parID;
    private string sesID;
    float t = 0;

    // Start is called before the first frame update
    void Start()
    {
        parID = PlayerPrefs.GetString("parID");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad -t > 0.5f)
        {
            float x = this.transform.position.x;
            float y = this.transform.position.y;
            float z = this.transform.position.z;
            float elapsedtime = Time.timeSinceLevelLoad;
            StartCoroutine(SendTextToFile(x.ToString(), y.ToString(), z.ToString(), elapsedtime.ToString()));
            t = elapsedtime;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(SendCrouchToFile("Just crouched."));
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(SendStandToFile("Just stand up."));
        }
    }

    IEnumerator SendTextToFile(string x, string y, string z, string elapsedtime)
    {
        WWWForm form = new WWWForm();
        form.AddField("parID", parID);
        form.AddField("xposition", x + ",");
        form.AddField("yposition", y + ",");
        form.AddField("zposition", z + ",");
        form.AddField("elapsedtime", elapsedtime);
        WWW www = new WWW("https://emergencytraining.azurewebsites.net/fromUnity.php", form);
        yield return www;
    }

    IEnumerator SendCrouchToFile(string s)
    {
        WWWForm form = new WWWForm();
        form.AddField("parID", parID);
        form.AddField("crouch", s);
        WWW www = new WWW("https://emergencytraining.azurewebsites.net/fromUnity.php", form);
        yield return www;
    }

    IEnumerator SendStandToFile(string s)
    {
        WWWForm form = new WWWForm();
        form.AddField("parID", parID);
        form.AddField("stand", s);
        WWW www = new WWW("https://emergencytraining.azurewebsites.net/fromUnity.php", form);
        yield return www;
    }
}
