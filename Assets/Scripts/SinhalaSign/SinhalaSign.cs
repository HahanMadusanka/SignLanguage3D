using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyAndCode.UI;

public struct ContactInfo1
{
    public string UserName;
    public long profPicID;
}

public class SinhalaSign : MonoBehaviour, IRecyclableScrollRectDataSource
{
    [SerializeField]
    RecyclableScrollRect recyclableScrollRect;

    //Dummy data List
    private List<ContactInfo1> contactList = new List<ContactInfo1>();

    private void Awake()
    {
        recyclableScrollRect.DataSource = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<10; i++)
        {
            ContactInfo1 obj = new ContactInfo1();
            obj.UserName = "hashan " + i;
            contactList.Add(obj);
        }
        recyclableScrollRect.show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region DATA-SOURCE

    public int GetItemCount()
    {
        return contactList.Count;
    }

    public void SetCell(ICell cell, int index)
    {
        var item = cell as DemoCell1;
        item.ConfigureCell1(contactList[index], index);
    }
    #endregion
}
