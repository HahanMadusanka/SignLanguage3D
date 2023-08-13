using System;
using System.IO;
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
    private string videoFilePath;

    // Start is called before the first frame update
    void Start()
    {
        defaultBackground = background.texture;
        videoFilePath = Path.Combine(Application.persistentDataPath, "recordedVideo.mp4");

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

    public void SaveRecordedVideo()
    {
        if (frontCam == null || !frontCam.isPlaying)
        {
            Debug.Log("No video to save.");
            return;
        }

        // Convert WebCamTexture to a Texture2D
        Texture2D videoTexture = new Texture2D(frontCam.width, frontCam.height);
        videoTexture.SetPixels(frontCam.GetPixels());
        videoTexture.Apply();

        // Encode the video to bytes (MP4 format)
        byte[] videoBytes = videoTexture.EncodeToJPG();

        // Save video to the persistent data path
        string savePath = Path.Combine(Application.persistentDataPath, "recordedVideo.mp4");
        File.WriteAllBytes(savePath, videoBytes);
        Debug.Log("Video saved to: " + savePath);

    }


}
