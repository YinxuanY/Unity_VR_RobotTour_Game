using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMovieTextureOnUI : MonoBehaviour
{
    public RawImage rawimage;
    public Dropdown cameraSource;
    public static WebCamTexture webcamTexture;

    public WebCamTexture getWebCamTexture()
    {
        return webcamTexture;
    }
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        // for debugging purposes, prints available devices to the console
        for (int i = 0; i < devices.Length; i++)
        {
            print("Webcam available: " + devices[i].name);
        }
        //WebCamTexture webcamTexture = new WebCamTexture();
        //WebCamTexture webcamTexture = new WebCamTexture(devices[2].name);
        //WebCamTexture webcamTexture = new WebCamTexture("Microsoft® LifeCam HD-3000");
        WebCamTexture webcamTexture = new WebCamTexture("HD Webcam C615");
        
        rawimage.texture = webcamTexture;
        rawimage.material.mainTexture = webcamTexture;
        webcamTexture.Stop();
        webcamTexture.Play();
    }
    public void Selection(Dropdown selection)
    {
        //Debug.Log(Dropdown.options[Dropdown.value].text);
        webcamTexture.Stop();
        print("set to " + selection.options[selection.value].text); //itemText.ToString());
        webcamTexture = new WebCamTexture(selection.options[selection.value].text);//camera.itemText.ToString());
        rawimage.texture = webcamTexture;
        rawimage.material.mainTexture = webcamTexture;
        
        webcamTexture.Play();

    }
}