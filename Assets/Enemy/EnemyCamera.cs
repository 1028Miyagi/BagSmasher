using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamera : MonoBehaviour
{
    //HPバーを常にカメラの方に向けておく

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}