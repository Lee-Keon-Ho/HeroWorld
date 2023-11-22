using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public ParticleSystem psHit;
    private float speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
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
