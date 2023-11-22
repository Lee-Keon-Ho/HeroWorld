using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosEffect : MonoBehaviour
{
    public GameObject effect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100f))
            {
                Vector3 hitPos = hit.point;

                GameObject game = Instantiate(effect);
                game.transform.position = hitPos;
            }
        }
    }
}
