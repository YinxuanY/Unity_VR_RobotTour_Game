using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBehaviour : MonoBehaviour
{
    Toggle toggle;
  
    private void Awake()
    {
        toggle = gameObject.GetComponent<Toggle>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        toggle.isOn = !toggle.isOn;
    }
}
