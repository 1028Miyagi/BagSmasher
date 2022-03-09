using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewwalkScript : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;

    GameObject Player;
    GameObject Walk;

    [SerializeField] Slider WalkEnemySlider;
    //敵ステータス
    public float EnemySpeed = 2.0f;
    float maxHP = 6;
    public float HP;
    private Animator animator;
    public bool action = true;
    public bool right = true;
    public bool death = false;
    public float coolTime = 3.0f;

    public GameObject[] particleObject;
    GameObject Obj;

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
        Walk = this.gameObject;
        Player = GameObject.Find("Player");

        WalkEnemySlider.value = 1;
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //敵とプレイヤーの距離差
        float Diference;
        Diference = Player.transform.position.x - Walk.transform.position.x;
        //距離に応じての反転処理
        if (Diference <= 0.0)
        {
            right = false;
        }
        else
        {
            right = true;
        }

        WalkEnemySlider.value = HP / maxHP;

        //行動フラグ
        if (action)
        {
            Walk.transform.position -= transform.forward * EnemySpeed * Time.deltaTime;
        }
        //行動フラグと敵停止処理
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
    #region 反転処理
    //障害物との接触判定を受け取り反転する処理
    public void TurnObj()
    {
        float roty = Walk.transform.localEulerAngles.y;

        if (roty == 90)
        {
            roty = 270;
        }
        else
        {
            roty = 90;
        }

        Walk.transform.localEulerAngles = new Vector3(0, roty, 0);
    }
    #endregion

    #region 吹っ飛ばす処理
    //一番低い態での吹っ飛び距離
    public void FirstPower()
    {
        HP = 0.0f;
        animator.enabled = false;
        if (!right)
        {
            transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, -90, 0);
        }
        rigidbody.AddForce((transform.up + transform.forward * 2).normalized * FPower, ForceMode.Force);
        //タグ変更
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 1.5f);

        WalkEnemySlider.gameObject.SetActive(false);
    }
    //真ん中の力での吹っ飛び距離
    public void SecondPower()
    {
        HP = 0.0f;
        animator.enabled = false;
        if (!right)
        {
            transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, -90, 0);
        }

        rigidbody.AddForce((transform.up + transform.forward * 1.5f).normalized * SPower, ForceMode.Force);
        //タグ変更
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 3.5f);

        WalkEnemySlider.gameObject.SetActive(false);
    }

    //最大の力での吹っ飛び距離
    public void MostPower()
    {
        HP = 0.0f;
        animator.enabled = false;

        if (!right)
        {
            transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, -90, 0);
        }
        rigidbody.AddForce((transform.up + (transform.forward * 1.5f)).normalized * MPower, ForceMode.Force);
        //タグ変更
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 10f);

        WalkEnemySlider.gameObject.SetActive(false);
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
                Obj = Instantiate(particleObject[0], new Vector3(transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.Euler(90, 0, 0));
                Obj.transform.parent = this.transform;
                Obj = Instantiate(particleObject[1], new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
                Obj.transform.parent = this.transform;
            }

            Debug.Log("hit");
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
            WalkEnemySlider.gameObject.SetActive(false);
        }

        if (other.CompareTag("boss")) //追加点
        {
            //追加
            WalkEnemySlider.gameObject.SetActive(false);

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