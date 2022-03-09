using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery_tane : MonoBehaviour
{
    public GameObject Battery;
    Rigidbody myRigidbody;

    public Vector3 bulletForce;
    public float bulletpower;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        bulletForce = transform.forward * bulletpower;
        myRigidbody.velocity = bulletForce;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            Destroy(gameObject);
            //print("地面");
        }
        else if (other.tag == "Player")
        {
            Destroy(gameObject);
            //print("プレイヤー");
        }
        else if (other.tag == "Bag")
        {
            Destroy(gameObject);
            //print("袋");
        }
    }
}
