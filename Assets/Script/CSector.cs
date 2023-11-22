using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSector
{
    struct sector
    {
        public int x_max;
        public int x_min;
        public int y_max;
        public int y_min;
    }

    sector[] sectors;

    private int max;

    public void Init()
    {
        max = 43;

        sectors = new sector[max * max];

        for(int y = 0; y < max; y++)
        {
            for(int x = 0; x < max; x++)
            {
                sectors[x + (y * max)].x_min = 0 + (x * 6);
                sectors[x + (y * max)].x_max = 6 + (x * 6);
                sectors[x + (y * max)].y_min = 0 + (6 * y);
                sectors[x + (y * max)].y_max = 6 + (6 * y);
            }
        }
    }

    public int Contains(Vector3 _position)
    {
        for(int i = 0; i < 1849; i++)
        {
            if(sectors[i].x_max > _position.x && sectors[i].x_min < _position.x && sectors[i].y_max > _position.z && sectors[i].y_min < _position.z )
            {
                return i;
            }
        }

        return 0;
    }
}
