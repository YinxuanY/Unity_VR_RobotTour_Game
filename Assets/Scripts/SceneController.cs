using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.ComponentModel;
using System.Diagnostics;

public class SceneController : MonoBehaviour
{
    protected bool disableVR;
    protected bool isAndi;

    protected PlayerSingleton player;
    protected ControlPanelSingleton controlPanel;
    protected GameOptions gameOptions;
    public Transform playerSpawn;

    public Transform robotSpawn;
    public Transform mollikSpawn;
    protected GameObject robot;
    protected GameObject mollik;

    public List<AudioSource> backgroundSounds;

    private bool sceneLoading = false;

    public bool saveTransformData = false;
    public List<Transform> trackedTransforms;

    private CsvLogger dataLogger;

    protected AudioMixer audioMixer;

    public List<GameObject> exits;

    void Awake()
    {
        controlPanel = ControlPanelSingleton.Instance;
        gameOptions = controlPanel.GetComponent<GameOptions>();

        isAndi = gameOptions.isAndi;
        disableVR = gameOptions.disableVR;

        gameObject.AddComponent<CsvLogger>();
        dataLogger = gameObject.GetComponent<CsvLogger>();
        dataLogger.filepath = gameOptions.datafilePath;

        if (saveTransformData)
        {
            dataLogger.tracklist = trackedTransforms;
        }

        // Initialize player
        player = PlayerSingleton.Instance;
        // Initialize control panel
        FadePlayerScreenIn();

        audioMixer = Resources.Load<AudioMixer>("AudioMixer");
        UnmuteMasterVolume();


        DisableExits();
    }

    protected GameObject LoadRobot(string name, Vector3 pos)
    {
        bool isAndi = GameObject.Find("SceneController").GetComponent<SceneController>().isAndi;

        string path;
        if (isAndi == true)
            path = "RobotAndi";
        else
            path = "RobotEmergy";

        var prefab = Resources.Load<GameObject>(path);
        UnityEngine.Debug.Log("loaded " + path);
        // create the prefab in your scene
        var inScene = Instantiate<GameObject>(prefab, pos, Quaternion.identity);
        inScene.name = name;
        inScene.tag = "Robot";
        inScene.GetComponent<RobotAI>().isAndi = isAndi;
        //inScene.GetComponent<NavMeshAgent>().Warp(pos); // cause navmesh can be weird
        inScene.SetActive(true);

        // try find the instance inside the prefab
        //_Instance = inScene.GetComponentInChildren<PlayerSingleton>();
        // guess there isn't one, add one
        //if (!_Instance) _Instance = inScene.AddComponent<PlayerSingleton>();
        // mark root as DontDestroyOnLoad();
        //DontDestroyOnLoad(_Instance.transform.root.gameObject);

        return inScene;
    }

    protected GameObject LoadMollik(string name, Vector3 pos)
    {
        string path = "RobotMollik";

        var prefab = Resources.Load<GameObject>(path);
        UnityEngine.Debug.Log("loaded " + path);
        // create the prefab in your scene
        var inScene = Instantiate<GameObject>(prefab, pos, Quaternion.identity);
        inScene.name = name;
        inScene.tag = "Mollik";
        //inScene.GetComponent<RobotAI>().isAndi = isAndi;
        //inScene.GetComponent<NavMeshAgent>().Warp(pos); // cause navmesh can be weird
        inScene.SetActive(true);

        return inScene;
    }

    public void DisableExits()
    {
        foreach (var e in exits)
        {
            e.GetComponent<ExitTrigger>().DisableExit();
        }
    }

    public void EnableExits()
    {
        foreach (var e in exits)
        {
            UnityEngine.Debug.Log("Enable trigger");

            e.GetComponent<ExitTrigger>().EnableExit();
        }
    }

    public Transform GetPlayerTransform()
    {
        //Debug.Log(player.transform.position);
        return player.transform;
    }

    public Transform GetRobotTransform()
    {
        return robot.transform;
    }

    public Transform GetMollikTransform()
    {
        return mollik.transform;
    }

    protected void StopSounds(List<AudioSource> sounds)
    {
        foreach (var s in sounds)
            s.Stop();
    }

    protected void PlaySounds(List<AudioSource> sounds)
    {
        foreach (var s in sounds)
            s.Play();
    }

    protected void PlaySoundsDelayed(List<AudioSource> sounds)
    {
        foreach (var s in sounds)
            s.PlayDelayed(5);
    }


    void Update()
    {

    }
    public bool IsVRDisabled()
    {
        return disableVR;
    }

    public bool IsRobotAndi()
    {
        return isAndi;
    }

    IEnumerator GoToSceneAsyncRoutine(string sceneName)
    {
        if (sceneLoading)
        {
            UnityEngine.Debug.Log("Scene already loading");
            yield break;
        }

        var fadeTime = player.transform.GetComponent<FadeScreen>().fadeDuration;

        FadePlayerScreenOut();

        MuteMasterVolume();

        //if(disableVR)
        yield return new WaitForSeconds(fadeTime); // wait till fade is complete to prevent stuttering

        //Launch the new scene

        sceneLoading = true;
        UnityEngine.AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                sceneLoading = false;
                operation.allowSceneActivation = true;
            }
            yield return 0;
        }
        //WebCamTexture webcam = GameObject.Find("Image").GetComponent<PlayMovieTextureOnUI>().getWebCamTexture();
        //if (webcam!=null)
        //    webcam.Play();
        //yield break;

    }

    public void GoToNextScene()
    {
        //WebCamTexture webcam = GameObject.Find("Image").GetComponent<PlayMovieTextureOnUI>().getWebCamTexture();
        //webcam.Stop();
        string currentScene = SceneManager.GetActiveScene().name;
        UnityEngine.Debug.Log(currentScene);

        //if (!disableVR)
        //{
        //    Debug.Log("VR LOAD" + SceneManager.GetSceneByBuildIndex(idx).name);
        //    Valve.VR.SteamVR_LoadLevel.Begin(SceneManager.GetSceneByBuildIndex(idx).name);
        //}
        //else
        StartCoroutine(GoToSceneAsyncRoutine(controlPanel.GetComponent<GameOptions>().nextSceneNameDict[currentScene]));
        //UnityEngine.Debug.Log("Loading scene....");
    }


    protected void MovePlayerToPosition(Vector3 pos)
    {
        if (player.name == "Player")
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            cc.enabled = false;
            player.transform.position = pos;
            cc.enabled = true;
        }
        else
        {
            player.transform.position = pos;
        }
    }

    protected void FadePlayerScreenOut()
    {
        if (!disableVR)
        {
            Valve.VR.SteamVR_Fade.View(Color.clear, 0f);
            Valve.VR.SteamVR_Fade.View(Color.white, 1.0f);
        }
        else
            player.transform.GetComponent<FadeScreen>().FadeOut();
    }

    protected void FadePlayerScreenIn()
    {
        if (!disableVR)
        {
            Valve.VR.SteamVR_Fade.View(Color.white, 0f);
            Valve.VR.SteamVR_Fade.View(Color.clear, 1.0f);
        }
        else
            player.transform.GetComponent<FadeScreen>().FadeIn();
    }

    public void ToggleControlPanel()
    {
        controlPanel.GetComponentInChildren<Camera>().enabled = !controlPanel.GetComponentInChildren<Camera>().enabled;
    }

    public void SaveTabletData(int[] data)
    {
        dataLogger.SaveTabletData(data);
    }

    public void MuteMasterVolume()
    {
        audioMixer.SetFloat("MasterVolume", -80);
    }
    public void UnmuteMasterVolume()
    {
        audioMixer.SetFloat("MasterVolume", 0);
    }
}


