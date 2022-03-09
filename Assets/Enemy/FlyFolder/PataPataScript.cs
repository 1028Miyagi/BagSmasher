using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PataPataScript : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    GameObject Player;
    GameObject Fry;
    //敵ステータス
    public bool PActive = false;
    public float FryPos;
    public bool action = true;
    public float coolTime = 3.0f;

    private Animator animator;
    public bool death = false;
    public bool right = true;


    [SerializeField] Slider patapataSlider;
    public float HP;
    float maxHP = 6;
    //飛ばす力と距離
    float FPower = 8000;
    float SPower = 15000;
    float MPower = 50000;

    [Header("音源")] public AudioSource audioSource;
    public AudioClip sound;
    public float soundVolume;

    public GameObject[] particleObject;
    GameObject Obj;

    void Start()
    {
        Fry = this.gameObject;
        FryPos = Fry.transform.position.y;
        animator = GetComponent<Animator>();
        Player = GameObject.Find("Player");

        patapataSlider.value = 1;
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //パタパタの動き
        patapata();
        //
    }

    #region 敵AI
    private void patapata()
    {
        //パタパタAI
        float Diference;
        //敵とプレイヤーの距離差
        Diference = Player.transform.position.x - Fry.transform.position.x;

        patapataSlider.value = HP / maxHP;
        //距離に応じての反転処理
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

        if (action)
        {
            Fry.transform.position = new Vector3(Fry.transform.position.x, FryPos + Mathf.PingPong(Time.time / 1, 2f), Fry.transform.position.z);
        }

        if (!action)
        {

            transform.localPosition = new Vector3(Fry.transform.position.x, Fry.transform.position.y, Fry.transform.position.z);

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
    #endregion

    #region 吹っ飛ばす処理
    //一番低い態での吹っ飛び距離
    public void FirstPower()
    {
        HP = 0.0f;
        action = false;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + transform.forward * 2).normalized * FPower, ForceMode.Force);
        //タグ変更
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 3f);

        patapataSlider.gameObject.SetActive(false);
    }
    //真ん中の力での吹っ飛び距離
    public void SecondPower()
    {
        HP = 0.0f;
        action = false;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + transform.forward * 1.5f).normalized * SPower, ForceMode.Force);
        //タグ変更
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 4f);

        patapataSlider.gameObject.SetActive(false);
    }

    //最大の力での吹っ飛び距離
    public void MostPower()
    {
        HP = 0.0f;
        action = false;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + (transform.forward * 1.5f)).normalized * MPower, ForceMode.Force);
        //タグ変更
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 3f);

        patapataSlider.gameObject.SetActive(false);
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


            patapataSlider.gameObject.SetActive(false);
        }

        if (other.CompareTag("boss"))
        {
            //追加
            patapataSlider.gameObject.SetActive(false);

            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);
            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);
            Destroy(gameObject);
        }
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