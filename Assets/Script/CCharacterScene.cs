using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class CCharacterScene : MonoBehaviour
{
    CStruct.sCharacterList sCharacters;
    private Camera camera;
    public Canvas create;
    public Button createButton;
    public Button deleteButton;
    public Button[] characterButton;
    public Button CreateCanvasCreateButton;
    public Button CreateCanvasExitButton;
    public Button NameCheckButton;
    public Button NameImageExitButton;
    public Image CreateImage;
    public Image DeleteImage;
    public Image character1;
    public Image character2;
    public Image character3;
    public Image doubleCheckNameImage;
    public Image doubleCheckImage;
    public TMP_InputField InputDoubleCheck;
    public GameObject[] info1;
    public GameObject[] info2;
    public GameObject[] info3;
    public ParticleSystem[] ps;
    private GameObject[] characterObject;
    private int CreateCharacterindex;
    private int DeleteCharacterindex;
    private int characterCount;
    public Color color;
    public GameObject[] test;
    void Start()
    {
        characterObject = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            ps[i].Stop();
        }
        camera = Camera.main;
        sCharacters = FindAnyObjectByType<CWorldApp>().GetCharacterList();
        CreateCharacterindex = 0;
        DeleteCharacterindex = 0;
        characterCount = 0;
        if (sCharacters.c1 > 0)
        {
            GameObject gameObject = GameObject.Find("character1");
            gameObject.transform.GetChild(sCharacters.c1 - 1).gameObject.SetActive(true);
            characterObject[0] = gameObject.transform.GetChild(sCharacters.c1 - 1).gameObject;
            info1[0].SetActive(true);
            info1[1].GetComponent<TextMeshProUGUI>().text = "Lv." + sCharacters.c_1_Level.ToString();
            SetJopText(info1[2].GetComponent<TextMeshProUGUI>(), sCharacters.c1);
            info1[3].GetComponent<TextMeshProUGUI>().text = sCharacters.c1_name;
            characterCount++;
        }
        if (sCharacters.c2 > 0)
        {
            GameObject gameObject = GameObject.Find("character2");
            gameObject.transform.GetChild(sCharacters.c2 - 1).gameObject.SetActive(true);
            characterObject[1] = gameObject.transform.GetChild(sCharacters.c2 - 1).gameObject;
            info2[0].SetActive(true);
            info2[1].GetComponent<TextMeshProUGUI>().text = "Lv." + sCharacters.c_2_Level.ToString();
            SetJopText(info2[2].GetComponent<TextMeshProUGUI>(), sCharacters.c2);
            info2[3].GetComponent<TextMeshProUGUI>().text = sCharacters.c2_name;
            characterCount++;
        }
        if (sCharacters.c3 > 0)
        {
            GameObject gameObject = GameObject.Find("character3");
            gameObject.transform.GetChild(sCharacters.c3 - 1).gameObject.SetActive(true);
            characterObject[2] = gameObject.transform.GetChild(sCharacters.c3 - 1).gameObject;
            info3[0].SetActive(true);
            info3[1].GetComponent<TextMeshProUGUI>().text = "Lv." + sCharacters.c_3_Level.ToString();
            SetJopText(info3[2].GetComponent<TextMeshProUGUI>(), sCharacters.c3);
            info3[3].GetComponent<TextMeshProUGUI>().text = sCharacters.c3_name;
            characterCount++;
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 1000f))
            {
                ParticlePlay(hit.transform.tag);
            }
        }
    }

    public void OnMainCreate()
    {
        if (characterCount >= 3) return;
        
        createButton.enabled = false;
        deleteButton.enabled = false;
        create.gameObject.SetActive(true);
    }

    public void OnExit()
    {
        CreateCharacterindex = 0;
        createButton.enabled = true;
        deleteButton.enabled = true;
        character1.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f);
        character2.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f);
        character3.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f);
        create.gameObject.SetActive(false);
    }

    public void OnCharacter1()
    {
        CreateCharacterindex = 1;
        character1.GetComponent<Outline>().effectColor = new Color(0f, 255f, 255f);
        character2.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f);
        character3.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f);
    }
    public void OnCharacter2()
    {
        CreateCharacterindex = 2;
        character1.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f);
        character2.GetComponent<Outline>().effectColor = new Color(0f, 255f, 255f);
        character3.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f);
    }
    public void OnCharacter3()
    {
        CreateCharacterindex = 3;
        character1.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f);
        character2.GetComponent<Outline>().effectColor = new Color(0f, 0f, 0f);
        character3.GetComponent<Outline>().effectColor = new Color(0f, 255f, 255f);
    }

    public void OnDoubleCheckImage()
    {
        if(CreateCharacterindex > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                characterButton[i].enabled = false;
            }
            CreateCanvasCreateButton.enabled = false;
            CreateCanvasExitButton.enabled = false;
            doubleCheckNameImage.gameObject.SetActive(true);
        }
    }

    public void OnDoubleCheckImageExit()
    {
        for (int i = 0; i < 3; i++)
        {
            characterButton[i].enabled = true;
        }
        CreateCanvasCreateButton.enabled = true;
        CreateCanvasExitButton.enabled = true;
        doubleCheckNameImage.gameObject.SetActive(false);
    }
    public void OnDoubleCheck()
    {
        int len = InputDoubleCheck.text.Length;
        if (len >= Constants.name_min && len <= Constants.name_max)
        {
            CWorldApp app = FindAnyObjectByType<CWorldApp>();
            app.DoubleCheck(InputDoubleCheck.text);
            NameImageExitButton.enabled = false;
            NameCheckButton.enabled = false;
        }
    }
    public void OnCreateCharacter()
    {
        for(int i = 0; i < 3;i ++)
        {
            characterButton[i].enabled = false;
        }
        CreateCanvasCreateButton.enabled = false;
        CreateCanvasExitButton.enabled = false;
        CreateImage.gameObject.SetActive(true);
    }

    public void OnCreateCharacterOk()
    {
        CWorldApp app = FindAnyObjectByType<CWorldApp>();

        app.CreateCharacter(InputDoubleCheck.text, CreateCharacterindex);
        InputDoubleCheck.text = "";
        CreateImage.gameObject.SetActive(false);
        create.gameObject.SetActive(false);
        doubleCheckNameImage.gameObject.SetActive(false);
        CreateCanvasCreateButton.enabled = true;
        CreateCanvasExitButton.enabled = true;
        createButton.enabled = true;
        deleteButton.enabled = true;
        NameImageExitButton.enabled = true;
        NameCheckButton.enabled = true;
        for (int i = 0; i < 3; i++)
        {
            characterButton[i].enabled = true;
        }
    }

    public void OnCreateCharacterNo()
    {
        CreateImage.gameObject.SetActive(false);
        InputDoubleCheck.text = "";
        NameCheckButton.enabled = true;
        NameImageExitButton.enabled = true;
    }

    public void OnDoubleImageExit()
    {
        NameCheckButton.enabled = true;
        NameImageExitButton.enabled = true;
        doubleCheckImage.gameObject.SetActive(false);
    }

    public void OnDeleteCharacter()
    {
        if(DeleteCharacterindex > 0)
        {
            createButton.enabled = false;
            deleteButton.enabled = false;
            DeleteImage.gameObject.SetActive(true);
        }
    }


    public void OnDeleteCharacterOk()
    {
        createButton.enabled = true;
        deleteButton.enabled = true;
        CWorldApp app = FindAnyObjectByType<CWorldApp>();

        if(DeleteCharacterindex == 1)
        {
            app.DeleteCharacter(info1[3].GetComponent<TextMeshProUGUI>().text);
        }
        else if(DeleteCharacterindex == 2)
        {
            app.DeleteCharacter(info2[3].GetComponent<TextMeshProUGUI>().text);
        }
        else if(DeleteCharacterindex == 3)
        {
            app.DeleteCharacter(info3[3].GetComponent<TextMeshProUGUI>().text);
        }
        DeleteImage.gameObject.SetActive(false);
        DeleteCharacterindex = 0;
    }

    public void OnDeleteCharacterNo()
    {
        createButton.enabled = true;
        deleteButton.enabled = true;
        DeleteImage.gameObject.SetActive(false);
    }

    private void SetJopText(TextMeshProUGUI _textMeshPro, int _index)
    {
        if(_index == 1)
        {
            _textMeshPro.text = "기사";
        }
        else if (_index == 2)
        {
            _textMeshPro.text = "궁수";
        }
        else if (_index == 3)
        {
            _textMeshPro.text = "법사";
        }
    }

    private void ParticlePlay(string _tag)
    {
        if (_tag == "Player" && sCharacters.c1 != 0)
        {
            DeleteCharacterindex = 1;
            characterObject[0].GetComponent<CharacterSelectionAnimator>().OnRun();
            ps[0].Play();

            if (sCharacters.c2 != 0)
            {
                characterObject[1].GetComponent<CharacterSelectionAnimator>().OnIdle();
                ps[1].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            if (sCharacters.c3 != 0)
            {
                characterObject[2].GetComponent<CharacterSelectionAnimator>().OnIdle();
                ps[2].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
        else if(_tag == "Monster" && sCharacters.c2 != 0)
        {
            DeleteCharacterindex = 2;
            characterObject[1].GetComponent<CharacterSelectionAnimator>().OnRun();
            ps[1].Play();
            if (sCharacters.c1 != 0)
            {
                characterObject[0].GetComponent<CharacterSelectionAnimator>().OnIdle();
                ps[0].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            if(sCharacters.c3 != 0)
            {
                characterObject[2].GetComponent<CharacterSelectionAnimator>().OnIdle();
                ps[2].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
        else if(_tag == "test" && sCharacters.c3 != 0)
        {
            DeleteCharacterindex = 3;
            characterObject[2].GetComponent<CharacterSelectionAnimator>().OnRun();
            ps[2].Play();

            if (sCharacters.c1 != 0)
            {
                characterObject[0].GetComponent<CharacterSelectionAnimator>().OnIdle();
                ps[0].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            if (sCharacters.c2 != 0)
            {
                characterObject[1].GetComponent<CharacterSelectionAnimator>().OnIdle();
                ps[1].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    public void UpdateCharacterList(CStruct.sCharacterList _characterList)
    {
        if (_characterList.c1 > 0)
        {
            GameObject gameObject = GameObject.Find("character1");
            if(sCharacters.c1 > 0) gameObject.transform.GetChild(sCharacters.c1 - 1).gameObject.SetActive(false);
            gameObject.transform.GetChild(_characterList.c1 - 1).gameObject.SetActive(true);
            characterObject[0] = gameObject.transform.GetChild(_characterList.c1 - 1).gameObject;
            info1[0].SetActive(true);
            info1[1].GetComponent<TextMeshProUGUI>().text = "Lv." + _characterList.c_1_Level.ToString();
            SetJopText(info1[2].GetComponent<TextMeshProUGUI>(), _characterList.c1);
            info1[3].GetComponent<TextMeshProUGUI>().text = _characterList.c1_name;
        }
        else
        {
            info1[0].SetActive(false);
            character1.GetComponent<Image>().color = new Color(255f, 255f, 255f);
            GameObject gameObject = GameObject.Find("character1");
            for (int i = 0; i < 3; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        if (_characterList.c2 > 0)
        {
            GameObject gameObject = GameObject.Find("character2");
            if (sCharacters.c2 > 0) gameObject.transform.GetChild(sCharacters.c2 - 1).gameObject.SetActive(false);
            gameObject.transform.GetChild(_characterList.c2 - 1).gameObject.SetActive(true);
            characterObject[1] = gameObject.transform.GetChild(_characterList.c2 - 1).gameObject;
            info2[0].SetActive(true);
            info2[1].GetComponent<TextMeshProUGUI>().text = "Lv." + _characterList.c_2_Level.ToString();
            SetJopText(info2[2].GetComponent<TextMeshProUGUI>(), _characterList.c2);
            info2[3].GetComponent<TextMeshProUGUI>().text = _characterList.c2_name;
        }
        else
        {
            info2[0].SetActive(false);
            character2.GetComponent<Image>().color = new Color(255f, 255f, 255f);
            GameObject gameObject = GameObject.Find("character2");
            for (int i = 0; i < 3; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        if (_characterList.c3 > 0)
        {
            GameObject gameObject = GameObject.Find("character3");
            if (sCharacters.c3 > 0) gameObject.transform.GetChild(sCharacters.c3 - 1).gameObject.SetActive(false);
            gameObject.transform.GetChild(_characterList.c3 - 1).gameObject.SetActive(true);
            characterObject[2] = gameObject.transform.GetChild(_characterList.c3 - 1).gameObject;
            info3[0].SetActive(true);
            info3[1].GetComponent<TextMeshProUGUI>().text = "Lv." + _characterList.c_3_Level.ToString();
            SetJopText(info3[2].GetComponent<TextMeshProUGUI>(), _characterList.c3);
            info3[3].GetComponent<TextMeshProUGUI>().text = _characterList.c3_name;
        }
        else
        {
            info3[0].SetActive(false);
            character3.GetComponent<Image>().color = new Color(255f, 255f, 255f);
            GameObject gameObject = GameObject.Find("character3");
            for (int i = 0; i < 3; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            characterCount--;
        }

        for(int i = 0; i < 3; i++)
        {
            ps[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        int count = 0;

        if (_characterList.c1 > 0) count++;
        if (_characterList.c2 > 0) count++;
        if (_characterList.c3 > 0) count++;

        characterCount = count;

        sCharacters = _characterList;
    }

    public void OnStartButton()
    {
        CWorldApp app = FindAnyObjectByType<CWorldApp>();
        
        switch(DeleteCharacterindex)
        {
            case 1:
                app.OnStartButton(sCharacters.c1_name, sCharacters.c1_name_Len);        
                break;
            case 2:
                app.OnStartButton(sCharacters.c2_name, sCharacters.c2_name_Len);
                break;
            case 3:
                app.OnStartButton(sCharacters.c3_name, sCharacters.c3_name_Len);
                break;
        }
    }

    public void NameCheck(int _check)
    {
        if(_check == 0)
        {
            CreateImage.gameObject.SetActive(true);
        }
        else
        {
            doubleCheckImage.gameObject.SetActive(true);
        }
    }


    public void TEST()
    {
        foreach (GameObject game in test)
        {
            game.SetActive(true);
        }
    }
}
