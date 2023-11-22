using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private Camera camera;
    public float moveSpeed; // 텍스트 이동속도
    public float alphaSpeed; // 투명도 변환속도
    public float destroyTime;
    TextMeshPro text;
    Color alpha;

    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        text = GetComponent<TextMeshPro>();
        alpha = text.color;
        Invoke("DestroyObject", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = camera.transform.rotation;
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        moveSpeed += 0.1f;
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
        text.fontSize -= 0.1f;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void SetDamage(int _damage)
    {
        text = GetComponent<TextMeshPro>();
        text.text = _damage.ToString();
    }
}
