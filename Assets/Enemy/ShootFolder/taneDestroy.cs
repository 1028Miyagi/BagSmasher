using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taneDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            Destroy(gameObject);
            //print("�n��");
        }
        else if (other.tag == "Player")
        {
            Destroy(gameObject);
            //print("�v���C���[");
        }
        else if (other.tag == "Bag")
        {
            Destroy(gameObject);
            //print("��");
        }
    }
}
