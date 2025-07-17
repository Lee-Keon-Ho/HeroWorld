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

        byte[] id = new byte[30];
        Array.Clear(id, 0, id.Length);
        byte[] idStrByte = System.Text.Encoding.Unicode.GetBytes(_id);
        Array.Copy(idStrByte, id, idStrByte.Length);

        byte[] pw = new byte[30];
        Array.Clear(pw, 0, pw.Length);
        byte[] pwStrByte = System.Text.Encoding.Unicode.GetBytes(_pw);
        Array.Copy(pwStrByte, pw, pwStrByte.Length);

        bw.Write((ushort)(4 + id.Length + pw.Length));
        bw.Write((ushort)1);
        bw.Write(id);
        bw.Write(pw);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void ConfirmCheckID(string _id)
    {
        memoryStream.Position = 0;

        byte[] id = new byte[30];
        Array.Clear(id, 0, id.Length);
        byte[] idStrByte = System.Text.Encoding.Unicode.GetBytes(_id);
        Array.Copy(idStrByte, id, idStrByte.Length);

        bw.Write((ushort)(4 + id.Length));
        bw.Write((ushort)2);
        bw.Write(id);

        m_socket.Send(m_sendBuffer, (int)memoryStream.Position, 0);
    }

    public void CreateAccount(string _id, string _pw)
    {
        memoryStream.Position = 0;

        byte[] id = new byte[30];
        Array.Clear(id, 0, id.Length);
        byte[] idStrByte = System.Text.Encoding.Unicode.GetBytes(_id);
        Array.Copy(idStrByte, id, idStrByte.Length);

        byte[] pw = new byte[30];
        Array.Clear(pw, 0, pw.Length);
        byte[] pwStrByte = System.Text.Encoding.Unicode.GetBytes(_pw);
        Array.Copy(pwStrByte, pw, pwStrByte.Length);

        bw.Write((ushort)(4 + id.Length + pw.Length));
        bw.Write((ushort)3);
        bw.Write(id);
        bw.Write(pw);

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
