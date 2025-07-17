using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CUserMove : MonoBehaviour
{
    private Animator animator;
    private Vector3 endPosition;

    public GameObject[] Character;
    public GameObject ArrowObject;
    public GameObject FireObject;
    public ParticleSystem psSword;
    public GameObject userChat;
    public TextMeshProUGUI userChatText;

    private Vector3 arrowVector;
    private bool isMove;
    private float speed;
    public int userNumber;
    float distance;
    int nCharacter;
    private void Awake()
    {
        isMove = false;
        speed = 2f;
        userNumber = 0;
        animator = GetComponentInChildren<Animator>();
        userChat.SetActive(false);
    }

    private void Start()
    {
        if (psSword != null) psSword.Stop();
    }
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
        {
            if (nCharacter == 0) psSword.Stop();
            animator.SetBool("ATTACK", false);
            animator.SetBool("IDLE", true);
        }
    }

    private void LateUpdate()
    {
        if (isMove) MoveMent();
    }

    private void MoveMent()
    {        
        distance = Vector3.Distance(this.transform.position, endPosition);
        if (distance <= 0.1f)
        {
            animator.SetBool("IDLE", true);
            animator.SetBool("RUN", false);
            isMove = false;
            return;
        }
        else
        {
            var dir = new Vector3(endPosition.x, this.transform.position.y, endPosition.z) - transform.position;
            animator.transform.forward = dir;
            this.transform.position = Vector3.MoveTowards(this.transform.position, endPosition, speed * Time.deltaTime);
        }
    }

    public void SetDestination(Vector3 _startPosition, Vector3 _endPosition)
    {
        if (Vector3.Distance(_startPosition, _endPosition) >= 0.1f)
        {
            this.transform.position = _startPosition;
            endPosition = _endPosition;

            isMove = true;
            animator.SetBool("ATTACK", false);
            animator.SetBool("IDLE", false);
            animator.SetBool("RUN", true);
        }
    }

    public void Arrive(Vector3 _endPosition, float _y)
    {
        this.transform.position = _endPosition;
        endPosition = _endPosition;
        this.transform.GetChild(2).localEulerAngles = new Vector3(0, _y, 0);
        isMove = false;
        animator.SetBool("IDLE", true);
        animator.SetBool("RUN", false);
    }

    public void IdleAttack(float _y)
    {
        isMove = false;
        this.transform.GetChild(2).localEulerAngles = new Vector3(0, _y, 0);

        animator.SetBool("IDLE", false);
        animator.SetBool("RUN", false);
        animator.SetBool("ATTACK", true);

        if (nCharacter == 0)
        {
            psSword = transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
            psSword.Play();
        }
        if (nCharacter == 1) Invoke("CreateArrow", 0.7f);
        if (nCharacter == 2) Invoke("CreateFireBall", 0.7f);
    }

    public void MoveAttack(Vector3 _position, float _y)
    {
        isMove = false;
        this.transform.position = _position;
        endPosition = _position;
        this.transform.GetChild(2).localEulerAngles = new Vector3(0, _y, 0);

        animator.SetBool("IDLE", false);
        animator.SetBool("RUN", false);
        animator.SetBool("ATTACK", true);

        if (nCharacter == 0)
        {
            psSword = transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
            psSword.Play();
        }
        if (nCharacter == 1) Invoke("CreateArrow", 0.7f);
        if (nCharacter == 2) Invoke("CreateFireBall", 0.7f);
    }

    private void CreateArrow()
    {
        GameObject arrow = Instantiate(ArrowObject);
        arrow.transform.position = new Vector3(transform.position.x, 1.35f, transform.position.z);
        //arrow.transform.localEulerAngles = transform.GetChild(1).localEulerAngles;
        arrow.transform.right = -transform.GetChild(2).transform.forward;
    }

    private void CreateFireBall()
    {
        GameObject fireBall = Instantiate(FireObject);
        fireBall.transform.position = new Vector3(transform.position.x, 1.35f, transform.position.z);
        fireBall.transform.forward = transform.GetChild(2).transform.forward;
    }

    public void SetUserNumber(int _userNumber)
    {
        userNumber = _userNumber;
    }

    public void SetGoalPosition(Vector3 _position) { endPosition = _position; }

    public void SetCharacter(int _num)
    {
        nCharacter = _num;
        GameObject fish = Instantiate(Character[_num]) as GameObject;
        fish.transform.SetParent(this.transform, false);
        animator = GetComponentInChildren<Animator>();
        if(_num == 0)
        {
            psSword = transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
            psSword.Stop();
        }        
    }

    public void psStop()
    {
        if (nCharacter == 0)
        {
            psSword = transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
            psSword.Stop();
        }
    }

    public void OnChatting(string _str)
    {
        userChat.SetActive(true);
        userChatText.text = _str;
        Invoke("UserChatDisabled", 3.0f);
    }

    private void UserChatDisabled()
    {
        userChat.SetActive(false);
    }
}