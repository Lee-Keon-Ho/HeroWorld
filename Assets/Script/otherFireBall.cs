using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class otherFireBall : MonoBehaviour
{
    public ParticleSystem psHit;
    private float speed = 10.0f;
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            GameObject ps = Instantiate(psHit.gameObject);
            ps.transform.position = other.transform.position;
            Destroy(gameObject);
        }
    }
}
