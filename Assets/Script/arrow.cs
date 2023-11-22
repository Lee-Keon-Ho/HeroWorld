using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public ParticleSystem psHit;
    private float speed = 20.0f;
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Monster")
        {
            CApp app = FindAnyObjectByType<CApp>();
            CClickMoveMent cClick = FindAnyObjectByType<CClickMoveMent>();
            cClick.SetTargetMonsterInfo(other.gameObject);
            GameObject ps = Instantiate(psHit.gameObject);
            ps.transform.position = other.transform.position;
            app.HitMonster(other.GetComponent<CMonster>().GetIndex());
            Destroy(gameObject);
        }
    }
}
