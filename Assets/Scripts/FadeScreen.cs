using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public GameObject faderObj;
    public bool fadeOnStart = true;
    public float fadeDuration = 2;
    public Color fadeColor;

    

    // Start is called before the first frame update
    void Start()
    {
        if (fadeOnStart)
            FadeIn();
    }

    public void FadeIn()
    {
        Debug.Log("Fading in");
        Fade(1, 0);
    }
    
    public void FadeOut()
    {
        Fade(0, 1);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn,alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn,float alphaOut)
    {
        float timer = 0;
        MeshRenderer meshRend = faderObj.GetComponent<MeshRenderer>();
        var startColor = fadeColor;
        var endColor = fadeColor;
        startColor.a = alphaIn;
        endColor.a = alphaOut;
        while (timer <= fadeDuration)
        {
            
            faderObj.GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, timer/fadeDuration);
            //Debug.Log(faderObj.GetComponent<Renderer>().material.color.a);
            timer += Time.deltaTime;
            yield return null;
        }

    }
}
