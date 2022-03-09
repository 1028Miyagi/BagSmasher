using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnim : MonoBehaviour
{
    [SerializeField] Boss boss;
    [SerializeField] BossAttack attack;
    [SerializeField] BossSummon summon;
    [SerializeField] BossSummon summon2;
    [SerializeField] BossRush rush;

    public GameObject hitEfect;
    public GameObject bossObj;

    public AudioSource SummonAudioSource;
    public AudioClip SummonSound;
    public float SummonSoundVolume;

    //���[�V�����֘A

    void Movalbe() //�ړ��΍�
    {
        boss.Movalbe();
    }

    #region�@�ʏ�U��

    void canAttack()
    {
        attack.ColOn();
    }

    void AttackEffect()
    {
        boss.AttackEfect();
    }

    void enableAttack()
    {
        attack.ColOff();
    }

    #endregion

    #region�@����

    void CanSummon()
    {
        SummonAudioSource.volume = SummonSoundVolume;
        SummonAudioSource.PlayOneShot(SummonSound);

        summon.spawn();
        summon2.spawn();
    }

    #endregion

    #region�@�ːi

    void PoseStart()
    {
        boss.PoseStart();
    }

    void PoseEnd()
    {
        boss.PoseEnd();
    }

    void canRush()
    {
        rush.R_ColOn();
    }

    void enableRush()
    {
        rush.R_ColOff();

        Instantiate(hitEfect, new Vector3(bossObj.transform.position.x, bossObj.transform.position.y + 3, bossObj.transform.position.z) + (bossObj.transform.forward * 5), Quaternion.identity);
    }

    void RushEnd()
    {
        boss.RushEnd();
    }

    #endregion
    
    void DamageEnd() //�_���[�W
    {
        boss.DamageEnd();
    }
}
