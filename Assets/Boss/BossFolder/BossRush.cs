using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRush : MonoBehaviour
{
    [SerializeField] Boss boss;
    
    //突進時のコライダー　on/off

    void Start()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }

    public void R_ColOn()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }

    public void R_ColOff()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }

    //腕が壁に当たった時

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall") 
        {
            boss.RushStop();
        }
    }
}
