using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    [SerializeField] Boss boss;
    [SerializeField] Slider bossSlider;
    [SerializeField] float maxHp = 200;
    [SerializeField] int damage = 16;

    public static bool bossDeathFlag;
    public float bossCurrentHp;

    //É{ÉXÇÃHP

    void Start()
    {
        bossSlider.value = 1;
        bossCurrentHp = maxHp;
        bossDeathFlag = false;
    }

    void Update()
    {
        bossSlider.value = bossCurrentHp / maxHp;

        if (bossCurrentHp <= 0)
        {
            bossDeathFlag = true;
        }
    }

    public static bool SetBossDeathFlag()
    {
        return bossDeathFlag;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Attacktag"))
        {
            bossCurrentHp -= damage;
            boss.damegeFlag = true;
        }
    }
}
