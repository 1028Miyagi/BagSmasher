using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Boss : MonoBehaviour
{
    public enum bossState
    {
        Wait,
        Walk,
        Attack,
        Summon,
        Rush,
        Damege
    }

    public bossState nowState;

    [SerializeField] float stopDistance;
    [SerializeField] float moveDistance;
    [SerializeField] float moveSpeed;

    [SerializeField] float bossDirection;
    [SerializeField] float attackCoolTime = 10.0f;
    [SerializeField] float nowCoolTime;

    [SerializeField] Transform player;
    [SerializeField] GameObject bossObject;

    [SerializeField] bool getBossDeathFlag;

    [SerializeField] bool attackFlag;
    [SerializeField] bool canAttackFlag;

    [SerializeField] int pattern; //攻撃パターン
    float distance;

    [SerializeField] Animator bossAnimator;

    [SerializeField] bool stopFlag; //モーション中の移動対策用
    [SerializeField] bool attackStop; //突進中の他の攻撃パターンへの移行防止

    [SerializeField] bool rushFlag;
    [SerializeField] LayerMask groundLayer;

    public bool damegeFlag;

    public GameObject rushEfect;
    public GameObject attackEfect;

    public AudioSource BossAudioSource;
    public AudioClip[] BossSound;
    public float[] BossSoundVolume;

    public int count; //倒された時の効果音重複対策用

    void Start()
    {
        canAttackFlag = true;
        rushFlag = false;
        damegeFlag = false;
        count = 0;
    }

    void Update()
    {
        Vector3 playerPos = player.position;
        Vector3 bossPos = this.transform.position;

        bossObject = transform.GetChild(0).gameObject;

        distance = Vector3.Distance(transform.position, player.transform.position); //攻撃範囲などの設定

        if (!rushFlag)
        {
            bossDirection = bossPos.x - playerPos.x; //ボスとプレイヤーの位置の差
        }

        if (damegeFlag)
        {
            BossDamege();
        }

        Turn_B();
        Attack_B();

        #region　突進時の移動

        if (rushFlag)
        {
            BossAudioSource.volume = BossSoundVolume[1];
            BossAudioSource.PlayOneShot(BossSound[1]);

            transform.position += bossObject.transform.forward * moveSpeed * Time.deltaTime;
            rushEfect.SetActive(true);
        }
        else
        {
            rushEfect.SetActive(false);
        }

        #endregion

        CoolTime_B();
        Walk_B();
        BossAnimation();

        getBossDeathFlag = BossHP.SetBossDeathFlag();

        if (getBossDeathFlag)
        {
            count++; 

            if (count == 1)
            {
                BossAudioSource.volume = BossSoundVolume[2];
                BossAudioSource.PlayOneShot(BossSound[2]);

                bossAnimator.Play("Death");
                GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    #region　待機、移動のモーション

    void BossAnimation()
    {
        if (nowState == bossState.Walk)
        {
            bossAnimator.SetBool("isWalk", true);
        }
        else
        {
            bossAnimator.SetBool("isWalk", false);
        }
    }

    #endregion

    #region　方向転換

    void Turn_B()
    {
        if (attackStop || stopFlag || rushFlag)
        {
            return;
        }

        if (bossDirection < 0)
        {
            float y = 90;
            bossObject.transform.rotation = Quaternion.Euler(0.0f, y, 0.0f);
        }
        else if (bossDirection > 0)
        {
            float y = -90;
            bossObject.transform.rotation = Quaternion.Euler(0.0f, y, 0.0f);
        }
    }

    #endregion

    #region　移動

    void Walk_B()
    {
        if (stopFlag || rushFlag) return;

        if (distance < moveDistance && distance > stopDistance)
        {
            nowState = bossState.Walk;

            if (transform.position.x >= 517 && transform.position.x <= 551)
            {
                if (bossDirection < 0)
                {
                    transform.position = transform.position - transform.forward * moveSpeed * Time.deltaTime;
                }
                else if (bossDirection > 0)
                {
                    transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
                }
            }
        }
        else if (!attackFlag || !canAttackFlag) //待機
        {
            nowState = bossState.Wait;
        }
    }

    #endregion

    #region　攻撃関連

    void Attack_B()
    {
        if (attackStop)
        {
            return;
        }

        if ((distance <= stopDistance) && (nowCoolTime <= 0.0f)) //攻撃の範囲内に入る
        {
            attackFlag = true;
            pattern = Random.Range(2, 7);
        }
        else
        {
            attackFlag = false;
        }

        if (attackFlag)
        {
            if (canAttackFlag && ((pattern == 2) || (pattern == 3))) //通常攻撃
            {
                nowState = bossState.Attack;
                bossAnimator.SetBool("isAttack", true);
                
                canAttackFlag = false;
                nowCoolTime = attackCoolTime;
                stopFlag = true;
            }
            else if (canAttackFlag && ((pattern == 4) || (pattern == 5))) //召喚
            {
                nowState = bossState.Summon;
                bossAnimator.SetBool("isSummon", true);
                
                canAttackFlag = false;
                nowCoolTime = attackCoolTime;
                stopFlag = true;
            }
            else if (canAttackFlag && (pattern == 6)) //突進
            {
                nowState = bossState.Rush;
                rushFlag = true;
                bossAnimator.SetTrigger("isRush");

                canAttackFlag = false;
                nowCoolTime = attackCoolTime;
                attackStop = true; //他の攻撃パターンが発動しないようにする
            }

        }
    }

    #endregion

    #region　モーション中の移動対策

    public void Movalbe()
    {
        bossAnimator.SetBool("isAttack", false);
        bossAnimator.SetBool("isSummon", false);
        bossAnimator.SetBool("isHit", false);

        canAttackFlag = true;
        stopFlag = false;
    }

    #endregion

    #region　ダメージ

    void BossDamege()
    {
        nowState = bossState.Wait;
        bossAnimator.SetBool("isAttack", false);
        bossAnimator.SetBool("isSummon", false);
        bossAnimator.Play("Damage");
        damegeFlag = false;
        attackFlag = false;
        rushFlag = false;
        stopFlag = true;
        attackStop = true;
        canAttackFlag = false;
    }

    public void DamageEnd()
    {
        attackStop = false;
        canAttackFlag = true;
        stopFlag = false;
        nowCoolTime = 1.0f;
    }

    #endregion

    //通常攻撃のエフェクト
    public void AttackEfect()
    {
        BossAudioSource.volume = BossSoundVolume[0];
        BossAudioSource.PlayOneShot(BossSound[0]);

        Instantiate(attackEfect, new Vector3(bossObject.transform.position.x, bossObject.transform.position.y, bossObject.transform.position.z) + (bossObject.transform.forward * 5), Quaternion.identity);
    }

    #region　突進関係

    public void PoseStart()
    {
        stopFlag = true;
    }

    public void PoseEnd()
    {
        stopFlag = false;

        moveSpeed = 5.0f;
    }

    public void RushStop()
    {
        rushFlag = false;
        stopFlag = true;

        bossAnimator.SetBool("isHit", true);
    }

    public void RushEnd()
    {
        canAttackFlag = true;
        stopFlag = false;
        attackStop = false;

        bossAnimator.SetBool("isHit", false);
        moveSpeed = 3.0f;
    }

    #endregion

    //クールタイム
    void CoolTime_B()
    {
        nowCoolTime -= Time.deltaTime;
        if (nowCoolTime <= 0.0f)
        {
            nowCoolTime = 0.0f;
        }
    }
}
