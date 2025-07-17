using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using TMPro;

public class CPacketHandler : MonoBehaviour
{
    enum ePacket
    {
        CS_PT_LATENCY,
        CS_PT_LOGIN,
        CS_PT_INFIELD,
        CS_PT_DUMMY,
        CS_PT_NEWUSER,
        CS_PT_MOVEUSER,
        CS_PT_ARRIVE,
        CS_PT_USEROUT,
        CS_PT_DELETEUSERSOUTOFSECTOR,
        CS_PT_INSECTOR,
        CS_PT_OUTSECTOR,
        CS_PT_NEWSECTORUSER,
        CS_PT_PLAYERIDLEATTACK, 
        CS_PT_PLAYERMOVEATTACK,
        CS_PT_USERPOSITION = 18,
        CS_PT_AREANEWUSER,
        CS_PT_MONSTERMOVE,
        CS_PT_CREATEMONSTER,
        CS_PT_MONSTEROUTSECTOR,
        CS_PT_MONSTERINSECTOR,
        CS_PT_OUTSECTORMONSTER,
        CS_PT_INSECTORMONSTER,
        CS_PT_HIT_MONSTER,
        CS_PT_ATTACK_MONSTER,
        CS_PT_DIE_MONSTER,
        CS_PT_REGEN_MONSTER,
        CS_PT_PLAYER_EXP,
        CS_PT_PALYER_HIT,
    }

    CSocket m_socket;

    MemoryStream memoryStream;
    BinaryReader binaryReader;

    Dictionary<int, GameObject> players;
    Dictionary<int, GameObject> monsters;

    bool logOut = false;

    int userCount;

    int userCountAll = 0;
    int inSectorUserCount = 0;

    float latency = 0f;

    GUIStyle style = new GUIStyle();
    float x = 5f;
    float y = 5f;
    float w = Screen.width;
    float h = 20f;

    private void Awake()
    {
        players = new Dictionary<int, GameObject>();
        monsters = new Dictionary<int, GameObject>();

        style.normal.textColor = Color.red;
        style.fontSize = 20;
    }

    //public void OnGUI()
    //{
    //    GUI.Label(new Rect(x, y, w, h), "ALL UserCount : " + userCountAll, style);
    //    GUI.Label(new Rect(x, y + 20, w, h), "Sector UserCount : " + inSectorUserCount, style);
    //}

    public void Handle(byte[] _Buffer)
    {
        memoryStream = new MemoryStream(_Buffer);
        binaryReader = new BinaryReader(memoryStream);

        memoryStream.Position = 0;

        ushort type = binaryReader.ReadUInt16();

        switch (type)
        {
            case 0:
                Latency();
                break;
            case 1:
                Login();
                break;
            case 2:
                InField();
                break;
            case 3:
                NextField();
                break;
            case 4:
                NewUser();
                break;
            case 5:
                MoveUser();
                break;
            case 6:
                Arrive();
                break;
            case 7:
                UserOut();
                break;
            case 8:
                DeleteUsersOutOfSector();
                break;
            case 9:
                InSector();
                break;
            case 10:
                OutSector();
                break;
            case 11:
                NewSectorUser();
                break;
            case 12:
                PlayerIdleAttack();
                break;
            case 13:
                PlayerMoveAttack();
                break;
            case 14:
                PlayerChatting();
                break;
            case 15:
                Notice();
                break;
            case 16:
                AacherIdleAttack();
                break;
            case 17:
                AacherMoveAttack();
                break;
            case 18:
                AreaNewUser();
                break;
            case 19:
                MonsterMove();
                break;
            case 20:
                CreateMonster();
                break;
            case 21:
                MonsterOutSector();
                break;
            case 22:
                MonsterInSector();
                break;
            case 23:
                OutSectorMonster();
                break;
            case 24:
                InSectorMonster();
                break;
            case 25:
                Hit_Monster();
                break;
            case 26:
                Attack_Monster();
                break;
            case 27:
                Die_Monster();
                break;
            case 28:
                Regen_Monster();
                break;
            case 29:
                PlayerExp();
                break;
            case 30:
                PlayerHit();
                break;
            case 31:
                PlayerLevelUp();
                break;
            case 32:
                HeartBeat();
                break;
            case 33:
                WrapDeleteUser();
                break;
            case 34:
                WRAP();
                break;
            case 35:
                ChannelChange();
                break;
            case 98:
                LogOut();
                break;
            case 99:
                UserCount();
                break;
            default:
                break;
        }
        //return size;
    }

    public void Latency()
    {
        CApp app = FindAnyObjectByType<CApp>();
        latency = Time.time - binaryReader.ReadSingle();

        if (latency > 0.03)
        {
            Debug.Log("Latency : " + latency);
            Debug.Log("FPS : " + 1.0 / app.GetFPS());
        }
    }
    public void Login()
    {
        userCount = binaryReader.ReadUInt16();
        byte[] Buffer = new byte[28];
        Buffer = binaryReader.ReadBytes(28);
        string name = System.Text.Encoding.Unicode.GetString(Buffer);
        int type = binaryReader.ReadInt32();
        int level = binaryReader.ReadInt32();
        int curHp = binaryReader.ReadInt32();
        int maxHp = binaryReader.ReadInt32();
        int curMp = binaryReader.ReadInt32();
        int maxMp = binaryReader.ReadInt32();
        Vector3 position;
        position.x = binaryReader.ReadSingle();
        position.y = binaryReader.ReadSingle();
        position.z = binaryReader.ReadSingle();
        int curExp = binaryReader.ReadInt32();
        int damageMin = binaryReader.ReadInt32();
        int damageMax = binaryReader.ReadInt32();

        CDataManager.Instance.SetInfo(type - 1, level, curHp, maxHp, curMp, maxMp, curExp, position, (ushort)userCount);
        CDataManager.Instance.SetName(name);
    }

    public void InField()
    {
        userCount = binaryReader.ReadUInt16();

        if (userCount == 0) return;

        int userNumber;
        int userState;
        int userCharacter;
        byte[] Buffer = new byte[28];
        Vector3 startPosition;
        Vector3 EndPosition;
        float y;

        CObjectManager objManager = CObjectManager.instance;
        //objManager.ResetMonster();
        //players.Clear();
        for (int i = 0; i < userCount; i++)
        {
            userNumber = binaryReader.ReadUInt16();
            userState = binaryReader.ReadUInt16();
            userCharacter = binaryReader.ReadUInt16();
            y = binaryReader.ReadSingle();
            Buffer = binaryReader.ReadBytes(28);

            startPosition.x = binaryReader.ReadSingle();
            startPosition.y = binaryReader.ReadSingle();
            startPosition.z = binaryReader.ReadSingle();
            EndPosition.x = binaryReader.ReadSingle();
            EndPosition.y = binaryReader.ReadSingle();
            EndPosition.z = binaryReader.ReadSingle();

            if (userNumber == CDataManager.Instance.GetIndex()) continue;

            GameObject player = objManager.GetPlayer();

            player.SetActive(true);
            CUserMove userMove = player.GetComponent<CUserMove>();
            player.transform.Find("Name").GetComponent<TextMeshPro>().text = System.Text.Encoding.Unicode.GetString(Buffer);
            userMove.SetUserNumber(userNumber);
            userMove.SetCharacter(userCharacter - 1);
            player.transform.position = startPosition;
            player.transform.GetChild(1).transform.localEulerAngles = new Vector3(0, y, 0);
            userMove.SetDestination(startPosition, EndPosition);

            players.Add(userNumber, player);
            players[userNumber].SetActive(true);
        }
    }

    private void WRAP()
    {
        int index = binaryReader.ReadUInt16();

        CApp app = FindAnyObjectByType<CApp>();
        app.NextField(index);

        CObjectManager.instance.ResetMonster();
        players.Clear();
        monsters.Clear();

        if (index == 0)
        {
            CSceneManager.Instance.OnChangeScene("ForestTown");
        }
        else if(index == 1)
        {
            CSceneManager.Instance.OnChangeScene("ForestField");
        }
        else if(index == 2)
        {
            CSceneManager.Instance.OnChangeScene("WinterField");
        }
    }

    private void ChannelChange()
    {
        int field = CDataManager.Instance.GetFieldIndex();
        CApp app = FindAnyObjectByType<CApp>();

        if (field == 0) CSceneManager.Instance.OnChangeScene("ForestTown");
        else if (field == 1) CSceneManager.Instance.OnChangeScene("ForestField");
        else if (field == 2) CSceneManager.Instance.OnChangeScene("WinterField");

        CObjectManager.instance.ResetMonster();
        players.Clear();
        monsters.Clear();
        app.Init();
    }

    public void NextField()
    {
        Vector3 position;

        position.x = binaryReader.ReadSingle();
        position.y = binaryReader.ReadSingle();
        position.z = binaryReader.ReadSingle();

        CDataManager.Instance.SetPosition(position);
        CObjectManager.instance.ResetMonster();
        monsters.Clear();
        players.Clear();

        CApp app = FindAnyObjectByType<CApp>();
        app.InField();
    }

    public void WrapDeleteUser()
    {
        int index = binaryReader.ReadUInt16();

        if(players.ContainsKey(index))
        {
            players[index].gameObject.SetActive(false);
            players.Remove(index);
        }
    }

    public void MoveUser()
    {
        ushort number = binaryReader.ReadUInt16();

        if (players.ContainsKey(number))
        {
            Vector3 startPosition;
            Vector3 EndPosition;

            startPosition.x = binaryReader.ReadSingle();
            startPosition.y = binaryReader.ReadSingle();
            startPosition.z = binaryReader.ReadSingle();
            EndPosition.x = binaryReader.ReadSingle();
            EndPosition.y = binaryReader.ReadSingle();
            EndPosition.z = binaryReader.ReadSingle();

            players[number].GetComponent<CUserMove>().SetDestination(startPosition, EndPosition);
        }
    }

    public void NewUser() // 새로 입장한 플레이어
    {
        Vector3 position;
        byte[] Buffer = new byte[28];
        ushort userNumber = binaryReader.ReadUInt16();

        if (userNumber == CDataManager.Instance.GetIndex()) return;

        int userCharacter = binaryReader.ReadUInt16();
        Buffer = binaryReader.ReadBytes(28);
        position.x = binaryReader.ReadSingle();
        position.y = binaryReader.ReadSingle();
        position.z = binaryReader.ReadSingle();

        GameObject player = CObjectManager.instance.GetPlayer();

        CUserMove userMove = player.GetComponent<CUserMove>();
        player.transform.Find("Name").GetComponent<TextMeshPro>().text = System.Text.Encoding.Unicode.GetString(Buffer);
        userMove.SetUserNumber(userNumber);
        userMove.SetCharacter(userCharacter - 1);
        player.transform.position = position;
        userMove.SetDestination(position, position);

        players.Add(userNumber, player);
        player.SetActive(true);
    }
    public void Arrive()
    {
        ushort userNumber = binaryReader.ReadUInt16();

        if (players.ContainsKey(userNumber))
        {
            float rotationY = binaryReader.ReadSingle();
            Vector3 position;

            position.x = binaryReader.ReadSingle();
            position.y = binaryReader.ReadSingle();
            position.z = binaryReader.ReadSingle();

            players[userNumber].GetComponent<CUserMove>().Arrive(position, rotationY);
        }
    }
    public void UserOut()
    {
        ushort userNumber = binaryReader.ReadUInt16();

        if (players.ContainsKey(userNumber))
        {
            //CObjectManager.instance.ReturnPlayer(players[userNumber]);
            //players.Remove(userNumber);
            players[userNumber].SetActive(false);
        }
    }

    private void DeleteUsersOutOfSector()
    {
        int userCount = binaryReader.ReadUInt16();
        int userNumber;
        float y;

        CObjectManager objManager = CObjectManager.instance;

        for (int i = 0; i < userCount; i++)
        {
            userNumber = binaryReader.ReadUInt16();

            if (players.ContainsKey(userNumber))
            {
                //objManager.ReturnPlayer(players[userNumber]);
                //players.Remove(userNumber);
                players[userNumber].SetActive(false);
            }
        }
    }

    void InSector()
    {
        int userNumber = binaryReader.ReadUInt16();

        Vector3 startPosition;
        Vector3 EndPosition;

        startPosition.x = binaryReader.ReadSingle();
        startPosition.y = binaryReader.ReadSingle();
        startPosition.z = binaryReader.ReadSingle();

        EndPosition.x = binaryReader.ReadSingle();
        EndPosition.y = binaryReader.ReadSingle();
        EndPosition.z = binaryReader.ReadSingle();

        //GameObject player = CObjectManager.instance.GetPlayer();
        if(players.ContainsKey(userNumber))
        {
            players[userNumber].SetActive(true);
            players[userNumber].transform.position = startPosition;
            players[userNumber].GetComponent<CUserMove>().SetUserNumber(userNumber);
            players[userNumber].GetComponent<CUserMove>().SetDestination(startPosition, EndPosition);
            players[userNumber].GetComponent<CUserMove>().psStop();
        }
    }

    void OutSector()
    {
        int userIndex = binaryReader.ReadUInt16();

        if (players.ContainsKey(userIndex))
        {
            //CObjectManager.instance.ReturnPlayer(players[userNumber]);
            //players.Remove(userNumber);
            players[userIndex].SetActive(false);
        }
    }

    private void NewSectorUser()
    {
        int userCount = binaryReader.ReadUInt16();
        int userIndex;
        int userState;
        Vector3 startPosition;
        Vector3 EndPosition;
        float y;

        for (int i = 0; i < userCount; i++)
        {
            userIndex = binaryReader.ReadUInt16();

            //GameObject player = objManager.GetPlayer();

            userState = binaryReader.ReadUInt16();
            y = binaryReader.ReadSingle();
            startPosition.x = binaryReader.ReadSingle();
            startPosition.y = binaryReader.ReadSingle();
            startPosition.z = binaryReader.ReadSingle();
            EndPosition.x = binaryReader.ReadSingle();
            EndPosition.y = binaryReader.ReadSingle();
            EndPosition.z = binaryReader.ReadSingle();

            if(players.ContainsKey(userIndex))
            {
                players[userIndex].SetActive(true);
                if (userState == 0)
                {
                    players[userIndex].transform.position = EndPosition;
                    players[userIndex].GetComponent<CUserMove>().SetUserNumber(userIndex);
                    players[userIndex].GetComponent<CUserMove>().SetGoalPosition(EndPosition);
                }
                if (userState == 1)
                {
                    players[userIndex].transform.position = startPosition;
                    players[userIndex].GetComponent<CUserMove>().SetUserNumber(userIndex);
                    players[userIndex].GetComponent<CUserMove>().SetDestination(startPosition, EndPosition);
                }
                players[userIndex].GetComponent<CUserMove>().psStop();
            }
            //players.Add(userNumber, player);
        }
    }
    private void PlayerIdleAttack()
    {
        int userIndex = binaryReader.ReadUInt16();
        float _y = binaryReader.ReadSingle();

        if (players.ContainsKey(userIndex))
        {
            players[userIndex].GetComponent<CUserMove>().IdleAttack(_y);
        }
    }
    private void PlayerMoveAttack()
    {
        int userIndex = binaryReader.ReadUInt16();
        float _y = binaryReader.ReadSingle();
        Vector3 position;

        position.x = binaryReader.ReadSingle();
        position.y = binaryReader.ReadSingle();
        position.z = binaryReader.ReadSingle();

        if (players.ContainsKey(userIndex))
        {
            players[userIndex].GetComponent<CUserMove>().MoveAttack(position, _y);
        }
    }

    private void PlayerChatting()
    {
        int index = binaryReader.ReadUInt16();
        string name = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(28));
        string str = name.Replace("\0", string.Empty);
        string chat = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(28));

        Chatting chatting = FindAnyObjectByType<Chatting>();
        if (CDataManager.Instance.CompareName(name))
        {
            chatting.MyChat(chat);
        }
        else
        {
            players[index].GetComponent<CUserMove>().OnChatting(chat);
        }

        chatting.InputChat(str + " : " + chat);
    }

    private void Notice()
    {
        string str = System.Text.Encoding.Unicode.GetString(binaryReader.ReadBytes(28));
        Chatting chatting = FindAnyObjectByType<Chatting>();
        chatting.Notice(str);
    }

    private void AacherIdleAttack()
    {
        int userIndex = binaryReader.ReadUInt16();
        float _y = binaryReader.ReadSingle();

        if (players.ContainsKey(userIndex))
        {
            players[userIndex].GetComponent<CUserMove>().IdleAttack(_y);
        }
    }

    private void AacherMoveAttack()
    {
        int userIndex = binaryReader.ReadUInt16();
        float _y = binaryReader.ReadSingle();
        Vector3 position;

        position.x = binaryReader.ReadSingle();
        position.y = binaryReader.ReadSingle();
        position.z = binaryReader.ReadSingle();

        if (players.ContainsKey(userIndex))
        {
            players[userIndex].GetComponent<CUserMove>().MoveAttack(position, _y);
        }
    }
    private void Hit_Monster()
    {
        int index = binaryReader.ReadUInt16();
        int curHp = binaryReader.ReadUInt16();
        int damage = binaryReader.ReadUInt16();
        if (monsters.ContainsKey(index))
        {
            monsters[index].GetComponent<CMonster>().Hit(curHp, damage);
        }
    }

    private void Attack_Monster()
    {
        Vector3 position;
        int index = binaryReader.ReadUInt16();
        position.x = binaryReader.ReadSingle();
        position.y = binaryReader.ReadSingle();
        position.z = binaryReader.ReadSingle();

        if (monsters.ContainsKey(index))
        {
            monsters[index].GetComponent<CMonster>().Attack(position);
        }
    }

    private void Die_Monster()
    {
        int index = binaryReader.ReadUInt16();
        int damage = binaryReader.ReadUInt16();

        if (monsters.ContainsKey(index))
        {
            monsters[index].GetComponent<CMonster>().Die(damage);
            monsters[index].GetComponent<BoxCollider>().enabled = false;
        }
    }
    private void Regen_Monster()
    {
        int index = binaryReader.ReadUInt16();
        int type = binaryReader.ReadUInt16();
        int level = binaryReader.ReadUInt16();
        Vector3 position;

        position.x = binaryReader.ReadSingle();
        position.y = binaryReader.ReadSingle();
        position.z = binaryReader.ReadSingle();

        GameObject monster = CObjectManager.instance.GetMonster(type);

        monster.transform.position = position;
        monsters[index].SetActive(true);
        monsters[index].GetComponent<BoxCollider>().enabled = true;
        monsters[index].GetComponent<CMonster>().SetInfo(index, type, level);
        monsters[index].GetComponent<CMonster>().SetDestination(position);

    }

    private void UserCount()
    {
        userCountAll = binaryReader.ReadUInt16();
        inSectorUserCount = binaryReader.ReadUInt16();
    }

    private void AreaNewUser()
    {
        int userNumber = binaryReader.ReadUInt16();

        if (players.ContainsKey(userNumber))
        {
            players[userNumber].SetActive(true);
        }
    }

    private void UserPosition()
    {
        int userNumber = binaryReader.ReadUInt16();

        if (players.ContainsKey(userNumber))
        {
            Vector3 vector;
            vector.x = binaryReader.ReadSingle();
            vector.y = binaryReader.ReadSingle();
            vector.z = binaryReader.ReadSingle();

            players[userNumber].transform.position = vector;
        }
    }

    private void MonsterMove()
    {
        int index = binaryReader.ReadUInt16();
        int type = binaryReader.ReadUInt16();

        Vector3 currentPosition;
        Vector3 destinationPosition;

        currentPosition.x = binaryReader.ReadSingle();
        currentPosition.y = binaryReader.ReadSingle();
        currentPosition.z = binaryReader.ReadSingle();

        destinationPosition.x = binaryReader.ReadSingle();
        destinationPosition.y = binaryReader.ReadSingle();
        destinationPosition.z = binaryReader.ReadSingle();

        if (monsters.ContainsKey(index))
        {
            monsters[index].SetActive(true);
            if (type == 1) // override
            {
                monsters[index].GetComponent<BigSlimeLeaf>().SetDestination(destinationPosition);
            }
            else
            {
                monsters[index].GetComponent<CMonster>().SetDestination(destinationPosition);
            }
                
        }
        else
        {
            GameObject monster = CObjectManager.instance.GetMonster(type);
            monsters.Add(index, monster);
            if (type == 1) // override
            {
                monsters[index].GetComponent<BigSlimeLeaf>().SetDestination(destinationPosition);
            }
            else
            {
                monsters[index].GetComponent<CMonster>().SetDestination(destinationPosition);
            }
        }

    }

    private void CreateMonster()
    {
        Vector3 currentPosition;
        Vector3 destinationPosition;
        int index = binaryReader.ReadUInt16();
        int type = binaryReader.ReadUInt16();
        int level = binaryReader.ReadUInt16();

        currentPosition.x = binaryReader.ReadSingle();
        currentPosition.y = binaryReader.ReadSingle();
        currentPosition.z = binaryReader.ReadSingle();

        destinationPosition.x = binaryReader.ReadSingle();
        destinationPosition.y = binaryReader.ReadSingle();
        destinationPosition.z = binaryReader.ReadSingle();

        CObjectManager.instance.CreateMonster(1, type);
        GameObject monster = CObjectManager.instance.GetMonster(type);

        monster.transform.position = currentPosition;
        
        if (type == 1) // override
        {
            monsters.Add(index, monster);
            monsters[index].SetActive(true);
            monsters[index].GetComponent<BigSlimeLeaf>().SetInfo(index, type, level);
            monsters[index].GetComponent<BigSlimeLeaf>().SetDestination(destinationPosition);
        }
        else if(type == 5)
        {
            monsters.Add(index, monster);
            monsters[index].SetActive(true);
            monsters[index].GetComponent<CGoblin>().SetInfo(index, type, level);
            monsters[index].GetComponent<CGoblin>().SetDestination(destinationPosition);
        }
        else
        {
            monsters.Add(index, monster);
            monsters[index].SetActive(true);
            monsters[index].GetComponent<CMonster>().SetInfo(index, type, level);
            monsters[index].GetComponent<CMonster>().SetDestination(destinationPosition);
        }
    }

    public void MonsterOutSector()
    {
        int index = binaryReader.ReadUInt16();

        if (monsters.ContainsKey(index))
        {
            monsters[index].SetActive(false);
        }
    }

    public void MonsterInSector()
    {
        int index = binaryReader.ReadUInt16();
        int type = binaryReader.ReadUInt16();

        Vector3 currentPosition;
        Vector3 destinationPosition;

        currentPosition.x = binaryReader.ReadSingle();
        currentPosition.y = binaryReader.ReadSingle();
        currentPosition.z = binaryReader.ReadSingle();

        destinationPosition.x = binaryReader.ReadSingle();
        destinationPosition.y = binaryReader.ReadSingle();
        destinationPosition.z = binaryReader.ReadSingle();

        if (monsters.ContainsKey(index))
        {
            monsters[index].SetActive(true);
        }
        else
        {
            GameObject monster = CObjectManager.instance.GetMonster(type);
            monster.GetComponent<CMonster>().SetInfo(index, type, 1);
            monsters.Add(index, monster);
        }

        monsters[index].transform.position = currentPosition;

        if(type == 1) // override
        {
            monsters[index].GetComponent<BigSlimeLeaf>().SetDestination(destinationPosition);
        }
        
    }

    private void OutSectorMonster()
    {
        int count = binaryReader.ReadUInt16();
        int index;

        for (int i = 0; i < count; i++)
        {
            index = binaryReader.ReadUInt16();

            if (monsters.ContainsKey(index))
            {
                monsters[index].SetActive(false);
            }
        }
    }

    private void InSectorMonster()
    {
        int size = binaryReader.ReadUInt16();
        int index;
        int type;
        Vector3 start;
        Vector3 destination;

        for(int i = 0; i < size; i++)
        {
            index = binaryReader.ReadUInt16();
            type = binaryReader.ReadUInt16();
            start.x = binaryReader.ReadSingle();
            start.y = binaryReader.ReadSingle();
            start.z = binaryReader.ReadSingle();
            destination.x = binaryReader.ReadSingle();
            destination.y = binaryReader.ReadSingle();
            destination.z = binaryReader.ReadSingle();

            if (!monsters.ContainsKey(index))
            {
                GameObject monster = CObjectManager.instance.GetMonster(type);
                monster.GetComponent<CMonster>().SetInfo(index, type, 1);
                monsters.Add(index, monster);
            }

            monsters[index].SetActive(true);
            monsters[index].transform.position = start;
            monsters[index].GetComponent<CMonster>().SetDestination(destination);
        }
    }

    private void PlayerExp()
    {
        float exp = binaryReader.ReadSingle();

        CClickMoveMent cClick = FindAnyObjectByType<CClickMoveMent>();
        cClick.SetExp(exp);
        //myPlayer.GetComponent<CClickMoveMent>().SetExp(exp);
    }

    private void PlayerHit()
    {
        int curHp = binaryReader.ReadUInt16();

        CClickMoveMent cClick = FindAnyObjectByType<CClickMoveMent>();
        cClick.SetCurHp(curHp);
        //myPlayer.GetComponent<CClickMoveMent>().SetCurHp(curHp);
    }

    private void PlayerLevelUp()
    {
        int level = binaryReader.ReadUInt16();
        float curExp = binaryReader.ReadSingle();
        float maxExp = binaryReader.ReadSingle();

        CClickMoveMent cClick = FindAnyObjectByType<CClickMoveMent>();
        cClick.SetLevel(level, curExp, maxExp);
        //myPlayer.GetComponent<CClickMoveMent>().SetLevel(level, curExp, maxExp);
    }

    private void HeartBeat()
    {
        CApp app = FindAnyObjectByType<CApp>();
        app.SendHeartBeat();
    }

    private void LogOut()
    {
        CApp app = FindAnyObjectByType<CApp>();
        CLoginApp loginApp = FindAnyObjectByType<CLoginApp>();
        logOut = true;
#if UNITY_EDITOR
        app.SocketDelete();
        loginApp.SocketDelete();
        UnityEditor.EditorApplication.isPlaying = false;
#else
        app.SocketDelete();
        loginApp.SocketDelete();
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public float GetLatency() { return latency; }
    public int GetUserCountAll() { return userCountAll; }
    public int GetSectorUserCount() { return inSectorUserCount; }

    public bool GetLogOut() { return logOut; }
}
