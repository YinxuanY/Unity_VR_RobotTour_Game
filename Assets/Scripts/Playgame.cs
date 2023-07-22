using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Playgame : MonoBehaviour
{
    public InputField par;
/*    public InputField ses;*/
    public TMPro.TextMeshProUGUI par_reminder;
/*    public TMPro.TextMeshProUGUI ses_reminder;*/
    public TMPro.TextMeshProUGUI loading;
    public Dropdown scenario;
    public Button playButton;

    public void play()
    {
        string par_num = par.text;
/*        string ses_num = ses.text;*/
        if (par_num.Equals(""))
        {
            par_reminder.gameObject.SetActive(true);
/*            ses_reminder.gameObject.SetActive(true);*/
        }
/*        else if (par_num.Equals(""))
        {
            par_reminder.gameObject.SetActive(true);
*//*            ses_reminder.gameObject.SetActive(false);*//*
        }
        else if (ses_num.Equals(""))
        {
            par_reminder.gameObject.SetActive(false);
            ses_reminder.gameObject.SetActive(true);
        }*/
        else
        {
            playButton.gameObject.SetActive(false);
            par_reminder.gameObject.SetActive(false);
/*            ses_reminder.gameObject.SetActive(false);*/
            loading.gameObject.SetActive(true);
            int sce = scenario.value + 1;

            StartCoroutine(SendTextToFile());
            PlayerPrefs.SetString("parID", par.text);
/*            PlayerPrefs.SetString("sesID", "s" + ses.text);*/

            SceneManager.LoadScene(sce);
        }
    }

    IEnumerator SendTextToFile()
    {
        WWWForm form = new WWWForm();
        form.AddField("parID", par.text);
/*        form.AddField("sesID", "s" + ses.text);*/
        form.AddField("xposition", "xposition,");
        form.AddField("yposition", "yposition,");
        form.AddField("zposition", "zposition,");
        form.AddField("elapsedtime", "elapsedtime");
        WWW www = new WWW("https://emergencytraining.azurewebsites.net/fromUnity.php", form);
        yield return www;
    }

}
