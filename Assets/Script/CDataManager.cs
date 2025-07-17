using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDataManager : MonoBehaviour
{
    CStruct.sCharacterInfo m_characterInfo;
    int     key;
    string  id;
    string  characterName;
    uint    FieldServerIP_;
    int     FieldServerPort;
    int     FSLogin;
    int     channel;
    int     Field;
    ushort  index;
    bool    warp;
    public static CDataManager Instance
    {
        get
        {
            return instance;
        }
    }

    private static CDataManager instance;

    void Start()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        warp = true;
    }
    public void SetName(string _name) { characterName = _name; }
    public void SetKey(int _key) { key = _key; }
    public void SetId(string _id) { id = _id; }
    public void SetInfo(int _type, int _level, int _curHp, int _maxHp, int _curMp, int _maxMp, int _curExp, Vector3 _position, ushort _index)
    {
        m_characterInfo.type = _type;
        m_characterInfo.level = _level;
        m_characterInfo.curHp = _curHp;
        m_characterInfo.maxHp = _maxHp;
        m_characterInfo.curMp = _curMp;
        m_characterInfo.maxMp = _maxMp;
        m_characterInfo.curExp = _curExp;
        m_characterInfo.position = _position;
        index = _index;
    }
    public void SetPosition(Vector3 _position)
    {
        m_characterInfo.position = _position;
    }
    public void SetFieldIndex(int _index)
    {
        Field = _index;
    }
    public void SetIp_Port(uint _ip, int _port)
    {
        FieldServerIP_ = _ip;
        FieldServerPort = _port;
    }
    public void SetChannel(int _channel)
    {
        channel = _channel;
    }

    public int GetKey() { return key; }
    public string GetId() { return id; }
    public string GetName() { return characterName; }

    public uint GetIP() { return FieldServerIP_; }
    public int GetPort() { return FieldServerPort; }
    public int GetFieldIndex() { return Field; }
    public int GetChannel() { return channel; }

    public ushort GetIndex() { return index; }

    public CStruct.sCharacterInfo GetCharacterInfo() { return m_characterInfo; }

    public bool CompareName(string _name)
    {
        return characterName == _name;
    }

    public void SetWarp(bool _warp)
    {
        warp = _warp;
    }

    public bool GetWarp() { return warp; }
}
