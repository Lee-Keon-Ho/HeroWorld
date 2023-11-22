using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class TileGrid : MonoBehaviour
{
    public LayerMask m_checkLayer;
    public Vector2Int m_gridSize;
    public GameObject[] m_monster1;
    public GameObject[] m_monster2;
    public GameObject[] m_monster2_1;
    public GameObject[] m_monster3;
    public GameObject[] m_monster3_1;
    public GameObject[] m_monster3_2;
    public GameObject[] m_monster3_3;
    public GameObject[] m_monster4_1;
    public GameObject[] m_monster4_2;
    public GameObject[] m_monster4_3;
    public GameObject[] m_monster4_4;
    public GameObject[] m_monster4_5;
    public Vector2Int m_rangeMin1; 
    public Vector2Int m_rangeMax1;
    public Vector2Int m_rangeMin2;
    public Vector2Int m_rangeMax2;
    public Vector2Int m_rangeMin2_1;
    public Vector2Int m_rangeMax2_1;
    public Vector2Int m_rangeMin3;
    public Vector2Int m_rangeMax3;
    public Vector2Int m_rangeMin3_1;
    public Vector2Int m_rangeMax3_1;
    public Vector2Int m_rangeMin3_2;
    public Vector2Int m_rangeMax3_2;
    public Vector2Int m_rangeMin3_3;
    public Vector2Int m_rangeMax3_3;
    public Vector2Int m_rangeMin4_1;
    public Vector2Int m_rangeMax4_1;
    public Vector2Int m_rangeMin4_2;
    public Vector2Int m_rangeMax4_2;
    public Vector2Int m_rangeMin4_3;
    public Vector2Int m_rangeMax4_3;
    public Vector2Int m_rangeMin4_4;
    public Vector2Int m_rangeMax4_4;
    public Vector2Int m_rangeMin4_5;
    public Vector2Int m_rangeMax4_5;

    public TileNode[,] m_grid;
    public bool[,] m_bGrid;
    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        //m_checkLayer = LayerMask.NameToLayer("walkable");
        m_grid = new TileNode[m_gridSize.x, m_gridSize.y];
        m_bGrid = new bool[m_gridSize.x, m_gridSize.y];

        Vector3 worldPoint;
        
        for (int y = 0; y < m_gridSize.y; y++)
        {
            for(int x = 0; x < m_gridSize.x; x++)
            {
                worldPoint = Vector3.right * ( 1 + x ) + Vector3.forward * ( 1 + y );
                bool walkable = !(Physics.CheckSphere(worldPoint, 1.0f, m_checkLayer));
                m_grid[y,x] = new TileNode(walkable, worldPoint);
                m_bGrid[y, x] = walkable;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(m_gridSize.x, 1, m_gridSize.y));
        if (m_grid != null)
        {
            foreach (TileNode n in m_grid)
            {
                Gizmos.color = (n.m_bWalkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.GetPosition(), Vector3.one * 0.9f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.9f);
    }

    public Vector2Int GetGridSize() { return m_gridSize; }

    public bool[,] GetGrid() { return m_bGrid; }

    public void SetWalkable(int _x, int _y, bool _bool)
    {
        m_grid[_y, _x].SetWalkable(_bool);
    }
}