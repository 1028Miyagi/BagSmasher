using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PataPataScript : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    GameObject Player;
    GameObject Fry;
    //GXe[^X
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
    //òÎ·ÍÆ£
    float FPower = 8000;
    float SPower = 15000;
    float MPower = 50000;

    [Header("¹¹")] public AudioSource audioSource;
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
        //p^p^Ì®«
        patapata();
        //
    }

    #region GAI
    private void patapata()
    {
        //p^p^AI
        float Diference;
        //GÆvC[Ì£·
        Diference = Player.transform.position.x - Fry.transform.position.x;

        patapataSlider.value = HP / maxHP;
        //£É¶ÄÌ½]
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

    #region ÁòÎ·
    //êÔá¢ÔÅÌÁòÑ£
    public void FirstPower()
    {
        HP = 0.0f;
        action = false;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + transform.forward * 2).normalized * FPower, ForceMode.Force);
        //^OÏX
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 3f);

        patapataSlider.gameObject.SetActive(false);
    }
    //^ñÌÍÅÌÁòÑ£
    public void SecondPower()
    {
        HP = 0.0f;
        action = false;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + transform.forward * 1.5f).normalized * SPower, ForceMode.Force);
        //^OÏX
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 4f);

        patapataSlider.gameObject.SetActive(false);
    }

    //ÅåÌÍÅÌÁòÑ£
    public void MostPower()
    {
        HP = 0.0f;
        action = false;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + (transform.forward * 1.5f)).normalized * MPower, ForceMode.Force);
        //^OÏX
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 3f);

        patapataSlider.gameObject.SetActive(false);
    }

    #endregion

    #region ÚG»è
    void OnTriggerEnter(Collider other)
    {
        //ÚGµ½IuWFNgÌ^Oª"Player"ÌÆ«
        if (other.CompareTag("PlayerBullets"))
        {
            //IuWFNgÌFðÔÉÏX·é
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

        //â~µÄ¢éGÆÌ»è
        if (other.CompareTag("Stop") && death)
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);
            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
        }

        //nÊÆÌ»è
        if (other.CompareTag("Ground"))
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);
            Destroy(gameObject);
        }
        //ÜÆÌÚG»è
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
            //ÇÁ
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
    //nÊÆÌ»è
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