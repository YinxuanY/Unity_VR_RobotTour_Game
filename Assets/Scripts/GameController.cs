using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{

    void Awake()
    {

    }

    void Start()
    {

    }

    IEnumerator GoToSceneAsyncRoutine(int sceneIndex)
    {
        //fadeScreen.FadeOut();
        //Launch the new scene

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        //operation.allowSceneActivation = false;
        /*
        float timer = 0;
        while (timer <= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }*/
        while (!operation.isDone)
        {
            yield return 0;
        }

        //operation.allowSceneActivation = true;

    }

    public void LoadNextScene()
    {
        StartCoroutine(GoToSceneAsyncRoutine(SceneManager.GetActiveScene().buildIndex + 1));
    }

    void Update()
    {
    }

}
