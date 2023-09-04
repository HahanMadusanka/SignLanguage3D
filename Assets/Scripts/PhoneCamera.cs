using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    public GameObject cameraScreen;
    public GameObject controlPabel;
    public GameObject btnCamera;

    private WebCamTexture frontCam;
    private Texture defaultBackground;

    public RawImage background;

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
    }


    public void closeCamera()
    {
        cameraScreen.SetActive(false);

        if (frontCam != null && frontCam.isPlaying)
        {
            frontCam.Stop();
        }
    }

    public void SaveRecordedVideo()
    {
        StartCoroutine(Saveframes());
    }

    IEnumerator Saveframes()
    {
        float delayBetweenIterations = 2.0f;

        string[] framPaths = new string[10];
        btnCamera.GetComponent<Image>().sprite = Resources.Load<Sprite>("Button/recordingBtn");
        for (int i = 0; i<10; i++)
        {
            // Convert WebCamTexture to a Texture2D
            Texture2D videoTexture = new Texture2D(frontCam.width, frontCam.height);
            videoTexture.SetPixels(frontCam.GetPixels());
            videoTexture.Apply();

            // Encode the video to bytes (MP4 format)
            byte[] videoBytes = videoTexture.EncodeToJPG();

            // Save video to the persistent data path
            string savePath = Path.Combine(Application.persistentDataPath, "frame_"+i+".jpg");
            File.WriteAllBytes(savePath, videoBytes);
            framPaths[i] = savePath;
            Debug.Log("Image saved to: " + savePath);

            yield return new WaitForSeconds(delayBetweenIterations);
        }

        controlPabel.GetComponent<ControlPanel>().sendFrames(framPaths);
        btnCamera.GetComponent<Image>().sprite = Resources.Load<Sprite>("Button/stopRecordingBtn");
    }

}
