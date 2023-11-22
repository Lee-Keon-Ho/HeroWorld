using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSlimeLeaf : CMonster
{
    private Animator animator;

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
}