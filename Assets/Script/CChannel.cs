using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CChannel : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    public void Awake()
    {
        dropdown.value = CDataManager.Instance.GetChannel();
    }

    public void SelectButton()
    {
        if(dropdown.value != CDataManager.Instance.GetChannel())
        {
            CWorldApp app = FindAnyObjectByType<CWorldApp>();

            app.ChannelChange(dropdown.value + 1);
            CDataManager.Instance.SetChannel(dropdown.value);
        }
    }
}
