using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI;
//using UnityEngine.SceneManagement;
using UnityEngine.UI; //


public class EnemyScript : MonoBehaviour
{
    //�G�X�e�[�^�X
    public float EnemySpeed = 0.1f;
    float Kdistance;
    public float coolTime = 3.0f;

    //
    [SerializeField] Slider walkSlider;
    public float HP;
    float maxHP = 10;
    //


    //�I�u�W�F�N�g
    GameObject Kuribo;
    GameObject Player;


    //�s���t���O
    public bool KActive = false;
    public bool flag = true;
    public bool action = true;

   
    //�G���W
    private float StartDistance;
    public float Diference;

    //
    private BagJudge bagJudge;

    public GameObject Bag;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Kuribo = this.gameObject;
        Player = GameObject.Find("Player");

        StartDistance = Kuribo.transform.position.x;
        //bagJudge = Bag.GetComponent<BagJudge>();
        animator = GetComponent<Animator>();

        //
        walkSlider.value = 1;
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        Diference = StartDistance - Kuribo.transform.position.x;

        //�N���{�[�̓���
        Kdistance = Mathf.Abs(Player.transform.position.x - Kuribo.transform.position.x);

        walkSlider.value = HP / maxHP;

        //�ړ����G�͈�
        if (action)
        {
            if (Kdistance <= 30)
            {
                //kuribo();
                Kuribo2();
            }
            else
            {
                KActive = false;
            }
        }

        if (!action)
        {
            coolTime -= Time.deltaTime;

            if (coolTime <= 0.0f)
            {
                action = true;
                GetComponent<Renderer>().material.color = Color.red;
                animator.enabled = true;
                coolTime = 3.0f;
            }
        }

        if (HP <= 0)
        {
            //
            HP = 0.0f;
            walkSlider.gameObject.SetActive(false);

            Destroy(gameObject);
        }

        //

    }

    private void kuribo()
    {
        //�N���{�[AI
        KActive = true;


        if (KActive)
        {
            //���񋗗��ƓG�̌����ω�
            if (Diference >= 13.0)
            {
                print("13�ȏ�");
                flag = false;

                Kuribo.transform.localEulerAngles = new Vector3(0, -90, 0);
                //Kuribo.transform.localScale = new Vector3(-0.7f, 0.7f, 0.7f);
            }
            else if (Diference <= 0.0)
            {
                print("13�ȉ�");
                flag = true;

                Kuribo.transform.localEulerAngles = new Vector3(0, 90, 0);
                //Kuribo.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }


            //�G�ړ�����
            if (!flag) 
            {
                Kuribo.transform.position += transform.forward * EnemySpeed * Time.deltaTime;
            }

            if (flag) 
            {
                Kuribo.transform.position -= transform.forward * EnemySpeed * Time.deltaTime;
            }

        }


    }

    #region �ǉ�
   // bool kuriboIsRight = true;

    Vector3[] target;

    Vector3 vector;

    int index = 0;
    void Kuribo2()
    {
       
        if (!KActive)
        {
            KActive = true;

            //�N���{�[�ƃv���C���[�̏��������̎擾
            bool playerIsRight = (Player.transform.position.x - Kuribo.transform.position.x) > 0;

            target = new Vector3[2];

            //�v���C���[�̏����ʒu�̎擾
            target[0] = Player.transform.position;
            target[1] = Player.transform.position;

            if (playerIsRight)
            {
                target[1].x = transform.position.x - Mathf.Abs(transform.position.x - Player.transform.position.x);
            }
            else
            {
                target[1].x = transform.position.x + Mathf.Abs(transform.position.x - Player.transform.position.x);
            }

            //�N���{�[�̊p�x�̕ύX
            Kuribo.transform.localEulerAngles = new Vector3(0, playerIsRight ? -90 : 90, 0);
          
            vector = (target[index] - transform.position).normalized;
        }
      

        transform.position += vector * EnemySpeed * Time.deltaTime;

        float distance = Mathf.Abs(target[index].x - transform.position.x);

        if (distance < 1)
        {

            Kuribo.transform.localEulerAngles = new Vector3(0, -90, 0);


            index++;
           
            if (index == 2)
            {
                index = 0;

                Kuribo.transform.localEulerAngles = new Vector3(0, 90, 0);
            }
            
            vector = (target[index] - transform.position).normalized;
         

        }
    }

    #endregion

    void OnTriggerEnter(Collider other)
    {
        //�ڐG�����I�u�W�F�N�g�̃^�O��"Player"�̂Ƃ�
        if (other.CompareTag("PlayerBullets"))
        {
            //�I�u�W�F�N�g�̐F��ԂɕύX����
            GetComponent<Renderer>().material.color = Color.gray;

            action = false;

            Debug.Log("hit");
            animator.enabled = false;

        }

        if (other.CompareTag("Bag"))
        {
            Destroy(gameObject);

            //
            walkSlider.gameObject.SetActive(false);

            //bagJudge.AttackUp();
          
        }

        if (other.CompareTag("boss")) //�ǉ��_
        {
            //�ǉ�
            walkSlider.gameObject.SetActive(false);

            Destroy(gameObject);
        }
    }

}
