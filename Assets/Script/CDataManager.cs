using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDataManager : MonoBehaviour
{
    CStruct.sCharacterInfo m_characterInfo;
    string key;
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
    }
    public void SetKey(string _name) { key = _name; }
    public void SetInfo(int _level, int _curHp, int _maxHp, int _curMp, int _maxMp, int _curExp, Vector3 _position)
    {
        m_characterInfo.level = _level;
        m_characterInfo.curHp = _curHp;
        m_characterInfo.maxHp = _maxHp;
        m_characterInfo.curMp = _curMp;
        m_characterInfo.maxMp = _maxMp;
        m_characterInfo.curExp = _curExp;
        m_characterInfo.position = _position;
    }

    public CStruct.sCharacterInfo GetInfo() { return m_characterInfo; }
    public string GetKey() { return key; }
}
