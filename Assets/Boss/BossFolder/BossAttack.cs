using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    //�ʏ�U���̃R���C�_�[ on/off

    void Start()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }

    public void ColOn()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }

    public void ColOff()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }
}
