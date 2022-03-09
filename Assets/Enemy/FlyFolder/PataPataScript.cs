using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PataPataScript : MonoBehaviour
{
    [SerializeField] Rigidbody rigidbody;
    GameObject Player;
    GameObject Fry;
    //�G�X�e�[�^�X
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
    //��΂��͂Ƌ���
    float FPower = 8000;
    float SPower = 15000;
    float MPower = 50000;

    [Header("����")] public AudioSource audioSource;
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
        //�p�^�p�^�̓���
        patapata();
        //
    }

    #region �GAI
    private void patapata()
    {
        //�p�^�p�^AI
        float Diference;
        //�G�ƃv���C���[�̋�����
        Diference = Player.transform.position.x - Fry.transform.position.x;

        patapataSlider.value = HP / maxHP;
        //�����ɉ����Ă̔��]����
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

    #region ������΂�����
    //��ԒႢ�Ԃł̐�����ы���
    public void FirstPower()
    {
        HP = 0.0f;
        action = false;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + transform.forward * 2).normalized * FPower, ForceMode.Force);
        //�^�O�ύX
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 3f);

        patapataSlider.gameObject.SetActive(false);
    }
    //�^�񒆂̗͂ł̐�����ы���
    public void SecondPower()
    {
        HP = 0.0f;
        action = false;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + transform.forward * 1.5f).normalized * SPower, ForceMode.Force);
        //�^�O�ύX
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 4f);

        patapataSlider.gameObject.SetActive(false);
    }

    //�ő�̗͂ł̐�����ы���
    public void MostPower()
    {
        HP = 0.0f;
        action = false;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + (transform.forward * 1.5f)).normalized * MPower, ForceMode.Force);
        //�^�O�ύX
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 3f);

        patapataSlider.gameObject.SetActive(false);
    }

    #endregion

    #region �ڐG����
    void OnTriggerEnter(Collider other)
    {
        //�ڐG�����I�u�W�F�N�g�̃^�O��"Player"�̂Ƃ�
        if (other.CompareTag("PlayerBullets"))
        {
            //�I�u�W�F�N�g�̐F��ԂɕύX����
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

        //��~���Ă���G�Ƃ̔���
        if (other.CompareTag("Stop") && death)
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);
            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
        }

        //�n�ʂƂ̔���
        if (other.CompareTag("Ground"))
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);
            Destroy(gameObject);
        }
        //�܂Ƃ̐ڐG����
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
            //�ǉ�
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
    //�n�ʂƂ̔���
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