using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_D : MonoBehaviour
{
    public GameObject[] particleObject;
    GameObject Obj;

    // Start is called before the first frame update
    void Start()
    {
        Obj = Instantiate(particleObject[0], this.transform.position, Quaternion.identity);
        Obj.transform.parent = this.gameObject.transform;
        Obj = Instantiate(particleObject[1], this.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        Obj.transform.position = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            Destroy(gameObject);
        }
        else if(other.tag == "boss")
        {
            Destroy(gameObject);
        }
        if (other.tag == "burosu")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "kuribo")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "Chase")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "patapata")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "hunmer")
        {
            Destroy(gameObject);
        }
        else if (other.tag == "Stop")
        {
            Destroy(gameObject);
        }
    }
}
