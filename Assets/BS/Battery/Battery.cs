using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public float startTime;
    public float time;
    public GameObject tane;
    public GameObject[] particleObject;

    // Start is called before the first frame update
    void Start()
    {
        time = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            Instantiate(tane, transform.position+Vector3.forward, transform.rotation);

            Instantiate(particleObject[0], transform.position + Vector3.forward, Quaternion.identity);
            time = startTime;
        }
    }
}
