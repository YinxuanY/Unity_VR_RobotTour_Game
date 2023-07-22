using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(AspectRatioFitter))]
public class CameraImage : MonoBehaviour
{

    private WebCamTexture cameraTexture;
    private Image image;
    private AspectRatioFitter fit;

    void Start()
    {
        // Use AspectRatioFitter's 'Aspect Mode' to control the layout
        // 'Height Controls Width' works well for portrait smartphone app
        fit = GetComponent<AspectRatioFitter>();
        image = GetComponent<Image>();
        cameraTexture = new WebCamTexture();
        image.material.mainTexture = cameraTexture;
        cameraTexture.Play();
    }

    private void Update()
    {

        // Fix ratio issues
        float ratio = 1f * cameraTexture.width / cameraTexture.height;
        fit.aspectRatio = ratio;

        // Fix scaling issues
        float scaleY = cameraTexture.videoVerticallyMirrored ? -1f : 1f;
        image.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        // Fix orientation issues
        int orient = -cameraTexture.videoRotationAngle;
        image.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
}