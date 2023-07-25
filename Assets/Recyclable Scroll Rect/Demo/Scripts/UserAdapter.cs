using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using TMPro;

public class UserAdapter : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject image;

    [Header("TextMeshProUGUI")]
    public TextMeshProUGUI ScoreValue;
    public TextMeshProUGUI UserName;
    public TextMeshProUGUI EmailAddress;
    

    [Header("Model")]
    //private UserInfo _userInfo;
    private int _cellIndex;

    // Start is called before the first frame update
    void Start()
    {
        //Can also be done in the inspector
        GetComponent<Button>().onClick.AddListener(ButtonListener);
    }

    //This is called from the SetCell method in DataSource
  //  public void UserCell(UserInfo contactInfo, int cellIndex)
  //  {
   //     _cellIndex = cellIndex;
  //      _userInfo = contactInfo;

        //UserName.text = contactInfo.UserName;
      //  EmailAddress.text = contactInfo.EmailAddress;
       // ScoreValue.text = (string) contactInfo.ScoreValue;
      //  image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/ProfPics/" + PlayerPrefs.GetInt(((int)contactInfo.profPicID).ToString()));
 //   }

    private void ButtonListener()
    {
     //   Debug.Log("Index : " + _cellIndex + ", Name : " + _userInfo.UserName + ", EmailAddress : " + _userInfo.EmailAddress);
    }
}
