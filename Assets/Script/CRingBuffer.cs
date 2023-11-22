using System;
using System.IO;

public class CRingBuffer
{
    byte[] buffer;
    byte[] tempBuffer;
    int bufferSize;
    int remainDataSize;

    MemoryStream writeStream;
    MemoryStream readStream;

    public object lockObject;
    public CRingBuffer(int _size)
    {
        bufferSize = _size;

        buffer = new byte[bufferSize];
        tempBuffer = new byte[bufferSize];

        writeStream = new MemoryStream(buffer);
        readStream = new MemoryStream(buffer);

        lockObject = new object();
    }

    private bool IsFull()
    {
        return (remainDataSize >= bufferSize);
    }

    public int GetWriteBufferSize()
    {
        if (IsFull()) return 0;

        if (writeStream.Position >= readStream.Position) return bufferSize - (int)writeStream.Position;

        return (int)(readStream.Position - writeStream.Position);
    }

    public void Write(int _recvSize)
    {
        lock (lockObject)
        {
            writeStream.Position += _recvSize;
            remainDataSize += _recvSize;

            if (writeStream.Position >= bufferSize)
            {
                writeStream.Position = 0;
            }
        }
    }

    public void Read(int _size)
    {
        lock (lockObject)
        {
            if (remainDataSize >= _size)
            {
                int endSize = bufferSize - (int)readStream.Position;

                if (endSize < _size)
                {
                    endSize = _size - endSize;
                    readStream.Position = endSize;
                }
                else
                {
                    readStream.Position += _size;
                }
                remainDataSize -= _size;
            }
        }
    }

    public int GetSize()
    {
        return bufferSize;
    }

    public int GetRemainSize()
    {
        return remainDataSize;
    }

    public byte[] GetBuffer()
    {
        return buffer;
    }

    public int GetWritePos()
    {
        return (int)writeStream.Position;
    }

    public int GetReadPos()
    {
        return (int)readStream.Position;
    }

    public int GetReadSize()
    {
        if (bufferSize - readStream.Position == 1)
        {
            byte[] tembuf = new byte[10];

            //Array.Copy(buffer, readStream.Position, tembuf, 0, 1);
            //Array.Copy(buffer, 0, tembuf, 1, 1);
            Buffer.BlockCopy(buffer, (int)readStream.Position, tembuf, 0, 1);
            Buffer.BlockCopy(buffer, 0, tembuf, 1, 1);

            return BitConverter.ToUInt16(tembuf, 0);
        }
        else
        {
            if(readStream.Position == bufferSize)
            {
                //Array.Copy(buffer, 0, tembuf, 0, 2);
                return BitConverter.ToUInt16(buffer, 0);
            }
            else
            {
                //Array.Copy(buffer, readStream.Position, tembuf, 0, 2);
                return BitConverter.ToUInt16(buffer, (int)readStream.Position);
            }

            //size = BitConverter.ToUInt16(tembuf, 0);
        }

        //if (remainDataSize >= m_readSize) return;

        //return;
    }

    public byte[] GetPacketBuffer()
    {
        int buffer_end = bufferSize - (int)readStream.Position;

        if (remainDataSize > buffer_end)
        {
            Buffer.BlockCopy(buffer, (int)readStream.Position, tempBuffer, 0, buffer_end);
            Buffer.BlockCopy(buffer, 0, tempBuffer, buffer_end, remainDataSize = buffer_end);
            //Array.Copy(buffer, readStream.Position, tempBuffer, 0, buffer_end);
            //Array.Copy(buffer, 0, tempBuffer, buffer_end, remainDataSize - buffer_end);
        }
        else
        {
            Buffer.BlockCopy(buffer, (int)readStream.Position, tempBuffer, 0, remainDataSize);
            //Array.Copy(buffer, readStream.Position, tempBuffer, 0, remainDataSize);
        }

        return tempBuffer;
    }

    public bool IsPacket()
    {
        if (remainDataSize < 2) return false;
        return remainDataSize >= GetReadSize();
    }
}
