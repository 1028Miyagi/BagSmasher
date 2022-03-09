using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaisePatapata : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    GameObject Player;
    GameObject Chase;

    //敵ステータス
    float CPdistance;
    public bool PActive = false;
    public float FryPos;
    public float moveSpeed = 3.5f;

    [SerializeField] Slider flySlider;
    public float HP;
    float maxHP = 6;

    //敵フラグ
    public bool action = true;
    //敵クールタイム
    public float coolTime = 3.0f;
    GameObject target;

    private Animator animator;
    public bool death = false;
    public bool right = true;
    //飛ばす力と距離
    float FPower = 10000;
    float SPower = 25000;
    float MPower = 50000;


    public GameObject[] particleObject;
    GameObject Obj;


    [Header("音源")] public AudioSource audioSource;
    public AudioClip sound;
    public float soundVolume;


    void Start()
    {
        Chase = this.gameObject;
        FryPos = Chase.transform.position.y;
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player");
        Player = GameObject.Find("Player");

        flySlider.value = 1;
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {

        float Diference;
        Diference = Player.transform.position.x - Chase.transform.position.x;

        CPdistance = (target.transform.position - Chase.transform.position).sqrMagnitude;

        flySlider.value = HP / maxHP;

        //AI
        if (action)
        {
            //飛行敵の動き
            patapata();
            //

            if (CPdistance <= 250)
            {
                Chaise(Diference, right);
            }

        }

        if (!action)
        {

            transform.localPosition = new Vector3(Chase.transform.position.x, Chase.transform.position.y, Chase.transform.position.z);
            coolTime -= Time.deltaTime;

            if (coolTime <= 0.0f && !death)
            {
                action = true;
                animator.enabled = true;
                GetComponent<Renderer>().material.color = Color.magenta;
                coolTime = 3.0f;
            }
        }

    }
    #region 敵AI
    private void patapata()
    {
        Chase.transform.position = new Vector3(Chase.transform.position.x, FryPos + Mathf.PingPong(Time.time / 1, 4f), Chase.transform.position.z);
    }
    #endregion

    #region 敵移動
    private void Chaise(float Diference, bool right)
    {

        if (Diference <= 0.0)
        {
            right = false;
            transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            right = true;
            transform.localEulerAngles = new Vector3(0, -90, 0);
        }

        if (!right)
        {
            Vector3 targetPos = target.transform.position;

            targetPos.x = transform.position.x;

            transform.position = transform.position + (-transform.forward) * moveSpeed * Time.deltaTime;
        }
        if (right)
        {
            Vector3 targetPos = target.transform.position;

            targetPos.x = transform.position.x;

            transform.position = transform.position - transform.forward * moveSpeed * Time.deltaTime;
        }
    }
    #endregion

    #region 吹っ飛ばす処理
    public void FirstPower()
    {
        HP = 0.0f;

        action = false;

        animator.enabled = false;

        rigidbody.AddForce((-transform.up * 2 + transform.forward * 2).normalized * FPower, ForceMode.Force);

        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 3f);

        flySlider.gameObject.SetActive(false);

    }
    public void SecondPower()
    {
        HP = 0.0f;
        action = false;

        animator.enabled = false;

        rigidbody.AddForce((-transform.up * 2 + transform.forward * 1.5f).normalized * SPower, ForceMode.Force);

        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 4f);

        flySlider.gameObject.SetActive(false);
    }

    public void MostPower()
    {
        HP = 0.0f;
        action = false;

        animator.enabled = false;

        rigidbody.AddForce((-transform.up * 2 + transform.forward * 1.5f).normalized * MPower, ForceMode.Force);

        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 3f);

        flySlider.gameObject.SetActive(false);
    }

    #endregion

    #region 接触判定
    void OnTriggerEnter(Collider other)
    {
        //接触したオブジェクトのタグが"Player"のとき
        if (other.CompareTag("PlayerBullets"))
        {
            //オブジェクトの色を赤に変更する
            // GetComponent<Renderer>().material.color = Color.gray;
            if (action)
            {
                Obj = Instantiate(particleObject[0], new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.Euler(90, 0, 0));
                Obj.transform.parent = this.transform;
                Obj = Instantiate(particleObject[1], new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
                Obj.transform.parent = this.transform;
            }

            action = false;
            animator.enabled = false;
        }
        //停止している敵との判定
        if (other.CompareTag("Stop") && death)
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
        }

        //地面との判定
        if (other.CompareTag("Ground"))
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
        }
        //袋との接触判定
        if (other.CompareTag("Bag") && (!death))
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
            flySlider.gameObject.SetActive(false);
        }

        if (other.CompareTag("boss"))
        {
            flySlider.gameObject.SetActive(false);

            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
        }
        //吹っ飛んだ敵との判定
        if (other.CompareTag("Attacktag"))
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
        }
    }
    //地面との判定
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
    #endregion
}