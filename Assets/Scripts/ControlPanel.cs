using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class ControlPanel : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject sinhalaSingView;
    public GameObject btnEnglish;
    public GameObject btnSinhala;
    public GameObject btnMic;
    public GameObject btnCamera;

    private string processingMode = "english"; // "english" , "sinhala", "camera"

    // Language codes for English and Sinhala
    private const string EnglishLanguageCode = "en-US";
    private const string SinhalaLanguageCode = "si-LK";

    private AndroidJavaObject speechRecognizer;
    // Start is called before the first frame update


    void Start()
    {
        // Initialize Android speech recognizer
        using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (var activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            speechRecognizer = new AndroidJavaObject("unity.SpeechRecognition", activityContext);
        } 

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void english(){
        sinhalaSingView.SetActive(false);
        Debug.Log("english");
     }

    public void sinhala() {
        sinhalaSingView.SetActive(true);
        Debug.Log("sinhala");
    }


    // Call this method to start listening for speech
    public void StartListening()
    {
                       
        // Set the language to English
        speechRecognizer.Call("setLanguage", EnglishLanguageCode);

        // Start listening for speech
        speechRecognizer.Call("startListening");

    }

    // Call this method to switch the language to Sinhala
    public void SwitchToSinhala()
    {
        // Set the language to Sinhala
        speechRecognizer.Call("setLanguage", SinhalaLanguageCode);
    }

    // Event handler for recognition results (called from Java)
    public void OnRecognitionResult(string result)
    {
        // Update the output text with the recognized speech
        //outputText.text = result;
         Debug.LogError(result);
    }

    private void OnDestroy()
    {
        // Release resources when the GameObject is destroyed
        if (speechRecognizer != null)
        {
            speechRecognizer.Call("destroy");
        }
    }

 }
