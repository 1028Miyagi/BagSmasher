using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagJudge : MonoBehaviour
{
    public BoxCollider _collider;
    public GameObject Bag;

    // Start is called before the first frame update
    void Start()
    {
        _collider = Bag.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _collider.enabled = true;
        }
        else
        {
            _collider.enabled = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag( "kuribo"))
        {
            Debug.Log("kuribo");
        }
        if (other.CompareTag("burosu"))
        {
            Debug.Log("burosu");
        }
        if(other.CompareTag("patapata"))
        {
            Debug.Log("patapata");
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
