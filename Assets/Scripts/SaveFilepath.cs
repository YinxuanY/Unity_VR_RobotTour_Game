using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveFilepath : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField inputField;
    public void OnButtonPress()
    {
        var path = System.IO.Path.Combine(Application.dataPath, inputField.text+".csv");
        inputField.text = path;
    }
}
