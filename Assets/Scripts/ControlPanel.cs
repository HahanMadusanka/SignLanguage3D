using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using UnityEngine.Networking;
using TextSpeech;
using TMPro;
using System;
using System.IO;

[System.Serializable]
public class JSONData
{
    public string[] result;
}

public class JSONDataVideo
{
    public string result;
}

public class ControlPanel : MonoBehaviour
{
    public Animator mAnimator;

    [Header("GameObject")]
    public GameObject sinhalaSingView;
    public GameObject btnEnglish;
    public GameObject btnSinhala;
  
    public GameObject popupVideoSelection;
    public GameObject TypingInputPopup;
    public GameObject ShowTextPopup;
    public GameObject SettingsPopup;

    [Header("TextMeshProUGUI")]
    public TextMeshProUGUI debugLog;
    public TextMeshProUGUI ShowTextPopupText;

    [Header("TMP_InputField")]
    public TMP_InputField inputTextEnglish;
    public TMP_InputField inputTextSinhala;
    public TMP_InputField urlText;

    private string processingMode = "english"; // "english" , "sinhala", "camera"

    // Language codes for English and Sinhala
    private const string EnglishLanguageCode = "en-US";
    private const string SinhalaLanguageCode = "si-LK";

    private string FilePath;

    private string URL = "http://b87e-2407-c00-c003-b66f-c1fd-e253-9747-3f38.ngrok.io";

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
        popupVideoSelection.SetActive(false);

        if (TypingInputPopup.activeSelf)
        {
            TypingInputPopup.SetActive(false);
            inputTextEnglish.text = "";
        }

        StartListening(EnglishLanguageCode);
     }

    public void sinhala() {
        processingMode = "sinhala";
        sinhalaSingView.SetActive(true);
        popupVideoSelection.SetActive(false);

        if (TypingInputPopup.activeSelf)
        {
            TypingInputPopup.SetActive(false);
            inputTextSinhala.text = "";
        }

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
        if (processingMode == "sinhala")
        {
            StartCoroutine(getSinhalaData(result));
        }
        else
        {
            StartCoroutine(getEnglishData(result));
        }
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
            TypingInputPopup.SetActive(false);
        }
       
    }

    public void openType(string lnaguage)
    {
        popupVideoSelection.SetActive(false);

        if (TypingInputPopup.activeSelf)
        {
            TypingInputPopup.SetActive(false);
            inputTextEnglish.text = "";
            inputTextSinhala.text = "";
        }
        else
        {
            TypingInputPopup.SetActive(true);
        }

        if (lnaguage == "sinhala")
        {
            processingMode = "sinhala";
            sinhalaSingView.SetActive(true);
            inputTextSinhala.gameObject.SetActive(true);
            inputTextEnglish.gameObject.SetActive(false);
        }
        else
        {
            processingMode = "english";
            sinhalaSingView.SetActive(false);
            inputTextEnglish.gameObject.SetActive(true);
            inputTextSinhala.gameObject.SetActive(false);
        }

    }
        
    public void okType()
    {
        TypingInputPopup.SetActive(false);

        if(processingMode != "english")
        {
            StartCoroutine(getSinhalaData(inputTextSinhala.text));
        }
        else
        {
            StartCoroutine(getEnglishData(inputTextEnglish.text));
        }

        inputTextEnglish.text = "";
        inputTextSinhala.text = "";
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
                sendVideo(path);
            }
        },new string[] { FileType});
    }

    #region API call sinhala
    IEnumerator getSinhalaData(string sentence)
    {
        Debug.Log("sentence is " + sentence);

        string jsonPayload = "{\"sentence\":\" " + sentence + " \"}";


        using (UnityWebRequest request = new UnityWebRequest(URL+ "/predict/sl", "POST"))
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
                string jsonString = request.downloadHandler.text;
          
                Debug.Log("json " + jsonString);
                JSONData data = JsonUtility.FromJson<JSONData>(jsonString);

                sinhalaSingView.GetComponent<SinhalaSign>().showSinhalaSign(data.result);

            }
            
        }
    }
    #endregion


    #region API call English
    IEnumerator getEnglishData(string sentence)
    {
        Debug.Log("sentence is " + sentence);

        string jsonPayload = "{\"sentence\":\" " + sentence + " \"}";


        using (UnityWebRequest request = new UnityWebRequest(URL + "/predict/en", "POST"))
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
                string jsonString = request.downloadHandler.text;

                Debug.Log("json " + jsonString);
                JSONData data = JsonUtility.FromJson<JSONData>(jsonString);

                StartCoroutine(playAnimation(data.result));

            }

        }
    }
    #endregion

    #region play Animation
    IEnumerator playAnimation(string[] result)
    {
        float delayBetweenIterations = 2.0f;

        foreach (string value in result)
        {
            string animation = value.Replace(".gif", "").Replace(",gif", "").Replace("'", "").Replace(" ", "_").ToLower();

            Debug.Log("Performing animation-> " + animation);
            if (mAnimator != null)
            {
                mAnimator.SetTrigger(animation);
            }
            else
            {
                Debug.Log("mAnimator null");
            }
            // Wait for the specified delay before continuing to the next iteration
            yield return new WaitForSeconds(delayBetweenIterations);
        }

        if (mAnimator != null)
        {
            mAnimator.SetTrigger("TrIdeal");
        }
        else
        {
            Debug.Log("mAnimator null");
        }

    }
    #endregion

    public void sendVideo(string filePath)
    {
        showTextInPopup("Processing....");
        StartCoroutine(UploadCoroutine(filePath));
    } 
    
    public void sendFrames(string[] framPaths)
    {
        showTextInPopup("Processing....");
        StartCoroutine(UploadFramesCoroutine(framPaths));
    }

  
    /* Upload the chosen video file to the movie server */
    [Obsolete]
    IEnumerator UploadCoroutine(string filePath)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("files", File.ReadAllBytes(filePath));
        UnityWebRequest www = UnityWebRequest.Post(URL + "/predict/video", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string jsonString = www.downloadHandler.text;
            Debug.Log("Form upload complete! " + jsonString);
            JSONDataVideo data = JsonUtility.FromJson<JSONDataVideo>(jsonString);
            showTextInPopup(data.result);
        }
    }


    /* Upload frames */
    [Obsolete]
    IEnumerator UploadFramesCoroutine(string[] framePaths)
    {
        WWWForm form = new WWWForm();
       // form.AddBinaryData("files", File.ReadAllBytes(framPaths[0]));

        foreach (string framePath in framePaths)
        {
            byte[] frameData = File.ReadAllBytes(framePath);
            form.AddBinaryData("files", frameData, Path.GetFileName(framePath));
        }

        UnityWebRequest www = UnityWebRequest.Post(URL + "/predict/frames", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string jsonString = www.downloadHandler.text;
            Debug.Log("Form upload complete! " + jsonString);
            JSONDataVideo data = JsonUtility.FromJson<JSONDataVideo>(jsonString);
            showTextInPopup(data.result);
        }
    }

    public void showTextInPopup(string value)
    {
        ShowTextPopup.SetActive(true);
        ShowTextPopupText.text = value;
    }

    public void hideTextInPopup()
    {
        ShowTextPopup.SetActive(false);
        ShowTextPopupText.text = "";
    }

    public void openSettings()
    {
        SettingsPopup.SetActive(true);

        urlText.text = URL;

    }
    public void closeSettings()
    {
        SettingsPopup.SetActive(false);
    }
    
    public void saveSettings()
    {
        SettingsPopup.SetActive(false);

        URL = urlText.text ;
    }
}