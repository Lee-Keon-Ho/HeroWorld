using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class CClickMoveMent : MonoBehaviour
{
    private Camera camera;
    private Animator animator;
    private NavMeshAgent agent;
    private ParticleSystem psSword;
    public ParticleSystem psHit;
    public ParticleSystem psLevelUp;
    public GameObject ArrowObject;
    public GameObject FireObject;
    private CMonster targetMonster;
    public TarGetInfo targetInfo;
    public TextMeshProUGUI curHpUI;
    public TextMeshProUGUI maxHpUI;
    public TextMeshProUGUI curMpUI;
    public TextMeshProUGUI maxMpUI;
    public TextMeshProUGUI curExpUI;
    public TextMeshProUGUI levelUp;
    public TextMeshProUGUI playerLevel;
    public Slider HpBarSlider;
    public Slider MpBarSlider;
    public Slider expSlider;
    public GameObject[] Character;
    public Slider slider;
    private GameObject basicAttack;

    public Image exit;
    bool bExit;

    int layerMask;
    int layerMaskMonster;

    NavMeshPath path;
    CApp app;

    CStruct.sCharacterInfo info;
    bool isMove;
    bool bAttack;
    int pathCount;
    ushort number;
    float timer;
    int m_state;
    int m_character;
    private float curHp;
    private float maxHp;
    private float curMp;
    private float maxMp;
    private int level;
    public Vector3 boxSize = new Vector3(2, 1, 2);
    private Collider[] colliders;
    public float radius;

    private bool toggleCameraRotation;
    private float smoothness = 100f;
    private float h;
    private float v;
    private Vector3 arrowVector;
    private Vector3 position;

    private GameObject objectManager;
    private bool warp;
    private void Awake()
    {
        camera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        path = new NavMeshPath();
        app = FindAnyObjectByType<CApp>();

        bExit = true;
        m_state = 0;
        pathCount = 0;
        timer = 0f;
        bAttack = true;
        toggleCameraRotation = false;
        warp = true;
        layerMask = 1 << LayerMask.NameToLayer("Default");
        layerMaskMonster = 1 << LayerMask.NameToLayer("Monster");
        psLevelUp.Stop();

        objectManager = GameObject.Find("ObjectManager");
    }

    private void Start()
    {
        Init();
    }

    void Update()
    {
        HpBarSlider.value = curHp / maxHp;
        MpBarSlider.value = curMp / maxMp;

        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (bExit)
            {
                exit.gameObject.SetActive(true);
                bExit = false;
            }
            else
            {
                exit.gameObject.SetActive(false);
                bExit = true;
            }
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true; // 둘러보기 활성화
            h = Input.GetAxis("Mouse X");
            v = Input.GetAxis("Mouse Y");
        }
        else if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
            toggleCameraRotation = false; // 둘러보기 비활성화
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK"))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    bAttack = true;
                    if(m_character == 0) psSword.Stop();
                    animator.SetBool("ATTACK", false);
                    if (Input.GetMouseButtonDown(1))
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 100f, layerMask))
                        {
                            SetDestination(hit.point);
                        }
                    }
                    else
                    {
                        animator.SetBool("IDLE", true);
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(1))
                {
                    RaycastHit hit;
                    bAttack = true;
                    if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 100f, layerMask))
                    {
                        
                        SetDestination(hit.point);
                    }
                }
                
                if(m_character == 0)
                {
                    knightUpdate();
                }
                else if(m_character == 1)
                {
                    ArcherUpdate();
                }
                else if(m_character == 2)
                {
                    WizardUpdate();
                }
            }

            if (slider.gameObject.activeSelf)
            {
                if (targetMonster.GetCurHp() == 0) slider.gameObject.SetActive(false);
                targetInfo.SetUi(targetMonster.GetMaxHp(), targetMonster.GetCurHp(), targetMonster.GetMonsterType(), targetMonster.GetLevel());
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("RUN")) // 보내도 안 받는 처리를 한 기억이 있다.
            {
                if (timer >= 1.0f)
                {
                    app.NowPosition(this.transform.position, number);
                    timer = 0f;
                }
            }
        }
    }

    public void SetDestination(Vector3 _dest)
    {
        if(Vector3.Distance(this.transform.position, _dest) > 0.7f)
        {
            NavMeshPath tempPath = new NavMeshPath(); ;
            agent.CalculatePath(_dest, tempPath);
            if(tempPath.corners.Length != 0)
            {
                path = tempPath;
                pathCount = 1;

                isMove = true;
                m_state = 1;
                animator.SetBool("RUN", true);
                animator.SetBool("ATTACK", false);
                animator.SetBool("IDLE", false);

                app.MoveUser(this.transform.position, path.corners[pathCount], number, m_state);
                agent.SetDestination(path.corners[pathCount]);
            }
        }
    }

    private void LateUpdate()
    {
        if(toggleCameraRotation)
        {
            camera.transform.position = this.transform.position + new Vector3(0f, 2f, 0f);
            Vector3 dir = new Vector3(-v, h, 0f);
            camera.transform.eulerAngles += dir * smoothness * Time.deltaTime;
        }
        else
        {
            camera.transform.position = this.transform.position + new Vector3(0f, 8f, -8f);
            camera.transform.eulerAngles = new Vector3(40f, 0f, 0f);
        }
        if (isMove && !toggleCameraRotation)
        {
            var dir = new Vector3(path.corners[pathCount].x, this.transform.position.y, path.corners[pathCount].z) - transform.position;
            animator.transform.forward = dir;
            if (Vector3.Distance(this.transform.position, path.corners[pathCount]) <= 0.1)
            {
                pathCount++;
                if (path.corners.Length == pathCount)
                {
                    pathCount = 0;
                    m_state = 0;
                    animator.SetBool("RUN", false);
                    animator.SetBool("IDLE", true);
                    isMove = false;
                    app.Arrive(this.transform.position, number, this.transform.GetChild(3).transform.eulerAngles.y, m_state);
                    return;
                }
                app.MoveUser(this.transform.position, path.corners[pathCount], number, m_state);
                agent.SetDestination(path.corners[pathCount]);
            }
        }
    }

    private void knightUpdate()
    {
        if (Input.GetMouseButtonDown(0) && bAttack)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            bAttack = false;

            colliders = Physics.OverlapSphere(basicAttack.transform.position, radius, layerMaskMonster);

            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Attack(hit.point);
            }

            int len = colliders.Length;
            if (len > 0)
            {
                List<int> indexList = new List<int>();
                for (var i = 0; i < len; i++)
                {
                    indexList.Add(colliders[i].gameObject.GetComponent<CMonster>().GetIndex()); // override
                    GameObject ps = Instantiate(psHit.gameObject);
                    ps.transform.position = colliders[i].transform.position;
                }
                if (!slider.gameObject.activeSelf)
                {
                    slider.gameObject.SetActive(true);
                }
                targetMonster = colliders[0].gameObject.GetComponent<CMonster>();
                HitMonster(indexList);
            }
        }
    }

    private void ArcherUpdate()
    {
        if (Input.GetMouseButtonDown(0) && bAttack)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            bAttack = false;

            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                ArcherAttack(hit.point);
            }

            arrowVector = new Vector3(hit.point.x, this.transform.position.y, hit.point.z) - transform.position;
            Invoke("CreateArrow", 0.7f);
        }
    }

    private void WizardUpdate()
    {
        if (Input.GetMouseButtonDown(0) && bAttack)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            bAttack = false;

            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                ArcherAttack(hit.point);
            }

            arrowVector = new Vector3(hit.point.x, this.transform.position.y, hit.point.z) - transform.position;
            Invoke("CreateFireBall", 0.7f);
        }
    }

    public void SetTargetMonsterInfo(GameObject _monster)
    {
        if (!slider.gameObject.activeSelf)
        {
            slider.gameObject.SetActive(true);
        }
        targetMonster = _monster.GetComponent<CMonster>();
    }

    private void CreateFireBall()
    {
        GameObject fireBall = Instantiate(FireObject);
        fireBall.transform.position = new Vector3(transform.position.x, 1.35f, transform.position.z);
        fireBall.transform.forward = arrowVector;
    }

    private void CreateArrow()
    {
        GameObject arrow = Instantiate(ArrowObject);
        arrow.transform.position = new Vector3(transform.position.x, 1.35f, transform.position.z);
        arrow.transform.right = -arrowVector;
    }

    private void ArcherAttack(Vector3 _position)
    {
        var dir = new Vector3(_position.x, this.transform.position.y, _position.z) - transform.position;
        animator.transform.forward = dir;

        if (isMove)
        {
            app.ArcherMoveAttack(this.transform.position, this.transform.GetChild(3).transform.eulerAngles.y);
        }
        else
        {
            app.ArcherIdleAttack(this.transform.GetChild(3).transform.eulerAngles.y);
        }

        isMove = false;
        agent.SetDestination(this.transform.position);
        animator.SetBool("IDLE", false);
        animator.SetBool("RUN", false);
        animator.SetBool("ATTACK", true);
        if (m_character == 0) psSword.Play();
    }

    private void Attack(Vector3 _position)
    {
        var dir = new Vector3(_position.x, this.transform.position.y, _position.z) - transform.position;
        animator.transform.forward = dir;
        
        if (isMove)
        {
            app.PlayerMoveAttack(this.transform.position, this.transform.GetChild(3).transform.eulerAngles.y);
        }
        else
        {
            app.PlayerIdleAttack(this.transform.GetChild(3).transform.eulerAngles.y);
        }

        isMove = false;
        agent.SetDestination(this.transform.position);
        animator.SetBool("IDLE", false);
        animator.SetBool("RUN", false);
        animator.SetBool("ATTACK", true);
        if(m_character == 0) psSword.Play();
    }
    private void HitMonster(List<int> _indexList)
    {
        app.HitMonster(_indexList);
    }

    public void Init() 
    {
        CStruct.sCharacterInfo info = CDataManager.Instance.GetCharacterInfo();

        number = CDataManager.Instance.GetIndex();
        playerLevel.text = info.level.ToString();
        curHp = info.curHp;
        maxHp = info.maxHp;
        curMp = info.curMp;
        maxMp = info.maxMp;
        curHpUI.text = curHp.ToString();
        maxHpUI.text = maxHp.ToString();
        curMpUI.text = curMp.ToString();
        maxMpUI.text = maxMp.ToString();

        HpBarSlider.value = curHp / maxHp;
        MpBarSlider.value = curMp / maxMp;

        this.transform.position = info.position;
        position = info.position;
        agent.gameObject.transform.position = info.position;
        agent.CalculatePath(info.position, path);
        agent.SetDestination(path.corners[pathCount]);
        agent.enabled = false;

        expSlider.value = 0f;
        expSlider.value = info.curExp * 0.01f;
        curExpUI.text = info.curExp.ToString("F2");

        m_character = info.type;
        GameObject fish = Instantiate(Character[info.type]) as GameObject;
        fish.transform.SetParent(this.transform, false);
        animator = GetComponentInChildren<Animator>();
        if (m_character == 0)
        {
            psSword = transform.GetChild(3).GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
            psSword.Stop();
            basicAttack = transform.GetChild(3).GetChild(3).gameObject;
        }

        transform.Find("Name").GetComponent<TextMeshPro>().text = CDataManager.Instance.GetName();

        agent.enabled = true;

        objectManager.SetActive(false);
        objectManager.SetActive(true);
    }

    public void SetLevel(int _level, float _curExp, float _maxExp)
    {
        curHp = 100.0f;
        HpBarSlider.value = curHp / maxHp;
        MpBarSlider.value = curMp / maxMp;

        playerLevel.text = _level.ToString();
        expSlider.value = _curExp * 0.01f;
        curExpUI.text = _curExp.ToString("F2");
        levelUp.gameObject.SetActive(true);
        Invoke("LeveLUp", 3.0f);
        Instantiate(psLevelUp, transform);
    }

    void LeveLUp()
    {
        levelUp.gameObject.SetActive(false);
    }

    public void SetExp(float _exp)
    {
        expSlider.value = _exp * 0.01f;
        curExpUI.text = _exp.ToString("F2");
    }

    public void SetCurHp(int _curHp)
    {
        curHp = _curHp;
        curHpUI.text = curHp.ToString();
        HpBarSlider.value = curHp / maxHp;
    }

    public void SetMove(bool _b)
    {
        isMove = _b;
    }

    public void Idle()
    {
        animator.SetBool("ATTACK", false);
        animator.SetBool("RUN", false);
        animator.SetBool("IDLE", true);
    }
    public void OnExitOk()
    {
        CApp app = FindAnyObjectByType<CApp>();
        app.LogOut();
    }

    public void OnExitNo()
    {
        exit.gameObject.SetActive(false);
        bExit = true;
    }
}