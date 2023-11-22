using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGoblin : CMonster
{
    private Animator animator;
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if (bDie)
        {
            fDieTime += Time.deltaTime;
            if (fDieTime >= 3f)
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

    public new void Hit(int _curHp, int _damage)
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

    public new void Attack(Vector3 _position)
    {
        var dir = new Vector3(_position.x, this.transform.position.y, _position.z) - transform.position;
        animator.transform.forward = dir;
        animator.SetBool("HIT", false);
        animator.SetBool("IDLE", false);
        animator.SetBool("RUN", false);
        animator.SetBool("ATTACK", true);
        isMove = false;
    }
}
