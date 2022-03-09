using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagopCollider : MonoBehaviour
{
    [SerializeField] Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "burosu")
        {
            player.BagopHit();
            print("Inburpsu");
        }
        else if (other.tag == "kuribo")
        {
            player.BagopHit();
            print("Inkuribo");
        }
        else if (other.tag == "Chase")
        {
            player.BagopHit();
            print("InChaise");
        }
        else if (other.tag == "patapata")
        {
            player.BagopHit();
            print("Inpatapata");
        }
        else if(other.tag == "hunmer")
        {
            player.BagopHit();
            print("tane");
        }
        else if (other.tag == "Stop")
        {
            player.BagopHit();
            print("Stop");
        }
    }
}
