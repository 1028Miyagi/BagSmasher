using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StopEnemyScript : MonoBehaviour
{
    [SerializeField] GameObject BagPoint;
    [SerializeField] Rigidbody rigidbody;

    GameObject Player;
    GameObject Stop;

    [SerializeField] Slider stopEnemySlider;
    //敵体力
    float maxHP = 10;
    public float HP;
    private Animator animator;
    //各フラグ
    public bool action = true;
    public bool right = true;
    public bool death = false;
    //敵停止時間
    public float coolTime = 3.0f;

    public GameObject[] particleObject;
    GameObject Obj;

    //飛ばす力と距離
    float FPower = 8000;
    float SPower = 15000;
    float MPower = 50000;


    [Header("音源")] public AudioSource audioSource;
    public AudioClip sound;
    public float soundVolume;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        BagPoint = GameObject.Find("Bags");
        Stop = this.gameObject;
        Player = GameObject.Find("Player");

        stopEnemySlider.value = 1;
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //敵とプレイヤーの距離差
        float Diference;

        stopEnemySlider.value = HP / maxHP;

        Diference = Player.transform.position.x - Stop.transform.position.x;
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

        //弾があたった時の停止処理
        if (!action)
        {
            coolTime -= Time.deltaTime;

            if (coolTime <= 0.0f && !death)
            {
                action = true;
                GetComponent<Renderer>().material.color = Color.red;
                animator.enabled = true;
                coolTime = 3.0f;
            }
        }

    }

    #region 吹っ飛ばし処理

    //一番低い態での吹っ飛び距離
    public void FirstPower()
    {
        HP = 0.0f;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + transform.forward * 2).normalized * FPower, ForceMode.Force);
        //タグ変更
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 3f);

        stopEnemySlider.gameObject.SetActive(false);
    }

    //真ん中の力での吹っ飛び距離
    public void SecondPower()
    {
        HP = 0.0f;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + transform.forward * 1.5f).normalized * SPower, ForceMode.Force);
        //タグ変更
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 5f);

        stopEnemySlider.gameObject.SetActive(false);
    }

    //最大の力での吹っ飛び距離
    public void MostPower()
    {
        HP = 0.0f;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + (transform.forward * 1.5f)).normalized * MPower, ForceMode.Force);
        //タグ変更
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 10f);

        stopEnemySlider.gameObject.SetActive(false);
    }

    #endregion

    #region 接触判定
    void OnTriggerEnter(Collider other)
    {
        //接触したオブジェクトのタグが"Player"のときの判定
        if (other.CompareTag("PlayerBullets"))
        {
            //オブジェクトの色を赤に変更する
            //GetComponent<Renderer>().material.color = Color.gray;
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
        //吹っ飛んだ敵との判定
        if (other.CompareTag("Attacktag"))
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
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
        //袋との接触判定
        if (other.CompareTag("Bag") && (!death))
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
            HP = 0.0f;
            stopEnemySlider.gameObject.SetActive(false);
        }

        if (other.CompareTag("boss")) //追加点
        {
            //追加
            stopEnemySlider.gameObject.SetActive(false);
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