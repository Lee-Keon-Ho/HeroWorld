using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class Chatting : MonoBehaviour
{
    public Selectable firstInput;
    public TMP_InputField input;
    public GameObject content;
    public TextMeshProUGUI text;
    public GameObject myChatting;
    public TextMeshProUGUI myChatText;
    CApp app;
    EventSystem system;
    void Start()
    {
        system = EventSystem.current;
        app = FindAnyObjectByType<CApp>();
        myChatting.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(input.text.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            SendChatting(input.text);
            input.text = "";
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            firstInput.Select();
        }
    }

    private void SendChatting(string _string)
    {
        app.SendChatting(_string);
    }

    public void InputChat(string _text)
    {
        TextMeshProUGUI textMesh = Instantiate(text);
        textMesh.text = _text;
        textMesh.transform.SetParent(content.transform, false);
    }

    public void Notice(string _str)
    {
        TextMeshProUGUI textMesh = Instantiate(text);
        textMesh.color = Color.yellow;
        textMesh.text = "[공지사항]" + _str;
        textMesh.transform.SetParent(content.transform, false);
    }

    public void MyChat(string _str)
    {
        myChatting.SetActive(true);
        myChatText.text = _str;
        Invoke("ChatDisabled", 3.0f);
    }

    private void ChatDisabled()
    {
        myChatting.SetActive(false);
    }
}
