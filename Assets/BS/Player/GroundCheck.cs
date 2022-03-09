using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    private string groundTag = "Ground";
    private bool isGround = false;
    private bool isGroundEnter, isGroundStay, isGroundExit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    //ê⁄ínîªíË
    public bool IsGround()
    {
        if (isGroundEnter || isGroundStay)
        {
            isGround = true;
        }
        else if (isGroundExit)
        {
            isGround = false;
        }

        isGroundEnter = false;
        isGroundStay = false;
        isGroundExit = false;
        return isGround;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == groundTag)
        {
            isGroundEnter = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == groundTag)
        {
            isGroundStay = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == groundTag)
        {
            isGroundExit = true;
        }
    }

}
