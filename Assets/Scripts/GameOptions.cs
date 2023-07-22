using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions : MonoBehaviour
{
    public Dictionary<string, string> nextSceneNameDict;
    public bool isAndi;
    public bool disableVR;
    public string datafilePath;
    public string arduinoPort;

    public void Awake()
    {
        nextSceneNameDict = new Dictionary<string, string>();
        //Default
        //if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "evolab5e")
        //{
        //    nextSceneNameDict.Add("evolab5e", "Intermediate");
        //    nextSceneNameDict.Add("Standard_Office", "EvoLab_End");
        //}
        //else
        //{
        //    nextSceneNameDict.Add("Wagner 1", "Wagner 3");
        //    nextSceneNameDict.Add("Standard_Office", "WagnerLab_End");
        //}

        nextSceneNameDict.Add("WagnerOffice", "PostSurvey");
        //nextSceneNameDict.Add("Standard_School", "recroom");
        //nextSceneNameDict.Add("recroom", "Standard_Office");
        
        
        if(datafilePath == "")
            datafilePath = System.IO.Path.Combine(Application.dataPath,"defaultTrackedDataFile.csv");
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
