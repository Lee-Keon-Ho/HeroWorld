using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class CLoginApp : MonoBehaviour
{
    private CLoginSocket m_loginSocket;
    public CLoginPacketHandler m_packetHandler;
    public TMP_InputField id;
    public TMP_InputField pw;
    public Button loginButton;
    public Button caButton;
    public Image notises1;
    public Image notises2;
    public Image notises3;
    public Image notises4;
    private void Awake()
    {
        Screen.SetResolution(960, 480, false);
        Application.targetFrameRate = 60;
        m_loginSocket = new CLoginSocket();
        
        m_loginSocket.Init("112.184.241.183", 30003);

        m_packetHandler.Initialized(m_loginSocket);
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        m_loginSocket.test();
    }

    void Update()
    {
        while (m_loginSocket.QueueCount() > 0)
        {
            m_packetHandler.Handle(m_loginSocket.GetBuffer());
        }
    }

    private void OnDestroy()
    {
        m_loginSocket.Delete();
    }

    public void login()
    {
        if(id.text.Length > 0 && pw.text.Length > 0 && !notises1.gameObject.activeSelf)
        {
            m_loginSocket.Login(id.text, pw.text);
        }
    }

    public void CreateAccount()
    {
        if(id.text.Length > Constants.text_max && pw.text.Length > Constants.text_max)
        {
            m_loginSocket.CreateAccount(id.text, pw.text);
        }
    }

    public void ConfirmCheckID()
    {
        if (id.text.Length > Constants.text_max)
        {
            m_loginSocket.ConfirmCheckID(id.text);
        }
    }

    public void OnCancel()
    {
        notises2.gameObject.SetActive(false);
        id.interactable = true;
        pw.interactable = true;
        loginButton.interactable = true;
        caButton.interactable = true;
    }

    public void OnNotises1CheckButton()
    {
        notises1.gameObject.SetActive(false);
        id.interactable = true;
        pw.interactable = true;
        loginButton.interactable = true;
        caButton.interactable = true;
    }
    public void OnNotises2CheckButton()
    {
        notises2.gameObject.SetActive(false);
        id.interactable = true;
        pw.interactable = true;
        loginButton.interactable = true;
        caButton.interactable = true;
    }
    public void OnNotises3CheckButton()
    {
        notises3.gameObject.SetActive(false);
        id.interactable = true;
        pw.interactable = true;
        loginButton.interactable = true;
        caButton.interactable = true;
    }

    public void OnNotises4CheckButton()
    {
        notises2.gameObject.SetActive(false);
        notises4.gameObject.SetActive(false);
        id.interactable = true;
        pw.interactable = true;
        loginButton.interactable = true;
        caButton.interactable = true;
    }

    public void SocketDelete()
    {
        m_loginSocket.Delete();
    }
}
