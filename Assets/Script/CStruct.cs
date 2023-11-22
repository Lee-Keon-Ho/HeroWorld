using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class CStruct
{
    public struct sCharacterList
    {
        public int c1_name_Len;
        public string c1_name;
        public int c1;
        public int c_1_Level;

        public int c2_name_Len;
        public string c2_name;
        public int c2;
        public int c_2_Level;

        public int c3_name_Len;
        public string c3_name;
        public int c3;
        public int c_3_Level;
    }

    public struct sCharacterInfo
    {
        public int level;
        public int curHp;
        public int maxHp;
        public int curMp;
        public int maxMp;
        public int curExp;
        public Vector3 position;
    }
}
