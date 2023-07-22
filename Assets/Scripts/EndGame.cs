using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private string parID;
    private string sesID;
    private bool first;

    // Start is called before the first frame update
    void Start()
    {
        first = true;
        parID = PlayerPrefs.GetString("parID");
/*        sesID = PlayerPrefs.GetString("sesID");*/
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad >= 100 && first == true)
        {
            first = false;
            StartCoroutine(SendTextToFile());
            SceneManager.LoadScene(2);
        }
    }

    IEnumerator SendTextToFile()
    {
        WWWForm form = new WWWForm();
        form.AddField("end", "true");
        form.AddField("parID", parID);
        WWW www = new WWW("https://emergencytraining.azurewebsites.net/fromUnity.php", form);
        yield return www;
    }
}
