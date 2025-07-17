using UnityEngine;
using TMPro;

public class CMonster : MonoBehaviour
{
    private Animator animator;
    protected int level;
    protected int index;
    protected bool isMove;
    protected bool bDie;
    protected float fDieTime;
    protected int nextIndex;
    protected float distance;
    protected float speed;
    protected Vector3 destinationPosition;
    public GameObject damage;
    protected float curHp;
    protected float maxHp;
    protected int type;

    void Start()
    {
        bDie = false;
        maxHp = 10f;
        fDieTime = 0f;
        curHp = maxHp;

        speed = 1.2f;
        isMove = false;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (bDie)
        {
            fDieTime += Time.deltaTime;
            if(fDieTime >= 3f)
            {
                bDie = false;
                fDieTime = 0f;
                this.gameObject.SetActive(false);
            }
            return;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("HIT") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            animator.SetBool("HIT", false);
            animator.SetBool("IDLE", true);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            animator.SetBool("ATTACK", false);
            animator.SetBool("IDLE", true);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            animator.SetBool("ATTACK", false);
            animator.SetBool("IDLE", false);
            animator.SetBool("Die", false);
        }
    }
    
    private void LateUpdate()
    {
        if (isMove) MoveMent();
    }

    void MoveMent()
    {
        distance = Vector3.Distance(this.transform.position, destinationPosition);
        if (distance <= 0.1f)
        {
            animator.SetBool("IDLE", true);
            animator.SetBool("RUN", false);
            isMove = false;
        }
        else
        {
            var dir = new Vector3(destinationPosition.x, this.transform.position.y, destinationPosition.z) - transform.position;
            animator.transform.forward = dir;
            this.transform.position = Vector3.MoveTowards(this.transform.position, destinationPosition, speed * Time.deltaTime);
        }
    }

    public void SetDestination(Vector3 _destination)
    {
        animator = GetComponent<Animator>();
        if (Vector3.Distance(this.transform.position, _destination) >= 0.1f)
        {
            animator.SetBool("ATTACK", false);
            animator.SetBool("IDLE", false);
            animator.SetBool("RUN", true);
            destinationPosition = _destination;
            isMove = true;
        }
    }

    public void Hit(int _curHp, int _damage)
    {
        curHp = _curHp;
        damage.GetComponent<DamageText>().SetDamage(_damage);
        GameObject text = Instantiate(damage);
        text.transform.position = transform.position + Vector3.up;
        animator.SetBool("ATTACK", false);
        animator.SetBool("IDLE", false);
        animator.SetBool("RUN", false);
        animator.SetBool("HIT", true);
    }

    public void Attack(Vector3 _position)
    {
        var dir = new Vector3(_position.x, this.transform.position.y, _position.z) - transform.position;
        animator.transform.forward = dir;
        animator.SetBool("HIT", false);
        animator.SetBool("IDLE", false);
        animator.SetBool("RUN", false);
        animator.SetBool("ATTACK", true);
        isMove = false;
    }

    public void Die(int _damage)
    {
        bDie = true;
        curHp = 0;
        damage.GetComponent<DamageText>().SetDamage(_damage);
        SetDestination(transform.position);
        GameObject text = Instantiate(damage);
        text.transform.position = transform.position + Vector3.up;
        animator.SetBool("Die", true);
    }

    public void SetInfo(int _index, int _type, int _level)
    {
        if(_type != 1)
        {
            maxHp = 15f;
        }
        if(_type == 5)
        {
            maxHp = 20;
        }
        index = _index;
        type = _type;
        level = _level;
    }
    public int GetIndex() { return index; }
    public float GetMaxHp() { return maxHp; }
    public float GetCurHp() { return curHp; }
    public int  GetMonsterType() { return type; }
    public int GetLevel() { return level; }
}