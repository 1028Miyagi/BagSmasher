using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEbter(Collider other)
    {
        if (other.tag == "Cube")
        {
            Debug.Log("Get");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        //�ڐG�����I�u�W�F�N�g�̃^�O��"Player"�̂Ƃ�
        if (other.CompareTag("Bag"))
        {
            //�I�u�W�F�N�g�̐F��ԂɕύX����
            GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("Cube");
        }
    }
}
