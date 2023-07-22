using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{

    private static PlayerSingleton _Instance;

    public static PlayerSingleton Instance
    {
        get
        {
            if (!_Instance)
            {
                string path;
                if (GameObject.Find("SceneController").GetComponent<SceneController>().IsVRDisabled() == true)
                    path = "Player";
                else
                    path = "PlayerVR";
                    
                // NOTE: read docs to see directory requirements for Resources.Load!
                var prefab = Resources.Load<GameObject>(path);
                
                // create the prefab in your scene
                var inScene = Instantiate<GameObject>(prefab);
                // try find the instance inside the prefab
                _Instance = inScene.GetComponentInChildren<PlayerSingleton>();
                // guess there isn't one, add one
                if (!_Instance) _Instance = inScene.AddComponent<PlayerSingleton>();
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
