using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamera : MonoBehaviour
{
    //HP�o�[����ɃJ�����̕��Ɍ����Ă���

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}