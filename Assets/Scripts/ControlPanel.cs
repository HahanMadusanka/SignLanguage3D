using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using TextSpeech;
using TMPro;
using System;

public class ControlPanel : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject sinhalaSingView;
    public GameObject btnEnglish;
    public GameObject btnSinhala;
    public GameObject btnMic;
    public GameObject btnCamera;

    [Header("TextMeshProUGUI")]
    public TextMeshProUGUI debugLog;

    private string processingMode = "english"; // "english" , "sinhala", "camera"

    // Language codes for English and Sinhala
    private const string EnglishLanguageCode = "en-US";
    private const string SinhalaLanguageCode = "si-LK";


    void Start()
    {
        SpeechToText.Instance.onResultCallback = OnFinalSpeechResult;

#if UNITY_ANDROID
        CheckPermission();
#endif

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void english(){
        processingMode = "english";
        sinhalaSingView.SetActive(false);
        Debug.Log("english");

        StartListening(EnglishLanguageCode);
     }

    public void sinhala() {
        processingMode = "sinhala";
        sinhalaSingView.SetActive(true);
        Debug.Log("sinhala");

        StartListening(SinhalaLanguageCode);
    }


    void CheckPermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }

    }


    #region Speach to Text
    private void StartListening(string code)
    {
        SpeechToText.Instance.Setting(code);
        SpeechToText.Instance.StartRecording();
    }

    void OnFinalSpeechResult(string result)
    {
        debugLog.text = result;
    }
    #endregion

}
