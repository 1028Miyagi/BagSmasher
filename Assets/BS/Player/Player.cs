using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //����

    GameObject stoptarget;
    StopEnemyScript SP;
    PataPataScript PP;
    ChaisePatapata CP;
    HunmerScript HPS;
    NewwalkScript NS;

    //�����܂�

    #region�@ //����

    //�ړ�
    [Header("���鑬�x")] public float[] speed;//���x
    [Header("�W�����v��")] public float jumpSpeed;//�W�����v���x
    float horizontalkey, verticalKey;
    [Header("�����Ă������")] public float xSpeed = 1.0f;
    //�e
    [Header("�e")] public GameObject Bullet; // �e
    [Header("�e�̔��ˑ��x")] public float bulletpower;
    private Vector3 bulletForce;

    //����
    private Rigidbody rb = null;
    private bool isGround = false;

    //�W�����v
    [Header("�W�����v�o����")] public bool canJump = false;
    [Header("��i�W�����v�o����")] public bool canDoubleJump = true;

    //�A�j���[�V����
    [Header("�v���C���[animator")] public Animator animator;//S�ǉ�

    //�e�X�g�p
    public float Multiplier = 1f;
    //public bool ON;

    public float direction = 1.0f;

    [SerializeField] Transform attackPoint;

    [SerializeField] Image playerSlider;

    public static bool playerDeathFlag;

    [Header("�G�t�F�N�g")] public GameObject[] particleObject;
    [Header("�U���G�t�F�N�g")] public GameObject[] particleAtackEfect;

    [SerializeField] Timer timer;


    [Header("����")] public AudioSource audioSource;
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
        //�ڒn������擾�@GroundCheck����
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


    #region �U���̏���

    //�U��
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



    void AttackAnimation()//S�ǉ��E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E�E
    {
        animator.SetBool("isAttack", true);

    }

    [Header("�U�����a")] public float attackhRadius = 1.5f;

    public void HitEnemy()
    {
        Collider[] collider = Physics.OverlapSphere(attackPoint.position, 1.5f);

        if (collider.Length == 0)
        {
            audioSource.volume = soundVolume[0];
            audioSource.PlayOneShot(sound[0]);//�p���`�f�U��
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

            //enemy�ˌ�
            if (collider[i].gameObject.tag == "Ground")
            {
                audioSource.volume = soundVolume[0];
                audioSource.PlayOneShot(sound[0]);//�p���`�f�U��
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
            audioSource.PlayOneShot(sound[7]);//�d���p���`2
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

    #region//�ڒn����
    [Header("�ڒn����")] public Transform[] groundCheck;
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

    #region//�ړ�����
    void MoveKey()
    {
        //�L�[����
        horizontalkey = Input.GetAxisRaw("Horizontal");
        verticalKey = Input.GetAxis("Vertical");


    }
    #endregion

    #region//�ړ�
    void Move()
    {
        if (NoInvincible)
            rb.velocity = new Vector3(xSpeed, rb.velocity.y, 0);
    }
    #endregion

    #region//�A�j���[�V�����i�ꕔ
    void anime()//S�ǉ�
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

    #region//�W�����v
    void Jump()
    {
        //�W�����v�E��i�W�����v
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
                animator.SetTrigger("isDjump");//S�ǉ�
            }
            else  */
            if (((Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown("joystick button 0"))) && (canDoubleJump))//�ύX
            {
                if (0 < Bag_in_enemy)
                {
                    minus = true;
                    rb.velocity = Vector3.zero;
                    rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                    //J_Shot();
                    canDoubleJump = false;
                    animator.SetTrigger("isDjump");//S�ǉ�
                    audioSource.volume = soundVolume[3];
                    audioSource.PlayOneShot(sound[3]);//��i�W�����v�����i�i�C�t�𓊂���j
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

    #region//�����]��
    void Direction()
    {
        //����
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

    #region//�V���b�g
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
            audioSource.PlayOneShot(sound[2]);//���ˉ��ibeam-gun01�j
            Instantiate(particleObject[1], this.transform.position, Quaternion.identity);
            Bullets.GetComponent<Rigidbody>().AddForce(bulletForce);
            Destroy(Bullets.gameObject, 3);
        }
        #endregion

        #region//�W�����v�V���b�g �p��
        void J_Shot()
        {
            GameObject Bullets = Instantiate(Bullet.gameObject, transform.position, transform.rotation);

            bulletForce = -transform.up * bulletpower;

            Bullets.GetComponent<Rigidbody>().AddForce(bulletForce);
            Destroy(Bullets.gameObject, 3);
        }
    }
    #endregion

    #region//�G���l�߂�

    [Header("�܁i����j�܂Ƃ�")] public GameObject rootBags;

    [Header("�e�傫���ʑ�")] public GameObject[] bags;

    [Header("�܁i��ށj")] public GameObject openBag;

    [Header("�܁i��ށj������X�s�[�h")] public float bagFallSpeed = 5;
    bool isOpen = false;
    public float Rote_x = 90.0f;
    private float x = 90.0f;
    [Header("�܁i��ށj�ړ��X�s�[�h")] public float Mz = 30.0f;
    [Header("�܁i��ށj��]�X�s�[�h")] public float Rx = 500.0f;
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
                audioSource.PlayOneShot(sound[4]);//�܂ɓ����
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

    [Header("�U����")] public bool action = false;
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

    #region// �ܓ�
    [Header("���̍U���́i�ύX�s�j")] public float B_attackPower = 3;
    [Header("���ł���G�̐�")] public int Bag_in_enemy = 3;
    [Header("�P�t���[���O�̕��ł���G�̐�")] public int rBag_in_enemy = 3;
    [Header("�܂̒��g����@���₷")] public bool plus;
    [Header("�܂̒��g����@���炷")] public bool minus;
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

    #region//�v���C���[HP
    [Header("�v���C���[�̗�")] public float PlayerHP = 100;
    [Header("���G")] public bool NoInvincible;
    [Header("���G����Set�i�����Ȃ��j")] [SerializeField] float Start_invincibleTime = 0.5f;
    [Header("�c�薳�G����")] [SerializeField] float invincibleTime;

    [Header("�v���C���[�̗�MAX")] public float playerMaxHP = 100;

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
                //print("�{��");
            }
            else if (other.tag == "LeftArm")
            {
                PlayerHP -= 20;
                NoInvincible = false;
                knockback(pos);
                //print("�r");
            }
            else if (other.tag == "RightArm")
            {
                PlayerHP -= 10;
                NoInvincible = false;
                knockback(pos);
                //print("�E�r");
            }
        }
    }
    [Header("�m�b�N�o�b�N�̗�")] public float knockbackPower;


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
        audioSource.PlayOneShot(sound[5]);//�_���[�W��
        //print("�m�b�N�o�b�N");
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

    #region�@����
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