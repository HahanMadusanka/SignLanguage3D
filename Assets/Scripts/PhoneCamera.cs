using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    public GameObject cameraScreen;
    private bool CamAvailable;
    private WebCamTexture frontCam;
    private Texture defaultBackground;

    public RawImage background;
 //   public AspectRatioFitter fit;
    // Start is called before the first frame update
    void Start()
    {
        defaultBackground = background.texture;
        
    }

    // Update is called once per frame
    void Update()
    {
 
    }


    public void openCamera()
    {
        cameraScreen.SetActive(true);

        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera .");
            CamAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {
                frontCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (frontCam == null)
        {
            Debug.Log("No front camera .");
            return;
        }

        frontCam.Play();
        background.texture = frontCam;

        background.rectTransform.localScale = new Vector3(1.32f, -0.28f, 0.28f); // Set scaleY to 1
        background.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height); // Set size to match screen

        int orient = -frontCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
        Debug.Log("orient = " + orient);
        CamAvailable = true;
    }


    public void closeCamera()
    {
        cameraScreen.SetActive(false);

        if (frontCam != null && frontCam.isPlaying)
        {
            frontCam.Stop();
        }
    }
}
