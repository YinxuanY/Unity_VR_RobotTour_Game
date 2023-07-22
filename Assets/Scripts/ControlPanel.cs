using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlPanel : MonoBehaviour
{
    public Dropdown sceneList;
    public Dropdown robotType;
    public Dropdown sceneOrder;
    public Dropdown webCam;
    public Toggle disableVR;
    public Button startButton;
    public InputField filePath;
    public InputField arduinoPort;
    public TextMeshProUGUI TimeStamp;
    private List<string> camOptions;
    // Start is called before the first frame update
    void Start()
    {
        InitMenu();
    }

    public void InitMenu()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        camOptions = new List<string>();
        // for debugging purposes, prints available devices to the console
        for (int i = 0; i < devices.Length; i++)
        {
            print("Webcam available: " + devices[i].name);
            camOptions.Add(devices[i].name);
        }
        //webCam = webCam.GetComponent<Dropdown>();
        webCam.ClearOptions();
        foreach (string option in camOptions)
        {
            webCam.options.Add(new Dropdown.OptionData(option));
            print("adding " + option + " to dropdown");
        }
        //webCam.AddOptions(camOptions);
        webCam.value = 0;
        webCam.RefreshShownValue();
    }

    public void StartButton()
    {
        // create a gameobject to store options that will be persistent throughout game
        var options = gameObject.GetComponent<GameOptions>();

        options.disableVR = disableVR.isOn;
        if (robotType.options[robotType.value].text == "Andi")
            options.isAndi = true;
        else
            options.isAndi = false;

        var startScene = sceneList.options[sceneList.value].text;
        var sceneO = sceneOrder.options[sceneOrder.value].text;

        options.nextSceneNameDict.Clear();

        if(startScene == "Wagner" && sceneO == "Shooter Scenario")
        {
            options.nextSceneNameDict.Add("Wagner", "Intermediate");
            options.nextSceneNameDict.Add("Intermediate","Standard_School");
            options.nextSceneNameDict.Add("Standard_School", "WagnerLab_End");
            //options.nextSceneNameDict.Add("Standard_School","recroom");
            //options.nextSceneNameDict.Add("recroom","Standard_Office");
            //options.nextSceneNameDict.Add("Standard_Office","WagnerLab_End");
        }
        else if (startScene == "evolab5e" && sceneO == "Shooter Scenario")
        {
            options.nextSceneNameDict.Add("evolab5e", "Intermediate");
            options.nextSceneNameDict.Add("Intermediate", "Standard_School");
            options.nextSceneNameDict.Add("Standard_School", "Evolab_End");
            //options.nextSceneNameDict.Add("Standard_School", "recroom");
            //options.nextSceneNameDict.Add("recroom", "Standard_Office");
            //options.nextSceneNameDict.Add("Standard_Office", "EvoLab_End");
        }
        else if (startScene == "Wagner" && sceneO == "Fire Scenario")
        {
            options.nextSceneNameDict.Add("Wagner", "Intermediate");
            options.nextSceneNameDict.Add("Intermediate", "Standard_Office");
            options.nextSceneNameDict.Add("Standard_Office", "WagnerLab_End");
            //options.nextSceneNameDict.Add("Standard_Office", "recroom");
            //options.nextSceneNameDict.Add("recroom", "Standard_School");
            //options.nextSceneNameDict.Add("Standard_School", "WagnerLab_End");
        }
        else if (startScene == "evolab5e" && sceneO == "Fire Scenario")
        {
            options.nextSceneNameDict.Add("evolab5e", "Intermediate");
            options.nextSceneNameDict.Add("Intermediate", "Standard_Office");
            options.nextSceneNameDict.Add("Standard_Office", "Evolab_End");
            //options.nextSceneNameDict.Add("Standard_Office", "recroom");
            //options.nextSceneNameDict.Add("recroom", "Standard_School");
            //options.nextSceneNameDict.Add("Standard_School", "EvoLab_End");
        }
        else
        {
            options.nextSceneNameDict.Add("evolab5e", "Intermediate");
            options.nextSceneNameDict.Add("Intermediate", "Standard_School");
            options.nextSceneNameDict.Add("Standard_School", "Evolab_End");
            options.nextSceneNameDict.Add("Standard_Office", "Evolab_End");
            //options.nextSceneNameDict.Add("Standard_School", "recroom");
            //options.nextSceneNameDict.Add("recroom", "Standard_Office");
            //options.nextSceneNameDict.Add("Standard_Office", "WagnerLab_End");
        }

        options.datafilePath = filePath.text;
        options.arduinoPort = arduinoPort.text;
        WebCamTexture webcam = GameObject.Find("Image").GetComponent<PlayMovieTextureOnUI>().getWebCamTexture();
        webcam.Stop();
        UnityEngine.SceneManagement.SceneManager.LoadScene(startScene);
    }
    // Update is called once per frame
    void Update()
    {
        TimeStamp.SetText(Time.time.ToString());
    }
}
