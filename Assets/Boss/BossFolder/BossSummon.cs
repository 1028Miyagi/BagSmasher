using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossSummon : MonoBehaviour
{
    [SerializeField] GameObject[] Enemy = new GameObject[3];

    public GameObject[] summonEffect;

    //����

    void Update()
    {
        GameObject[] walking = GameObject.FindGameObjectsWithTag("kuribo");
        GameObject[] shooting = GameObject.FindGameObjectsWithTag("burosu");
        GameObject[] floating = GameObject.FindGameObjectsWithTag("patapata");
    }

    public void spawn()
    {
        GameObject[] point = GameObject.FindGameObjectsWithTag("summonPoint");
        GameObject summonEnemy;

        summonEnemy = Instantiate(Enemy[Random.Range(0, 5)]); //3��ނ��烉���_���ŏ���
        summonEnemy.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        Instantiate(summonEffect[0], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
    }
}
