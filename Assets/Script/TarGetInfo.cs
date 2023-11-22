using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TarGetInfo : MonoBehaviour
{
    public Slider HpBarSlider;
    public TextMeshProUGUI cur;
    public TextMeshProUGUI max;
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI m_level;

    float curHealth;
    float maxHealth;

    public void SetUi(float _maxHp, float _curHp, int _type, int _level)
    {
        switch(_type)
        {
            case 0:
                m_name.text = "슬라임";
                break;
            case 1:
                m_name.text = "나뭇잎슬라임";
                break;
            case 2:
                m_name.text = "슬라임";
                break;
            case 3:
                m_name.text = "슬라임";
                break;
            case 4:
                m_name.text = "슬라임";
                break;
            case 5:
                m_name.text = "도깨비";
                break;

        }
        maxHealth = _maxHp;
        curHealth = _curHp;

        max.text = maxHealth.ToString();
        cur.text = curHealth.ToString();
        m_level.text = _level.ToString();

        if (maxHealth == 0 || curHealth <= 0) return;

        if (HpBarSlider != null)
        {
            HpBarSlider.value = curHealth / maxHealth;
        }
    }
}
