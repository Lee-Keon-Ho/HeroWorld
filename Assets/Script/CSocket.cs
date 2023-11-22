using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

public class CSocket
{
    CRingBuffer m_ringBuffer;
    Queue<byte[]> m_que;

    byte[] m_sendBuffer;
    Socket m_socket;
    Thread m_thread;

    MemoryStream memoryStream;
    BinaryWriter bw;

    object lockObj;

    float latency = 0f;
    public void Init(String _ip, int _port)
    {
        m_que = new Queue<byte[]>();
        lockObj = new object();

        m_ringBuffer = new CRingBuffer(65535);

        m_sendBuffer = new byte[65535];

        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);

            m_socket.Connect(iPEndPoint);
            byte[] temp = new byte[10];
            m_socket.Receive(temp, 0, 4, SocketFlags.None);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        m_thread = new Thread(Run);
        m_thread.Start();

        memoryStream = new MemoryStream(m_sendBuffer);
        bw = new BinaryWriter(memoryStream);
    }

    public void Delete()
    {
        m_socket.Close();
    }

    async void Run()
    {
        int size;

        byte[] sizeBuffer = new byte[2];
        while (true)
        {
            try
            {
                m_socket.Receive(sizeBuffer, 0, 2, SocketFlags.None);

                size = BitConverter.ToUInt16(sizeBuffer) - 2;

                if (size <= 0)
                {
                    m_socket.Close();
                    break;
                }

                byte[] Buffer = new byte[size];

                m_socket.Receive(Buffer, 0, size, SocketFlags.None);

                lock (lockObj)
                {
                    m_que.Enqueue(Buffer);
                }
            }
            catch(SocketException e)
            {
                Debug.Log(e);
                m_socket.Close();
                break;
            }
        }
    }

    public void Latency()
    {
        memoryStream.Position = 0;

        bw.Write((ushort)8);
        bw.Write((ushort)0);
        bw.Write(Time.time);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }
    public void Login()
    {
        byte[] bytes = System.Text.Encoding.Unicode.GetBytes(CDataManager.Instance.GetKey());
        memoryStream.Position = 0;

        bw.Write((ushort)(6 + bytes.Length));
        bw.Write((ushort)1);
        bw.Write((ushort)bytes.Length);
        bw.Write(bytes);

        int size = m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
        if(size <= 0)
        {
            Debug.Log("Login");
        }
    }

    public void InField()
    {
        memoryStream.Position = 0;

        bw.Write((ushort)4);
        bw.Write((ushort)2);

        int size = m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
        if (size <= 0)
        {
            Debug.Log("InField");
        }
    }

    public void NextField(int _index)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)6);
        bw.Write((ushort)3);
        bw.Write((ushort)_index);

        int size = m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
        if (size <= 0)
        {
            Debug.Log("NextField");
        }
    }

    public void Warp(int _index)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)6);
        bw.Write((ushort)98);
        bw.Write((ushort)_index);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }
    public void MoveUser(Vector3 _strtPosition, Vector3 _EndPosition, ushort _number, int _state)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)(8 + 24));
        bw.Write((ushort)5);
        bw.Write(_number);
        bw.Write((ushort)_state);

        bw.Write(_strtPosition.x);
        bw.Write(_strtPosition.y);
        bw.Write(_strtPosition.z);
        bw.Write(_EndPosition.x);
        bw.Write(_EndPosition.y);
        bw.Write(_EndPosition.z);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void NowPosition(Vector3 _position, ushort _number)
    {
        int size;
        memoryStream.Position = 0;

        bw.Write((ushort)(6 + 12));
        bw.Write((ushort)4);
        bw.Write(_number);
        bw.Write(_position.x);
        bw.Write(1.0f);
        bw.Write(_position.z);

        try
        {
            size = m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }
     
    public void Arrive(Vector3 _position, ushort _number, float _y, int _state)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)(8 + 4 + 12));
        bw.Write((ushort)6);
        bw.Write(_number);
        bw.Write((ushort)_state);
        bw.Write(_y);
        bw.Write(_position.x);
        bw.Write(1.0f);
        bw.Write(_position.z);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void LogOut()
    {
        memoryStream.Position = 0;

        bw.Write((ushort)4);
        bw.Write((ushort)7);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void PlayerIdleAttack(float _rotationY)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)8);
        bw.Write((ushort)12);
        bw.Write(_rotationY);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void PlayerMoveAttack(Vector3 _position, float _rotationY)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)20);
        bw.Write((ushort)13);
        bw.Write(_position.x);
        bw.Write(1.0f);
        bw.Write(_position.z);
        bw.Write(_rotationY);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void ArcherIdleAttack(float _rotationY)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)8);
        bw.Write((ushort)17);
        bw.Write(_rotationY);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void ArcherMoveAttack(Vector3 _position, float _rotationY)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)20);
        bw.Write((ushort)18);
        bw.Write(_position.x);
        bw.Write(_position.y);
        bw.Write(_position.z);
        bw.Write(_rotationY);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }
    public void HitSingleMonster(int _index)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)6);
        bw.Write((ushort)33);
        bw.Write((ushort)_index);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void HitMonster(List<int> _indexList)
    {
        int count = _indexList.Count;
        memoryStream.Position = 0;

        bw.Write((ushort)(6 + (2 * count)));
        bw.Write((ushort)14);
        bw.Write((ushort)count);
        
        for(var i = 0; i < count; i++)
        {
            bw.Write((ushort)_indexList[i]);
        }

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void SendChatting(string _str)
    {
        memoryStream.Position = 0;

        byte[] str = System.Text.Encoding.Unicode.GetBytes(_str);

        bw.Write((ushort)(6 + str.Length));
        bw.Write((ushort)15);
        bw.Write((ushort)str.Length);
        bw.Write(str);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void UserCount()
    {
        memoryStream.Position = 0;

        bw.Write((ushort)4);
        bw.Write((ushort)99);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public CRingBuffer GetRingBuffer() { return m_ringBuffer; }

    public void RingBufferRead(int _size)
    {
        m_ringBuffer.Read(_size);
    }

    public int QueueCount()
    {
        return m_que.Count;
    }

    public byte[] GetBuffer()
    {
        lock (lockObj)
        {
            return m_que.Dequeue();
        }
    }

    public float GetLatency() { return latency; }
}
