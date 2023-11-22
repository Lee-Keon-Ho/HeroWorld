using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWarp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            CApp app = FindAnyObjectByType<CApp>();
            app.Warp(1);
            other.GetComponent<CClickMoveMent>().SetMove(false);
            other.GetComponent<CClickMoveMent>().Idle();
        }
    }
}
