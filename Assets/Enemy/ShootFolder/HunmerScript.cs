using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HunmerScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject HunmerPoint;
    [SerializeField] GameObject Hunmer;

    [SerializeField] Rigidbody rigidbody;

    float Bdistance;

    GameObject HE;
    GameObject Player;

    //投擲クールタイム
    //投擲速度
    private float Rangetime = 1.5f;
    private float speed = 10.5f;

    //
    [SerializeField] Slider hunmerSlider;
    public float HP;
    float maxHP = 6;

    public float coolTime = 3.0f;

    public bool action = true;
    public bool right = true;
    public bool death = false;
    float FPower = 8000;      //力
    float SPower = 15000;
    float MPower = 50000;

    private Animator animator;

    public GameObject[] particleObject;
    GameObject Obj;

    [Header("音源")] public AudioSource audioSource;
    public AudioClip sound;
    public float soundVolume;

    void Start()
    {
        HE = this.gameObject;
        Player = GameObject.Find("Player");
        animator = GetComponent<Animator>();

        hunmerSlider.value = 1;
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        float Diference;
        bool BActive = false;

        Diference = Player.transform.position.x - HE.transform.position.x;

        Bdistance = (Player.transform.position - HE.transform.position).sqrMagnitude;

        hunmerSlider.value = HP / maxHP;

        //敵索敵範囲
        if (action)
        {
            if (Bdistance <= 1000)
            {
                burosu(Diference, BActive, right);
            }
            else
            {
                BActive = false;
            }
        }

        if (!action)
        {
            coolTime -= Time.deltaTime;

            if (coolTime <= 0.0f && !death)
            {
                action = true;
                animator.enabled = true;
                GetComponent<Renderer>().material.color = Color.blue;

                coolTime = 3.0f;
            }
        }


    }

    #region 敵Ai
    private void burosu(float Diference, bool BActive, bool right)
    {
        //ブロスの動き
        BActive = true;

        if (BActive)
        {
            if (Diference <= 0.0)
            {
                right = false;

            }
            else
            {
                right = true;

            }

            if (!right)
            {

                transform.localEulerAngles = new Vector3(0, -90, 0);

                Rangetime -= Time.deltaTime;

                if (Rangetime <= 0.0f)
                {
                    animator.SetBool("Attack", true);

                    Rangetime = 2.0f;
                }

            }

            if (right)
            {

                transform.localEulerAngles = new Vector3(0, 90, 0);

                Rangetime -= Time.deltaTime;

                if (Rangetime <= 0.0f)
                {
                    animator.SetBool("Attack", true);

                    Rangetime = 2.0f;
                }
            }

        }

    }
    #endregion

    #region 吹っ飛ばす処理
    public void FirstPower()
    {
        HP = 0.0f;

        animator.enabled = false;

        rigidbody.AddForce((transform.up + (-transform.forward * 2)).normalized * FPower, ForceMode.Force);

        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 3f);

        hunmerSlider.gameObject.SetActive(false);
    }
    public void SecondPower()
    {
        HP = 0.0f;
        animator.enabled = false;

        rigidbody.AddForce((transform.up + (-transform.forward * 1.5f)).normalized * SPower, ForceMode.Force);

        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 5f);

        hunmerSlider.gameObject.SetActive(false);
    }

    public void MostPower()
    {
        HP = 0.0f;
        animator.enabled = false;

        rigidbody.AddForce((transform.up + (-transform.forward * 1.5f)).normalized * MPower, ForceMode.Force);

        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 10f);

        hunmerSlider.gameObject.SetActive(false);
    }
    #endregion

    #region 接触判定
    void OnTriggerEnter(Collider other)
    {
        //接触したオブジェクトのタグが"Player"のとき
        if (other.CompareTag("PlayerBullets"))
        {
            //オブジェクトの色を赤に変更する
            GetComponent<Renderer>().material.color = Color.gray;

            if (action)
            {
                Obj = Instantiate(particleObject[0], new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.Euler(90, 0, 0));
                Obj.transform.parent = this.transform;
                Obj = Instantiate(particleObject[1], new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
                Obj.transform.parent = this.transform;
            }

            action = false;

            animator.enabled = false;
        }

        if (other.CompareTag("Attacktag"))
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
        }


        if (other.CompareTag("Stop") && death)
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
        }

        if (other.CompareTag("Bag") && (!death))
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);

            hunmerSlider.gameObject.SetActive(false);
        }

        if (other.CompareTag("boss"))
        {
            hunmerSlider.gameObject.SetActive(false);

            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && death)
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
        }
    }
    public void PlayAttack()
    {
        Vector3 bulletPosition = HunmerPoint.transform.position;

        GameObject newBall = Instantiate(Hunmer, bulletPosition, transform.rotation);

        newBall.GetComponent<Rigidbody>().AddForce((HunmerPoint.transform.forward + transform.up).normalized * speed, ForceMode.Impulse);

        // 出現させたボールの名前を"bullet"に変更
        newBall.name = Hunmer.name;
        // 出現させたボールを2.0秒後に消す
        Destroy(newBall, 2.0f);

        animator.SetBool("Attack", false);
    }
    #endregion
}