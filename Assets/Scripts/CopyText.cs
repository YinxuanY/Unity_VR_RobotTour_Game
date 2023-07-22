using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CopyText : MonoBehaviour
{
	public InputField input;
	public Text output;
	public ToggleGroup toggles;

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update ()
    {
		Toggle theActiveToggle = toggles.ActiveToggles().FirstOrDefault();

		if (theActiveToggle != null)
			Debug.Log(theActiveToggle.transform.Find("Label").GetComponent<Text>().text);
    }

	void DisplayText()
    {
		output.text = input.text;
    }
}
