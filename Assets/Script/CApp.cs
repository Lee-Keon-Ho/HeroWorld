using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CApp : MonoBehaviour
{
    private CSocket m_socket;
    public CPacketHandler m_packetHandler;

    float timer = 0f;
    GUIStyle style = new GUIStyle();
    float x = 5f;
    float y = 5f;
    float w = Screen.width;
    float h = 20f;

    float deltaTime = 0.0f;

    private void Awake()
    {
        m_socket = new CSocket();
        m_socket.Init("112.184.241.183", 30002);
        
        style.normal.textColor = Color.red;
        style.fontSize = 20;

        m_socket.Login();
        m_socket.NextField(0);

        DontDestroyOnLoad(this);
    }

    //public void OnGUI()
    //{
    //    GUI.Label(new Rect(w - 200, y, w, h + 20), "FPS : " + 1.0f / deltaTime, style);
    //}

    private void Start()
    {
        m_socket.InField();
    }

    void Update()
    {
        while (m_socket.QueueCount() > 0)
        {
            m_packetHandler.Handle(m_socket.GetBuffer());
        }

        //timer += Time.deltaTime;
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        //if (timer >= 1f)
        //{
        //    timer = 0f;
        //    m_socket.Latency();
        //    m_socket.UserCount();
        //}
    }

    private void OnDestroy()
    {
        m_socket.LogOut();
        m_socket.Delete();
    }
    public void InField()
    {
        m_socket.InField();
    }
    public void NextField(int _index)
    {
        m_socket.NextField(_index);
    }
    public void Warp(int _index)
    {
        m_socket.Warp(_index);
    }
    public void MoveUser(Vector3 _startPosition, Vector3 _endPosition, ushort _number, int _state)
    {
        m_socket.MoveUser(_startPosition, _endPosition, _number, _state);
    }

    public void NowPosition(Vector3 _position, ushort _number)
    {
        m_socket.NowPosition(_position, _number);
    }

    public void Arrive(Vector3 _position, ushort _number, float _y, int _state)
    {
        m_socket.Arrive(_position, _number, _y, _state);
    }

    public void PlayerMoveAttack(Vector3 _position, float _rotationY)
    {
        m_socket.PlayerMoveAttack(_position, _rotationY);
    }

    public void PlayerIdleAttack(float _rotationY)
    {
        m_socket.PlayerIdleAttack(_rotationY);
    }

    public void ArcherIdleAttack(float _rotationY)
    {
        m_socket.ArcherIdleAttack(_rotationY);
    }

    public void ArcherMoveAttack(Vector3 _position, float _rotationY)
    {
        m_socket.ArcherMoveAttack(_position, _rotationY);
    }
    public void HitMonster(int _index)
    {
        m_socket.HitSingleMonster(_index);
    }
    
    public void HitMonster(List<int> _indexList)
    {
        m_socket.HitMonster(_indexList);
    }

    public void SendChatting(string _str)
    {
        m_socket.SendChatting(_str);
    }
    public float GetFPS() { return deltaTime; }

    public float GetLatency() { return m_socket.GetLatency(); }
}
