using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class otherArrow : MonoBehaviour
{
    public ParticleSystem psHit;
    private float speed = 20.0f;
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    void Update()
    {
        transform.Translate(-Vector3.right * speed * Time.deltaTime);
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
