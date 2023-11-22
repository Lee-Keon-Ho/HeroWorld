using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionAnimator : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public void OnRun()
    {
        animator.SetBool("IDLE", false);
        animator.SetBool("RUN", true);
    }
    public void OnIdle()
    {
        animator.SetBool("RUN", false);
        animator.SetBool("IDLE", true);
    }
}
