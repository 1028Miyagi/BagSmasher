using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spwan : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject[] enemy = new GameObject[3];

    [SerializeField] float spawnTime = 5f;
    [SerializeField] float s_coolTime;

    //ボスステージのスポナー

    void Start()
    {
        s_coolTime = spawnTime;
    }

    void Update()
    {
        GameObject[] walkEnemy = GameObject.FindGameObjectsWithTag("kuribo");
        GameObject[] shootEnemy = GameObject.FindGameObjectsWithTag("burosu");
        GameObject[] flyEnemy = GameObject.FindGameObjectsWithTag("patapata");

        if (player.transform.position.x >= 513 && transform.position.x <= 569)
        {
            s_coolTime -= Time.deltaTime;

            if (s_coolTime <= 0.0f)
            {
                Spawn_Enemy();
                s_coolTime = spawnTime;
            }
        }

    }

    void Spawn_Enemy()
    {
        GameObject[] spawnPoint = GameObject.FindGameObjectsWithTag("spawnPoint");
        GameObject spawnEnemy;

        spawnEnemy = Instantiate(enemy[Random.Range(0, 2)]);
        spawnEnemy.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}
