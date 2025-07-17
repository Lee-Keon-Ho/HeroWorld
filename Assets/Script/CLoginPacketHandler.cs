using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
public class CLoginPacketHandler : MonoBehaviour
{
    MemoryStream memoryStream;
    BinaryReader binaryReader;
    CLoginSocket m_loginSocket;
    public GameObject worldServerObject;
    public TMP_InputField Input_id;
    public TMP_InputField Input_pw;
    public Button loginButton;
    public Button caButton;
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;

    public void Handle(byte[] _Buffer)
    {
        memoryStream = new MemoryStream(_Buffer);
        binaryReader = new BinaryReader(memoryStream);

        memoryStream.Position = 0;

        ushort type = binaryReader.ReadUInt16();

        switch (type)
        {
            case 1:
                Login();
                break;
            case 2:
                CheckID();
                break;
            case 3:
                CreateAccount();
                break;
            default:
                break;
        }
    }



    private void Login()
    {
        int ret = binaryReader.ReadUInt16();
        int key = binaryReader.ReadUInt16();
        byte[] Buffer = new byte[28];
        Buffer = binaryReader.ReadBytes(28);
        string id = System.Text.Encoding.Unicode.GetString(Buffer);

        if (ret == 0)
        {
            Input_id.interactable = false;
            Input_pw.interactable = false;
            loginButton.interactable = false;
            caButton.interactable = false;
            image1.gameObject.SetActive(true);
        }
        if(ret > 0)
        {
            CDataManager.Instance.SetKey(key);
            CDataManager.Instance.SetId(id);
            worldServerObject.SetActive(true);
        }
    }
    private void CheckID()
    {
        int key = binaryReader.ReadUInt16();

        Input_id.interactable = false;
        Input_pw.interactable = false;
        loginButton.interactable = false;
        caButton.interactable = false;

        if (key == 0) // 0 사용 가능
        {
            image2.gameObject.SetActive(true);
        }
        if (key == 1) // 1 중복
        {
            image3.gameObject.SetActive(true);
        }
    }

    private void CreateAccount()
    {
        int key = binaryReader.ReadUInt16();

        Input_id.interactable = false;
        Input_pw.interactable = false;
        loginButton.interactable = false;
        caButton.interactable = false;
        
        if (key == 1)
        {
            image2.gameObject.SetActive(false);
            image4.gameObject.SetActive(true);
        }
    }

    public void Initialized(CLoginSocket _loginSocket)
    {
        m_loginSocket = _loginSocket;
    }
}
