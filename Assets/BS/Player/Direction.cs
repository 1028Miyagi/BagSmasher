using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour
{
    public GameObject Player;//Player�I�u�W�F�N�g������ϐ�
    public Player D_Script;//PlayerScript������ϐ�
    public float direction;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        D_Script = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = D_Script.xSpeed;

        //����
        if (direction > 0)
        {
            //transform.localScale = new Vector3(1, 1, 1);
            //transform.Rotate(new Vector3(0, 0, 0));
            //transform.localRotation = new Quaternion(0.0f, 45f, 0.0f, 0.0f);
            //this.transform.LookAt(new Vector3(0, 0, 0));
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction < 0)
        {
            //transform.localScale = new Vector3(-1, 1, 1);
            //transform.Rotate(new Vector3(0, 180, 0));
            //transform.localRotation = new Quaternion(0.0f, -45f, 0.0f, 0.0f);
            //this.transform.LookAt(new Vector3(0, 0, 0));
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
