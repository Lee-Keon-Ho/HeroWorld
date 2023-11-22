using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileNode
{
    [System.Serializable]
    public struct vector3
    {
        public float x;
        public float y;
        public float z;
    }

    public vector3 m_position;
    public bool m_bWalkable;

    public TileNode(bool _walkable, Vector3 worldPoint)
    {
        m_position.x = worldPoint.x;
        m_position.y = worldPoint.y;
        m_position.z = worldPoint.z;
        m_bWalkable = _walkable;
    }

    public Vector3 GetPosition()
    {
        //return new Vector3(m_position.x, m_position.y, m_position.z);
        return new Vector3(m_position.x, 2, m_position.z);
    }

    public void SetWalkable(bool _walkable)
    {
        m_bWalkable = _walkable;
    }
}
