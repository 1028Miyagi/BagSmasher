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
    //�G�̗�
    float maxHP = 10;
    public float HP;
    private Animator animator;
    //�e�t���O
    public bool action = true;
    public bool right = true;
    public bool death = false;
    //�G��~����
    public float coolTime = 3.0f;

    public GameObject[] particleObject;
    GameObject Obj;

    //��΂��͂Ƌ���
    float FPower = 8000;
    float SPower = 15000;
    float MPower = 50000;


    [Header("����")] public AudioSource audioSource;
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
        //�G�ƃv���C���[�̋�����
        float Diference;

        stopEnemySlider.value = HP / maxHP;

        Diference = Player.transform.position.x - Stop.transform.position.x;
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

        //�e�������������̒�~����
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

    #region ������΂�����

    //��ԒႢ�Ԃł̐�����ы���
    public void FirstPower()
    {
        HP = 0.0f;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + transform.forward * 2).normalized * FPower, ForceMode.Force);
        //�^�O�ύX
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 3f);

        stopEnemySlider.gameObject.SetActive(false);
    }

    //�^�񒆂̗͂ł̐�����ы���
    public void SecondPower()
    {
        HP = 0.0f;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + transform.forward * 1.5f).normalized * SPower, ForceMode.Force);
        //�^�O�ύX
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 5f);

        stopEnemySlider.gameObject.SetActive(false);
    }

    //�ő�̗͂ł̐�����ы���
    public void MostPower()
    {
        HP = 0.0f;
        animator.enabled = false;
        rigidbody.AddForce((transform.up + (transform.forward * 1.5f)).normalized * MPower, ForceMode.Force);
        //�^�O�ύX
        gameObject.tag = "Attacktag";
        death = true;
        Destroy(this.gameObject, 10f);

        stopEnemySlider.gameObject.SetActive(false);
    }

    #endregion

    #region �ڐG����
    void OnTriggerEnter(Collider other)
    {
        //�ڐG�����I�u�W�F�N�g�̃^�O��"Player"�̂Ƃ��̔���
        if (other.CompareTag("PlayerBullets"))
        {
            //�I�u�W�F�N�g�̐F��ԂɕύX����
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
        //������񂾓G�Ƃ̔���
        if (other.CompareTag("Attacktag"))
        {
            Instantiate(particleObject[2], this.transform.position, Quaternion.identity);
            Instantiate(particleObject[3], this.transform.position, Quaternion.identity);

            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(sound);

            Destroy(gameObject);
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
        //�܂Ƃ̐ڐG����
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

        if (other.CompareTag("boss")) //�ǉ��_
        {
            //�ǉ�
            stopEnemySlider.gameObject.SetActive(false);
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