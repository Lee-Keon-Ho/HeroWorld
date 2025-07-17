using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWorldApp : MonoBehaviour
{
    public CStruct.sCharacterList sCharacters;
    private CWorldSocket m_WorldSocket;
    public CWorldPacketHandler m_packetHandler;
    
    void Start()
    {
        m_WorldSocket = new CWorldSocket();
        m_WorldSocket.Init("112.184.241.183", 50003);
        m_packetHandler.Initialized(m_WorldSocket);
        m_WorldSocket.Login();
        DontDestroyOnLoad(this);
    }

    
    void Update()
    {
        while (m_WorldSocket.QueueCount() > 0)
        {
            m_packetHandler.Handle(m_WorldSocket.GetBuffer());
        }
    }

    public void DoubleCheck(string _name)
    {
        m_WorldSocket.DoubleCheck(_name);
    }

    public void CreateCharacter(string _name, int _index)
    {
        if (_index > 0)
        {
            m_WorldSocket.CreateCharacter(_name, _index);
        }
    }

    public void DeleteCharacter(string _name)
    {
        if (_name.Length > 0)
        {
            m_WorldSocket.DeleteCharacter(_name);
        }
    }

    public void OnStartButton(string _name, int _nameLen)
    {
        m_WorldSocket.InField(_name, _nameLen);
    }

    public void ChannelChange(int _channel)
    {
        m_WorldSocket.ChannelChange(_channel);
    }

    public void SetCharacterList(CStruct.sCharacterList _sCharacter)
    {
        sCharacters = _sCharacter;
    }

    public CStruct.sCharacterList GetCharacterList() { return sCharacters; }

    private void OnDestroy()
    {
        m_WorldSocket.Delete();
    }
}
