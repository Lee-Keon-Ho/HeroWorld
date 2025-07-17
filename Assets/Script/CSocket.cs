using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

public class CSocket // Client
{
    Queue<byte[]> m_que;

    byte[] m_sendBuffer;
    Socket m_socket;
    Thread m_thread;

    MemoryStream memoryStream;
    BinaryWriter bw;

    object lockObj;

    float latency = 0f;
    public void Init()
    {
        m_que = new Queue<byte[]>();
        lockObj = new object();

        m_sendBuffer = new byte[65535];

        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            IPEndPoint iPEndPoint = new IPEndPoint(CDataManager.Instance.GetIP(), CDataManager.Instance.GetPort());

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
        byte[] bytes = new byte[28];
        bytes = System.Text.Encoding.Unicode.GetBytes(CDataManager.Instance.GetName());
        memoryStream.Position = 0;

        bw.Write((ushort)(4 + 28));
        bw.Write((ushort)1);
        bw.Write(bytes);

        int size = m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void InField()
    {
        memoryStream.Position = 0;

        bw.Write((ushort)4);
        bw.Write((ushort)2);

        int size = m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void NextField(int _index)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)6);
        bw.Write((ushort)3);
        bw.Write((ushort)_index);

        int size = m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void Warp(int _index)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)6);
        bw.Write((ushort)16);
        bw.Write((ushort)_index);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void NowPosition(Vector3 _position, ushort _number)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)(6 + 12));
        bw.Write((ushort)4);
        bw.Write(_number);
        bw.Write(_position.x);
        bw.Write(1.0f);
        bw.Write(_position.z);

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
        m_socket.Shutdown(SocketShutdown.Send);
    }

    public void PlayerIdleAttack(float _rotationY)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)8);
        bw.Write((ushort)8); //12
        bw.Write(_rotationY);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void PlayerMoveAttack(Vector3 _position, float _rotationY)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)20);
        bw.Write((ushort)9); //13
        bw.Write(_position.x);
        bw.Write(1.0f);
        bw.Write(_position.z);
        bw.Write(_rotationY);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void HitMonster(List<int> _indexList)
    {
        int count = _indexList.Count;
        memoryStream.Position = 0;

        bw.Write((ushort)(6 + 10));
        bw.Write((ushort)10);
        bw.Write((ushort)count);

        for (var i = 0; i < count; ++i) // 수정이 필요 2024-01-06
        {
            bw.Write((ushort)_indexList[i]);
        }
        for(var i = count; i < 5; ++i)
        {
            bw.Write((ushort)0);
        }

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

        public void SendChatting(string _str)
    {
        memoryStream.Position = 0;

        byte[] str = new byte[28];
        Array.Clear(str, 0, str.Length);
        byte[] StrByte = System.Text.Encoding.Unicode.GetBytes(_str);
        Array.Copy(StrByte, str, StrByte.Length);

        bw.Write((ushort)(4 + 28));
        bw.Write((ushort)11);
        bw.Write(str);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void ArcherIdleAttack(float _rotationY)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)8);
        bw.Write((ushort)12);
        bw.Write(_rotationY);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void ArcherMoveAttack(Vector3 _position, float _rotationY)
    {
        memoryStream.Position = 0;

        bw.Write((ushort)20);
        bw.Write((ushort)13);
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
        bw.Write((ushort)14);
        bw.Write((ushort)_index);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void SendHeartBeat()
    {
        memoryStream.Position = 0;
        bw.Write((ushort)(4));
        bw.Write((ushort)15);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void ChannelChange()
    {
        memoryStream.Position = 0;
        bw.Write((ushort)(4));
        bw.Write((ushort)18);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
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
