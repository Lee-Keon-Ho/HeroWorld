using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTownWarp : MonoBehaviour
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
        if (other.tag == "Player")
        {
            CDataManager manager = CDataManager.Instance;
            if (manager.GetWarp())
            {
                manager.SetWarp(false);
                CApp app = FindAnyObjectByType<CApp>();
                app.Warp(0);
                other.GetComponent<CClickMoveMent>().Idle();
                other.GetComponent<CClickMoveMent>().SetMove(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            CDataManager.Instance.SetWarp(true);
        }
    }
}
