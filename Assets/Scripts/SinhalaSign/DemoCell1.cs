using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using TMPro;
using System;

public class DemoCell1 : MonoBehaviour, ICell
{
    [Header("GameObject")]
    public GameObject image;

    [Header("TextMeshProUGUI")]
    public TextMeshProUGUI UserName;


    //Model
    private ContactInfo1 contactInfo;
    private int _cellIndex;
    private string userId;

    private void Start()
    {
   
    }


    public void ConfigureCell1(ContactInfo1 contactInfo, int cellIndex)
    {
        _cellIndex = cellIndex;
        this.contactInfo = contactInfo;

        UserName.text = contactInfo.signImage;
        image.GetComponent<Image>().sprite = Resources.Load<Sprite>("SinhalaSign/" + contactInfo.signImage);

    }

    private void ButtonListener()
    {

    }
}
