using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;


public class CLoginSocket
{
    CRingBuffer m_ringBuffer;
    Queue<byte[]> m_que;

    byte[] m_sendBuffer;
    Socket m_socket;
    Thread m_thread;

    MemoryStream memoryStream;
    BinaryWriter bw;

    object lockObj;

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
            catch (SocketException e)
            {
                Debug.Log(e);
                m_socket.Close();
                return;
            }
        }
        m_socket.Close();
    }

    public void test()
    {
        memoryStream.Position = 0;

        bw.Write((ushort)4);
        bw.Write((ushort)0);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void Login(string _id, string _pw)
    {
        memoryStream.Position = 0;

        byte[] id = System.Text.Encoding.Unicode.GetBytes(_id);
        byte[] pw = System.Text.Encoding.Unicode.GetBytes(_pw);

        bw.Write((ushort)(8 + id.Length + pw.Length));
        bw.Write((ushort)1);
        bw.Write((ushort)id.Length);
        bw.Write(id);
        bw.Write((ushort)pw.Length);
        bw.Write(pw);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void ConfirmCheckID(string _id)
    {
        memoryStream.Position = 0;

        byte[] id = System.Text.Encoding.Unicode.GetBytes(_id);
        bw.Write((ushort)(6 + id.Length));
        bw.Write((ushort)2);
        bw.Write((ushort)id.Length);
        bw.Write(id);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void CreateAccount(string _id, string _pw)
    {
        memoryStream.Position = 0;

        byte[] id = System.Text.Encoding.Unicode.GetBytes(_id);
        byte[] pw = System.Text.Encoding.Unicode.GetBytes(_pw);

        bw.Write((ushort)(8 + id.Length + pw.Length));
        bw.Write((ushort)3);
        bw.Write((ushort)id.Length);
        bw.Write(id);
        bw.Write((ushort)pw.Length);
        bw.Write(pw);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void CreateCharacter(string _name, int _index)
    {
        memoryStream.Position = 0;

        byte[] name = System.Text.Encoding.Unicode.GetBytes(_name);

        bw.Write((ushort)(8 + name.Length));
        bw.Write((ushort)4);
        bw.Write((ushort)_index);
        bw.Write((ushort)name.Length);
        bw.Write(name);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void DeleteCharacter(string _name)
    {
        memoryStream.Position = 0;

        byte[] name = System.Text.Encoding.Unicode.GetBytes(_name);

        bw.Write((ushort)(6 + name.Length));
        bw.Write((ushort)5);
        bw.Write((ushort)name.Length);
        bw.Write(name);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void InField(string _name, int _nameLen)
    {
        memoryStream.Position = 0;

        byte[] name = System.Text.Encoding.Unicode.GetBytes(_name, 0, _nameLen);

        bw.Write((ushort)(6 + name.Length));
        bw.Write((ushort)6);
        bw.Write((ushort)name.Length);
        bw.Write(name);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void DoubleCheck(string _name)
    {
        memoryStream.Position = 0;

        byte[] name = System.Text.Encoding.Unicode.GetBytes(_name);

        bw.Write((ushort)(6 + name.Length));
        bw.Write((ushort)7);
        bw.Write((ushort)name.Length);
        bw.Write(name);

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
}
