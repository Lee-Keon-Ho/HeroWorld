using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CObjectManager : MonoBehaviour
{
    public static CObjectManager instance;

    public GameObject playerPrefab;
    public GameObject[] monsterPrefab;

    List<GameObject> players = new List<GameObject>();
    List<GameObject> monsters = new List<GameObject>();
    List<bool> OnPlayer = new List<bool>();
    List<bool> OnMonster = new List<bool>(); // map으로

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        //CreatePlayer(100);
        //CreateMonster(200, 0);
        //CreateMonster(120, 1);
        //CreateMonster(200, 2);
        //CreateMonster(50, 3);
        //CreateMonster(50, 4);
    }

    void CreatePlayer(int _count)
    {
        for(int i = 0; i < _count; i++)
        {
            GameObject player = Instantiate(playerPrefab) as GameObject;
            player.transform.parent = transform;
            players.Add(player);
            OnPlayer.Add(false);
        }
    }

    public void CreateMonster(int _count, int _type)
    {
        for (int i = 0; i < _count; i++)
        {
            GameObject monster = Instantiate(monsterPrefab[_type]);
            monster.transform.parent = transform;
            monsters.Add(monster);
            OnMonster.Add(false);
        }
    }
    
    public GameObject GetPlayer()
    {
        GameObject obj = null;

        for (int i = 0; i < players.Count; i++)
        {
            if (!OnPlayer[i])
            {
                obj = players[i];
                OnPlayer[i] = true;
                break;
            }
        }

        if(obj == null)
        {
            GameObject newPlayer = Instantiate(playerPrefab) as GameObject;
            newPlayer.transform.parent = transform;
            newPlayer.SetActive(false);
            players.Add(newPlayer);
            OnPlayer.Add(true);
            obj = newPlayer;
        }

        return obj;
    }

    public GameObject GetMonster(int _type)
    {
        GameObject obj = null;

        for (int i = 0; i < monsters.Count; i++)
        {
            if (!OnMonster[i])
            {
                obj = monsters[i];  // 여기도 type에 맞춰서 줘야한다
                OnMonster[i] = true;
                break;
            }
        }

        if (obj == null)
        {
            GameObject newMonster = Instantiate(monsterPrefab[_type]) as GameObject;
            newMonster.transform.parent = transform;
            newMonster.SetActive(false);
            monsters.Add(newMonster);
            OnMonster.Add(true);
            obj = newMonster;
        }

        return obj;
    }

    public void ResetMonster()
    {
        GameObject obj = null;
        for (int i = 0; i < monsters.Count; i++)
        {
            if (OnMonster[i])
            {
                obj = monsters[i];  // 여기도 type에 맞춰서 줘야한다
                OnMonster[i] = false;

                if (obj != null)
                {
                    Destroy(obj);
                }
            }
        }
        
        monsters.Clear();
        OnMonster.Clear();

        GameObject playerObj = null;
        for (int i = 0; i < players.Count; i++)
        {
            if (OnPlayer[i])
            {
                playerObj = players[i]; 
                OnPlayer[i] = false;

                if (playerObj != null)
                {
                    Destroy(playerObj);
                }
            }
        }

        players.Clear();
        OnPlayer.Clear();
    }

    public void ChannelChange()
    {
        Destroy(gameObject);
    }
}