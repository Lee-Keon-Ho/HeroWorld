using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class testClickMove : MonoBehaviour
{
    private Camera camera;
    private Animator animator;
    private NavMeshAgent agent;

    NavMeshPath path;
    CSector sector;

    bool isMove;
    int pathCount;

    private void Awake()
    {
        camera = Camera.main;
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        path = new NavMeshPath();
        sector = new CSector();

        sector.Init();

        pathCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                SetDestination(hit.point);
            }
        }
    }

    private void SetDestination(Vector3 _dest)
    {
        agent.CalculatePath(_dest, path);
        if (Vector3.Distance(this.transform.position, _dest) > 0.7f)
        {
            agent.CalculatePath(_dest, path);
            if (path.corners.Length != 0)
            {
                pathCount = 1;

                isMove = true;
                animator.SetBool("IDLE", false);
                animator.SetBool("RUN", true);
                agent.SetDestination(path.corners[pathCount]);
            }
        }
    }

    private void LateUpdate()
    {
        if (isMove)
        {
            var dir = new Vector3(path.corners[pathCount].x, this.transform.position.y, path.corners[pathCount].z) - transform.position;
            animator.transform.forward = dir;
            if (Vector3.Distance(this.transform.position, path.corners[pathCount]) <= 0.1)
            {
                pathCount++;
                if (path.corners.Length == pathCount)
                {
                    pathCount = 0;
                    animator.SetBool("RUN", false);
                    animator.SetBool("IDLE", true);
                    isMove = false;
                    return;
                }
                agent.SetDestination(path.corners[pathCount]);
            }
        }
    }
}
