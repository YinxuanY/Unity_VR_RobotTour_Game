using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelSingleton : MonoBehaviour
{

    private static ControlPanelSingleton _Instance;

    public static ControlPanelSingleton Instance
    {
        get
        {
            if (!_Instance)
            {
                string path = "ControlPanel";

                // NOTE: read docs to see directory requirements for Resources.Load!
                var prefab = Resources.Load<GameObject>(path);
                
                // create the prefab in your scene
                var inScene = Instantiate<GameObject>(prefab);
                // try find the instance inside the prefab
                _Instance = inScene.GetComponentInChildren<ControlPanelSingleton>();
                // guess there isn't one, add one
                if (!_Instance) _Instance = inScene.AddComponent<ControlPanelSingleton>();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.transform.root.gameObject);

                _Instance.name = path;
            }
            return _Instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
