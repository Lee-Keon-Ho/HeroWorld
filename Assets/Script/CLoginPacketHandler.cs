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

    public TMP_InputField id;
    public TMP_InputField pw;
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
            case 6:
                InField();
                break;
            case 7:
                DoubleCheck();
                break;
            case 8:
                CharacterList();
                break;
            case 9:
                UpdateCharacterList();
                break;
            default:
                break;
        }
    }

    private void Login()
    {
        int key = binaryReader.ReadUInt16();

        if (key == 0)
        {
            id.interactable = false;
            pw.interactable = false;
            loginButton.interactable = false;
            caButton.interactable = false;
            image1.gameObject.SetActive(true);
        }
        if(key > 0)
        {
            CSceneManager.Instance.OnChangeScene("Characterselection");
        }
    }
    private void CheckID()
    {
        int key = binaryReader.ReadUInt16();

        id.interactable = false;
        pw.interactable = false;
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

        id.interactable = false;
        pw.interactable = false;
        loginButton.interactable = false;
        caButton.interactable = false;
        key = 1;
        if (key == 1)
        {
            image4.gameObject.SetActive(true);
        }
    }

    private void CharacterList()
    {
        int key = binaryReader.ReadUInt16();
        CStruct.sCharacterList sCharacter;

        sCharacter.c1_name_Len = binaryReader.ReadInt32();
        sCharacter.c1_name = System.Text.Encoding.Default.GetString(binaryReader.ReadBytes(28));
        sCharacter.c1 = binaryReader.ReadInt32();
        sCharacter.c_1_Level = binaryReader.ReadInt32();

        sCharacter.c2_name_Len = binaryReader.ReadInt32();
        sCharacter.c2_name = System.Text.Encoding.Default.GetString(binaryReader.ReadBytes(28));
        sCharacter.c2 = binaryReader.ReadInt32();
        sCharacter.c_2_Level = binaryReader.ReadInt32();

        sCharacter.c3_name_Len = binaryReader.ReadInt32();
        sCharacter.c3_name = System.Text.Encoding.Default.GetString(binaryReader.ReadBytes(28));
        sCharacter.c3 = binaryReader.ReadInt32();
        sCharacter.c_3_Level = binaryReader.ReadInt32();

        CLoginApp app = FindAnyObjectByType<CLoginApp>();
        app.SetCharacterList(sCharacter);
    }

    private void UpdateCharacterList()
    {
        int key = binaryReader.ReadUInt16();
        CStruct.sCharacterList sCharacter;

        sCharacter.c1_name_Len = binaryReader.ReadInt32();
        sCharacter.c1_name = System.Text.Encoding.Default.GetString(binaryReader.ReadBytes(28));
        sCharacter.c1 = binaryReader.ReadInt32();
        sCharacter.c_1_Level = binaryReader.ReadInt32();

        sCharacter.c2_name_Len = binaryReader.ReadInt32();
        sCharacter.c2_name = System.Text.Encoding.Default.GetString(binaryReader.ReadBytes(28));
        sCharacter.c2 = binaryReader.ReadInt32();
        sCharacter.c_2_Level = binaryReader.ReadInt32();

        sCharacter.c3_name_Len = binaryReader.ReadInt32();
        sCharacter.c3_name = System.Text.Encoding.Default.GetString(binaryReader.ReadBytes(28));
        sCharacter.c3 = binaryReader.ReadInt32();
        sCharacter.c_3_Level = binaryReader.ReadInt32();

        CCharacterScene cCharacter = FindAnyObjectByType<CCharacterScene>();
        cCharacter.UpdateCharacterList(sCharacter);
    }

    private void InField()
    {
        byte[] Buffer = new byte[32];
        Buffer = binaryReader.ReadBytes(32);
        string name = System.Text.Encoding.Unicode.GetString(Buffer);

        CDataManager.Instance.SetKey(name);
        CSceneManager.Instance.OnChangeScene("ForestTown");

        // key를 저장하고 연결은 끊어도 된다.
    }

    private void DoubleCheck()
    {
        int check = binaryReader.ReadUInt16();

        CCharacterScene scene = FindAnyObjectByType<CCharacterScene>();
        scene.NameCheck(check);
    }
}
