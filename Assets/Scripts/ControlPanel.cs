using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.Networking;
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
    public GameObject TypingInputPopup;


    [Header("TextMeshProUGUI")]
    public TextMeshProUGUI debugLog;

    private string processingMode = "english"; // "english" , "sinhala", "camera"

    // Language codes for English and Sinhala
    private const string EnglishLanguageCode = "en-US";
    private const string SinhalaLanguageCode = "si-LK";

    private string FilePath;

    private string URL = "http://4368-123-231-127-112.ngrok.io/predict/en";

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

    public void openType(string lnaguage)
    {
        TypingInputPopup.SetActive(true);
    }
        
    public void okType()
    {
        TypingInputPopup.SetActive(false);
        StartCoroutine(getData());
    }

    public void LoadFile()
    {
        string FileType = NativeFilePicker.ConvertExtensionToFileType("mp4");

        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                Debug.Log("path is null");
            }
            else
            {
                FilePath = path;
                Debug.Log("path is "+path);
            }
        },new string[] { FileType});
    }

    #region API call
    IEnumerator getData()
    {/*
        using(UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError)
            {
                debugLog.text = "Error = " + request.error;
            }
            else
            {
                string json = request.downloadHandler.text;
            //    SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);

              //  debugLog.text = " result " + stats[0]["result"];
            }
        }*/

        string jsonPayload = "{\"sentence\":\"mother come\"}";


        using (UnityWebRequest request = new UnityWebRequest(URL, "POST"))
        {
            byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                debugLog.text = "Error = " + request.error;
            }
            else
            {
                string json = request.downloadHandler.text;
                SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);
                Debug.Log("stats " + json);
                Debug.Log("json " + json);
                Dictionary<string, List<string>> data = JsonUtility.FromJson<Dictionary<string, List<string>>>(json);
                Debug.Log("1 " + data);
            /*
                    string[] resultArray = stats["result"];
                    Debug.Log("2 " + resultValues);
                    // Convert list to array if needed
                    //string[] resultArray = resultValues.ToArray();
                    Debug.Log("3 " + resultArray);
                    // Now you can use the values in resultArray
                    foreach (string value in resultArray)
                    {
                        Debug.Log("Value: " + value);
                    }*/
             }
            
        }
    }
    #endregion
}
