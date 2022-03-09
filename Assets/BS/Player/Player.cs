using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //名草

    GameObject stoptarget;
    StopEnemyScript SP;
    PataPataScript PP;
    ChaisePatapata CP;
    HunmerScript HPS;
    NewwalkScript NS;

    //ここまで

    #region　 //清水

    //移動
    [Header("走る速度")] public float[] speed;//速度
    [Header("ジャンプ力")] public float jumpSpeed;//ジャンプ速度
    float horizontalkey, verticalKey;
    [Header("向いている方向")] public float xSpeed = 1.0f;
    //弾
    [Header("弾")] public GameObject Bullet; // 弾
    [Header("弾の発射速度")] public float bulletpower;
    private Vector3 bulletForce;

    //判定
    private Rigidbody rb = null;
    private bool isGround = false;

    //ジャンプ
    [Header("ジャンプ出来る")] public bool canJump = false;
    [Header("二段ジャンプ出来る")] public bool canDoubleJump = true;

    //アニメーション
    [Header("プレイヤーanimator")] public Animator animator;//S追加

    //テスト用
    public float Multiplier = 1f;
    //public bool ON;

    public float direction = 1.0f;

    [SerializeField] Transform attackPoint;

    [SerializeField] Image playerSlider;

    public static bool playerDeathFlag;

    [Header("エフェクト")] public GameObject[] particleObject;
    [Header("攻撃エフェクト")] public GameObject[] particleAtackEfect;

    [SerializeField] Timer timer;


    [Header("音源")] public AudioSource audioSource;
    public AudioClip[] sound;
    public float[] soundVolume;

    public Vector3 Spawn;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody>();

        //playerSlider.value = 1;
        playerDeathFlag = false;
    }

    //     animator.GetCurrentAnimatorStateInfo(0).IsName("")
    // Update is called once per frame
    void Update()
    {
        //接地判定を取得　GroundCheckから
        isGround = IsGround();

        MoveKey();
        F_Shot();
        Put_in_Bag();
        BagsChange();
        Attack();
        Jump();
        anime();
        Direction();
        Move();
        Invincible();

        Action();


        if (isGround)
        {
            Spawn = this.transform.position;
        }

        if (this.transform.position.y < -5f)
        {
            //this.transform.position = new Vector3(transform.position.x, 20, transform.position.z);
            this.transform.position = Spawn;
            rb.velocity = Vector3.zero;

            PlayerHP -= 5;
        }

        playerSlider.fillAmount = PlayerHP / playerMaxHP;

        if (PlayerHP <= 0)
        {
            playerDeathFlag = true;
            timer.stoper = 0;
        }
        //}
    }

    private void FixedUpdate()
    {
        rb.AddForce((Multiplier - 1f) * Physics.gravity, ForceMode.Acceleration);
    }


    #region 攻撃の処理

    //攻撃
    void Attack()
    {
        if (!action)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(0) || (Input.GetKeyDown("joystick button 2")))
            {
                AttackAnimation();

                PlayAction(0.3f);
            }
        }
    }



    void AttackAnimation()//S追加・・・・・・・・・・・・・・・・・・
    {
        animator.SetBool("isAttack", true);

    }

    [Header("攻撃半径")] public float attackhRadius = 1.5f;

    public void HitEnemy()
    {
        Collider[] collider = Physics.OverlapSphere(attackPoint.position, 1.5f);

        if (collider.Length == 0)
        {
            audioSource.volume = soundVolume[0];
            audioSource.PlayOneShot(sound[0]);//パンチ素振り
            return;
        }

        for (int i = 0; i < collider.Length; i++)
        {
            /*if (i == 0)
            {
                hitObj = collider[i].gameObject;
            }
            else
            {
                if (hitObj == collider[i].gameObject) continue;
            }*/

            //enemy射撃
            if (collider[i].gameObject.tag == "Ground")
            {
                audioSource.volume = soundVolume[0];
                audioSource.PlayOneShot(sound[0]);//パンチ素振り
            }
            if (collider[i].gameObject.tag == "burosu")
            {
                HunmerScript hunmer = collider[i].gameObject.GetComponent<HunmerScript>();
                hunmer.HP -= B_attackPower;
                atackEfect();

                if (hunmer.HP <= 0)
                {
                    HPS = hunmer;
                    Invoke("Hunmercheck", 0.01f);
                }
            }
            else if (collider[i].gameObject.tag == "kuribo")
            {
                NewwalkScript kuribo = collider[i].gameObject.GetComponent<NewwalkScript>();
                kuribo.HP = kuribo.HP - (B_attackPower / 2);
                atackEfect();

                if (kuribo.HP <= 0)
                {
                    NS = kuribo;
                    Invoke("Kuribocheck", 0.01f);
                }
            }
            else if (collider[i].gameObject.tag == "Chase")
            {
                ChaisePatapata chaise = collider[i].gameObject.GetComponent<ChaisePatapata>();
                chaise.HP -= B_attackPower;
                atackEfect();
                if (chaise.HP <= 0)
                {
                    CP = chaise;
                    Invoke("Chaisecheck", 0.01f);
                }
            }
            else if (collider[i].gameObject.tag == "patapata")
            {
                PataPataScript pataPata = collider[i].gameObject.GetComponent<PataPataScript>();
                pataPata.HP -= B_attackPower;
                atackEfect();

                if (pataPata.HP <= 0)
                {
                    PP = pataPata;
                    Invoke("Patacheck", 0.01f);
                }
            }
            else if (collider[i].gameObject.tag == "boss")
            {
                BossHP boss = collider[i].gameObject.GetComponent<BossHP>();
                boss.bossCurrentHp -= B_attackPower;

                Boss B_State = collider[i].gameObject.GetComponent<Boss>();
                B_State.nowState = Boss.bossState.Damege;

                print("boss");
                atackEfect();
            }
            else if (collider[i].gameObject.tag == "Stop")
            {
                StopEnemyScript stop = collider[i].gameObject.GetComponent<StopEnemyScript>();
                stop.HP -= B_attackPower;
                atackEfect();

                if (stop.HP <= 0)
                {
                    SP = stop;
                    Invoke("Stopcheck", 0.01f);
                }
            }
        }
    }
    void atackEfect()
    {
        if (Bag_in_enemy == 0)
        {

        }
        else if (Bag_in_enemy == 1)
        {
            audioSource.volume = soundVolume[7];
            audioSource.PlayOneShot(sound[7]);//重いパンチ2
            Instantiate(particleAtackEfect[0], new Vector3(attackPoint.transform.position.x - 1, attackPoint.transform.position.y - 1, attackPoint.transform.position.z), Quaternion.identity);
        }
        else if (Bag_in_enemy == 2)
        {
            audioSource.volume = soundVolume[8];
            audioSource.PlayOneShot(sound[8]);
            Instantiate(particleAtackEfect[1], new Vector3(attackPoint.transform.position.x - 1, attackPoint.transform.position.y - 1, attackPoint.transform.position.z), Quaternion.identity);
        }
        else if (Bag_in_enemy == 3)
        {
            audioSource.volume = soundVolume[1];
            audioSource.PlayOneShot(sound[1]);
            Instantiate(particleAtackEfect[2], new Vector3(attackPoint.transform.position.x - 1, attackPoint.transform.position.y - 1, attackPoint.transform.position.z), Quaternion.identity);
        }
    }
    void bagsAtackPwoer()
    {
        if (Bag_in_enemy == 0)
        {
            B_attackPower = 0;
        }
        else if (Bag_in_enemy == 1)
        {
            B_attackPower = 1;
        }
        else if (Bag_in_enemy == 2)
        {
            B_attackPower = 3;
        }
        else if (Bag_in_enemy == 3)
        {
            B_attackPower = 6;
        }

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(attackPoint.position, 1f);
    //}

    #endregion

    #region//接地判定
    [Header("接地判定")] public Transform[] groundCheck;
    public LayerMask groundLayer;
    bool IsGround()
    {
        if (Physics.Raycast(groundCheck[0].position, Vector3.down, 0.5f, groundLayer))
        {
            return true;
        }
        else if (Physics.Raycast(groundCheck[1].position, Vector3.down, 0.5f, groundLayer))
        {
            return true;
        }
        else if (Physics.Raycast(groundCheck[2].position, Vector3.down, 0.5f, groundLayer))
        {
            return true;
        }
        else
        {
            return false; ;
        }
    }
    #endregion

    #region//移動入力
    void MoveKey()
    {
        //キー入力
        horizontalkey = Input.GetAxisRaw("Horizontal");
        verticalKey = Input.GetAxis("Vertical");


    }
    #endregion

    #region//移動
    void Move()
    {
        if (NoInvincible)
            rb.velocity = new Vector3(xSpeed, rb.velocity.y, 0);
    }
    #endregion

    #region//アニメーション（一部
    void anime()//S追加
    {
        //Run
        if (xSpeed != 0)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

        //Jump
        if (!canJump)
        {
            animator.SetBool("isJump", true);
        }
        else
        {
            animator.SetBool("isJump", false);
        }
        //DoubleJump
        /*if (!canDoubleJump)
        {
            animator.SetBool("isjump2", false);
        }*/
    }
    #endregion

    #region//ジャンプ
    void Jump()
    {
        //ジャンプ・二段ジャンプ
        if (isGround)
        {
            canJump = true;

            if (((Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown("joystick button 0"))) && (canJump))
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);

                //print((Vector3.up * jumpSpeed, ForceMode.Impulse).ToString());

                canJump = false;

                Reset_Trigger();
            }
            canDoubleJump = true;
        }
        else
        {
            canJump = false;
            /*
            if ((Input.GetButtonDown("Jump")) && (canDoubleJump))
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                //J_Shot();
                canDoubleJump = false;
                animator.SetTrigger("isDjump");//S追加
            }
            else  */
            if (((Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown("joystick button 0"))) && (canDoubleJump))//変更
            {
                if (0 < Bag_in_enemy)
                {
                    minus = true;
                    rb.velocity = Vector3.zero;
                    rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                    //J_Shot();
                    canDoubleJump = false;
                    animator.SetTrigger("isDjump");//S追加
                    audioSource.volume = soundVolume[3];
                    audioSource.PlayOneShot(sound[3]);//二段ジャンプ音源（ナイフを投げる）
                    Instantiate(particleObject[0], this.gameObject.transform.position, Quaternion.LookRotation(new Vector3(0f, 90f, 0f), Vector3.forward));

                    Reset_Trigger();
                }
            }
            /*
            if ((0 < Bag_in_enemy))
            {
                canDoubleJump = true;
            }
            else
            {
                canDoubleJump = false;
            }*/
        }
    }
    #endregion

    #region//方向転換
    void Direction()
    {
        //方向
        if (horizontalkey > 0)
        {
            //transform.localScale = new Vector3(1, 1, 1);
            xSpeed = speed[Bag_in_enemy];
            direction = 1;
        }
        else if (horizontalkey < 0)
        {
            //transform.localScale = new Vector3(-1, 1, 1);
            xSpeed = -speed[Bag_in_enemy];
            direction = -1;
        }
        else
        {
            xSpeed = 0.0f;
        }
    }
    #endregion

    #region//ショット
    void F_Shot()
    {
        float tri = Input.GetAxis("L_R_Trigger");
        if (!action)
        {
            if (Input.GetKeyDown(KeyCode.F) || (tri > 0) || (Input.GetKeyDown("joystick button 5")))
            {
                /*
                if (Bag_in_enemy != 0)
                {
                    GameObject Bullets = Instantiate(Bullet.gameObject, transform.position, transform.rotation);
                    minus = true;

                    //bulletForce = transform.forward * bulletpower;
                    if (direction > 0)
                    {
                        bulletForce = transform.right * bulletpower;
                    }
                    else
                    {
                        bulletForce = -transform.right * bulletpower;
                    }
                    Instantiate(particleObject[1], this.transform.position, Quaternion.identity);
                    Bullets.GetComponent<Rigidbody>().AddForce(bulletForce);
                    Destroy(Bullets.gameObject, 3);
                }
                */
                animator.SetBool("isShot", true);
                PlayAction(0.3f);
            }
        }

    }
    public void F_ShotA()
    {
        if (Bag_in_enemy != 0)
        {
            GameObject Bullets = Instantiate(Bullet.gameObject, transform.position, transform.rotation);
            minus = true;

            //bulletForce = transform.forward * bulletpower;
            if (direction > 0)
            {
                bulletForce = transform.right * bulletpower;
            }
            else
            {
                bulletForce = -transform.right * bulletpower;
            }
            audioSource.volume = soundVolume[2];
            audioSource.PlayOneShot(sound[2]);//発射音（beam-gun01）
            Instantiate(particleObject[1], this.transform.position, Quaternion.identity);
            Bullets.GetComponent<Rigidbody>().AddForce(bulletForce);
            Destroy(Bullets.gameObject, 3);
        }
        #endregion

        #region//ジャンプショット 廃案
        void J_Shot()
        {
            GameObject Bullets = Instantiate(Bullet.gameObject, transform.position, transform.rotation);

            bulletForce = -transform.up * bulletpower;

            Bullets.GetComponent<Rigidbody>().AddForce(bulletForce);
            Destroy(Bullets.gameObject, 3);
        }
    }
    #endregion

    #region//敵を詰める

    [Header("袋（殴り）まとめ")] public GameObject rootBags;

    [Header("各大きさ別袋")] public GameObject[] bags;

    [Header("袋（包む）")] public GameObject openBag;

    [Header("袋（包む）落ちるスピード")] public float bagFallSpeed = 5;
    bool isOpen = false;
    public float Rote_x = 90.0f;
    private float x = 90.0f;
    [Header("袋（包む）移動スピード")] public float Mz = 30.0f;
    [Header("袋（包む）回転スピード")] public float Rx = 500.0f;
    void Put_in_Bag()
    {
        if (!action)
        {
            if ((Input.GetKeyDown(KeyCode.E) || (Input.GetKeyDown("joystick button 1"))) && Bag_in_enemy < 3)
            {
                animator.SetBool("isPutin", true);
                isOpen = true;

                Vector3 localpos = openBag.transform.localPosition;
                localpos.y = 7;
                localpos.z = -1;
                //localpos.z = 3;

                openBag.transform.localPosition = localpos;
                x = Rote_x;
                openBag.transform.localRotation = Quaternion.Euler(x, 0, 0);


                rootBags.SetActive(false);

                openBag.SetActive(true);

                PlayAction(0.4f);

                audioSource.volume = soundVolume[4];
                audioSource.PlayOneShot(sound[4]);//袋に入れる
            }
        }

        if (isOpen)
        {
            Vector3 localpos = openBag.transform.localPosition;
            localpos.y -= bagFallSpeed * Time.deltaTime;



            if (localpos.y < 0)
            {
                localpos.y = 0;
                isOpen = false;
                rootBags.SetActive(true);

                openBag.SetActive(false);
            }
            if (localpos.z < 3)
            {
                localpos.z += Mz * Time.deltaTime;
            }
            if (x < 180)
            {
                x += Rx * Time.deltaTime;
            }

            openBag.transform.localPosition = localpos;
            openBag.transform.localRotation = Quaternion.Euler(x, 0, 0);
            //openBag.transform.eulerAngles = localrot;
        }
    }

    [Header("攻撃中")] public bool action = false;
    float currentTime = 0;
    float actionTime;
    void PlayAction(float time)
    {
        currentTime = 0;
        actionTime = time;
        action = true;
    }

    void Action()
    {
        if (action)
        {
            currentTime += Time.deltaTime;

            if (currentTime > actionTime)
            {
                action = false;

                animator.SetBool("isPutin", false);
                animator.SetBool("isAttack", false);
                animator.SetBool("isShot", false);
            }
        }
    }

    public void BagopHit()
    {
        plus = true;
        Instantiate(particleObject[2], openBag.transform.position, Quaternion.identity);
    }
    #endregion

    #region// 袋内
    [Header("今の攻撃力（変更不可）")] public float B_attackPower = 3;
    [Header("包んでいる敵の数")] public int Bag_in_enemy = 3;
    [Header("１フレーム前の包んでいる敵の数")] public int rBag_in_enemy = 3;
    [Header("袋の中身を一つ　増やす")] public bool plus;
    [Header("袋の中身を一つ　減らす")] public bool minus;
    void BagsChange()
    {
        if (plus)
        {
            Bag_in_enemy++;
            plus = false;
        }
        if (minus)
        {
            Bag_in_enemy--;
            minus = false;
        }
        if (Bag_in_enemy < 0)
        {
            Bag_in_enemy = 0;
        }
        else if (Bag_in_enemy >= 4)
        {
            Bag_in_enemy = 3;
        }

        if (rBag_in_enemy != Bag_in_enemy)
        {
            bags[rBag_in_enemy].SetActive(false);
            bags[Bag_in_enemy].SetActive(true);
        }
        bagsAtackPwoer();

        rBag_in_enemy = Bag_in_enemy;
    }
    #endregion

    #region//プレイヤーHP
    [Header("プレイヤー体力")] public float PlayerHP = 100;
    [Header("無敵")] public bool NoInvincible;
    [Header("無敵時間Set（動けない）")] [SerializeField] float Start_invincibleTime = 0.5f;
    [Header("残り無敵時間")] [SerializeField] float invincibleTime;

    [Header("プレイヤー体力MAX")] public float playerMaxHP = 100;

    void Invincible()
    {
        if (!NoInvincible)
        {
            invincibleTime -= Time.deltaTime;
        }
        if (invincibleTime <= 0)
        {
            NoInvincible = true;
            invincibleTime = Start_invincibleTime;
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        }
    }
    private float pos;
    private void OnTriggerStay(Collider other)
    {

        pos = other.transform.position.x - this.transform.position.x;
        if (NoInvincible)
        {
            if (other.tag == "burosu")
            {
                NoInvincible = false;
                PlayerHP -= 5;
                knockback(pos);
                //print("Inburpsu");
            }
            else if (other.tag == "hunmer")
            {
                PlayerHP -= 5;
                NoInvincible = false;
                knockback(pos);
                //print("hunmer");
            }
            else if (other.tag == "kuribo")
            {
                PlayerHP -= 5;
                NoInvincible = false;
                knockback(pos);
                //print("Stop");
            }
            else if (other.tag == "Stop")
            {
                PlayerHP -= 5;
                NoInvincible = false;
                knockback(pos);
                //print("Stop");
            }
            else if (other.tag == "Chase")
            {
                PlayerHP -= 5;
                NoInvincible = false;
                knockback(pos);
                //print("InChaise");
            }
            else if (other.tag == "patapata")
            {
                PlayerHP -= 5;
                NoInvincible = false;
                knockback(pos);
                //print("Inpatapata");
            }
            else if (other.tag == "boss")
            {
                PlayerHP -= 5;
                NoInvincible = false;
                knockback(pos);
                //print("本体");
            }
            else if (other.tag == "LeftArm")
            {
                PlayerHP -= 20;
                NoInvincible = false;
                knockback(pos);
                //print("腕");
            }
            else if (other.tag == "RightArm")
            {
                PlayerHP -= 10;
                NoInvincible = false;
                knockback(pos);
                //print("右腕");
            }
        }
    }
    [Header("ノックバックの力")] public float knockbackPower;


    void knockback(float pos)
    {
        rb.velocity = Vector3.zero;
        if (pos > 0)
        {
            rb.AddForce(((transform.up - transform.right)).normalized * knockbackPower, ForceMode.Impulse);
        }
        else if (pos < 0)
        {
            rb.AddForce(((transform.up + transform.right)).normalized * knockbackPower, ForceMode.Impulse);
        }
        audioSource.volume = soundVolume[5];
        audioSource.PlayOneShot(sound[5]);//ダメージ音
        //print("ノックバック");
    }

    public static bool SetPlayerDeathFlag()
    {
        return playerDeathFlag;
    }

    #endregion

    public void breakWall()
    {
        audioSource.volume = soundVolume[6];
        audioSource.PlayOneShot(sound[6]);
    }
    public void Reset_Trigger()
    {
        //print("CanAciton!!");
    }

    #endregion

    #region　名草
    void Stopcheck()
    {
        if (Bag_in_enemy == 1)
        {
            SP.FirstPower();

        }

        if (Bag_in_enemy == 2)
        {
            SP.SecondPower();

        }

        if (Bag_in_enemy == 3)
        {
            SP.MostPower();

        }
    }

    void Patacheck()
    {
        if (Bag_in_enemy == 1)
        {
            PP.FirstPower();

        }

        if (Bag_in_enemy == 2)
        {
            PP.SecondPower();

        }

        if (Bag_in_enemy == 3)
        {
            PP.MostPower();

        }
    }

    void Chaisecheck()
    {
        if (Bag_in_enemy == 1)
        {
            CP.FirstPower();

        }

        if (Bag_in_enemy == 2)
        {
            CP.SecondPower();

        }

        if (Bag_in_enemy == 3)
        {
            CP.MostPower();

        }
    }

    void Hunmercheck()
    {
        if (Bag_in_enemy == 1)
        {
            HPS.FirstPower();

        }

        if (Bag_in_enemy == 2)
        {
            HPS.SecondPower();

        }

        if (Bag_in_enemy == 3)
        {
            HPS.MostPower();

        }
    }

    void Kuribocheck()
    {
        if (Bag_in_enemy == 1)
        {
            NS.FirstPower();

        }

        if (Bag_in_enemy == 2)
        {
            NS.SecondPower();

        }

        if (Bag_in_enemy == 3)
        {
            NS.MostPower();

        }
    }

    #endregion
}