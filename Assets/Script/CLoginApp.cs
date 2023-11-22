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
    public CStruct.sCharacterList sCharacters;
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
        Screen.SetResolution(1280, 720, false);
        Application.targetFrameRate = 60;
        m_loginSocket = new CLoginSocket();

        m_loginSocket.Init("112.184.241.183", 30003);

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

    public void CreateCharacter(string _name, int _index)
    {
        if(_index > 0)
        {
            m_loginSocket.CreateCharacter(_name, _index);
        }

    }

    public void DeleteCharacter(string _name)
    {
        if(_name.Length > 0)
        {
            m_loginSocket.DeleteCharacter(_name);
        }
    }

    public void DoubleCheck(string _name)
    {
        m_loginSocket.DoubleCheck(_name);
    }
    public void OnStartButton(string _name, int _nameLen)
    {
        m_loginSocket.InField(_name, _nameLen);
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

    public void SetCharacterList(CStruct.sCharacterList _sCharacter)
    {
        sCharacters = _sCharacter;
    }

    public CStruct.sCharacterList GetCharacterList() { return sCharacters; }
}
