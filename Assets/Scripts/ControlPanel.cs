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
    public Animator mAnimator;

    [Header("GameObject")]
    public GameObject sinhalaSingView;
    public GameObject btnEnglish;
    public GameObject btnSinhala;
    public GameObject btnMic;
    public GameObject btnCamera; 
    public GameObject popupVideoSelection;

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

       // mAnimator = GetComponent<Animator>();

        if (mAnimator != null)
        {
            mAnimator.SetTrigger("TrIdeal");
        }
        else
        {
            Debug.Log("mAnimator null");
        }
    }

    // Update is called once per frame
    void Update()
    {

            // if (Input.GetKeyDown(KeyCode.0))




            //if (Input.GetKeyDown(KeyCode.r))
            //{
            //  mAnimator.SetTrigger("TrIdealR");
            //}
        
        
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


    public void videoSelectionPopup()
    {
        if (popupVideoSelection.activeSelf)
        {
            popupVideoSelection.SetActive(false);
        }
        else
        {
            popupVideoSelection.SetActive(true);
        }
       
    }
}
