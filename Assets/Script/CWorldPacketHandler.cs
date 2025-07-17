using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CWorldPacketHandler : MonoBehaviour
{
    MemoryStream memoryStream;
    BinaryReader binaryReader;
    CWorldSocket m_WorldSocket;

    public void Handle(byte[] _Buffer)
    {
        memoryStream = new MemoryStream(_Buffer);
        binaryReader = new BinaryReader(memoryStream);

        memoryStream.Position = 0;

        ushort type = binaryReader.ReadUInt16();

        switch (type)
        {
            case 0:
                Login();
                break;
            case 1:
                CharacterList();
                break;
            case 2:
                UpdateCharacterList();
                break;
            case 3:
                DoubleCheck();
                break;
            case 4:
                CreateCharacter();
                break;
            case 5:
                DeleteCharacter();
                break;
            case 6:
                InField();
                break;
            case 7:
                ChannelChange();
                break;
            default:
                break;
        }
    }

    private void Login()
    {
        m_WorldSocket.CharacterList();
        CSceneManager.Instance.OnChangeScene("Characterselection");
    }

    private void CharacterList()
    {
        CStruct.sCharacterList sCharacter;

        sCharacter.c1_name_Len = binaryReader.ReadInt32();
        sCharacter.c1_name = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(28));
        sCharacter.c1 = binaryReader.ReadInt32();
        sCharacter.c_1_Level = binaryReader.ReadInt32();

        sCharacter.c2_name_Len = binaryReader.ReadInt32();
        sCharacter.c2_name = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(28));
        sCharacter.c2 = binaryReader.ReadInt32();
        sCharacter.c_2_Level = binaryReader.ReadInt32();

        sCharacter.c3_name_Len = binaryReader.ReadInt32();
        sCharacter.c3_name = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(28));
        sCharacter.c3 = binaryReader.ReadInt32();
        sCharacter.c_3_Level = binaryReader.ReadInt32();

        CWorldApp app = FindAnyObjectByType<CWorldApp>();
        app.SetCharacterList(sCharacter);
    }

    private void CreateCharacter()
    {
        int result = binaryReader.ReadUInt16();

        if (result == 1) m_WorldSocket.UpdateCharacterList();
    }

    private void UpdateCharacterList()
    {
        CStruct.sCharacterList sCharacter;

        sCharacter.c1_name_Len = binaryReader.ReadInt32();
        sCharacter.c1_name = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(28));
        sCharacter.c1 = binaryReader.ReadInt32();
        sCharacter.c_1_Level = binaryReader.ReadInt32();

        sCharacter.c2_name_Len = binaryReader.ReadInt32();
        sCharacter.c2_name = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(28));
        sCharacter.c2 = binaryReader.ReadInt32();
        sCharacter.c_2_Level = binaryReader.ReadInt32();

        sCharacter.c3_name_Len = binaryReader.ReadInt32();
        sCharacter.c3_name = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(28));
        sCharacter.c3 = binaryReader.ReadInt32();
        sCharacter.c_3_Level = binaryReader.ReadInt32();

        CCharacterScene cCharacter = FindAnyObjectByType<CCharacterScene>();
        cCharacter.UpdateCharacterList(sCharacter);
    }

    private void DoubleCheck()
    {
        int check = binaryReader.ReadUInt16();

        CCharacterScene scene = FindAnyObjectByType<CCharacterScene>();
        scene.NameCheck(check);
    }

    private void DeleteCharacter()
    {
        int result = binaryReader.ReadUInt16();

        if (result == 1) m_WorldSocket.UpdateCharacterList();
    }

    private void InField()
    {
        byte[] Buffer = new byte[28];
        Buffer = binaryReader.ReadBytes(28);
        string name = System.Text.Encoding.Unicode.GetString(Buffer);
        int field = binaryReader.ReadUInt16();
        uint ip = binaryReader.ReadUInt32();
        ushort port = binaryReader.ReadUInt16();

        CDataManager.Instance.SetName(name);
        CDataManager.Instance.SetIp_Port(ip, port);
        CDataManager.Instance.SetFieldIndex(field);

        if(field == 0) CSceneManager.Instance.OnChangeScene("ForestTown");
        else if(field == 1) CSceneManager.Instance.OnChangeScene("ForestField");
        else if(field == 2) CSceneManager.Instance.OnChangeScene("WinterField");

        CCharacterScene cCharacter = FindAnyObjectByType<CCharacterScene>();
        cCharacter.TEST();
    }

    private void ChannelChange()
    {
        ushort field = binaryReader.ReadUInt16();
        uint ip = binaryReader.ReadUInt32();
        ushort port = binaryReader.ReadUInt16();

        CDataManager.Instance.SetIp_Port(ip, port);
        CDataManager.Instance.SetFieldIndex(field);
        CApp app = FindAnyObjectByType<CApp>();
        app.ChannelChange();

        //if(field == 0) CSceneManager.Instance.OnChangeScene("ForestTown");
        //else if(field == 1) CSceneManager.Instance.OnChangeScene("ForestField");
        //else if (field == 2) CSceneManager.Instance.OnChangeScene("WinterField");
    }

    public void Initialized(CWorldSocket _socket)
    {
        m_WorldSocket = _socket;
    }
}
