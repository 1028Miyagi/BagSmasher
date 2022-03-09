using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamera : MonoBehaviour
{
    //HPƒo[‚ğí‚ÉƒJƒƒ‰‚Ì•û‚ÉŒü‚¯‚Ä‚¨‚­

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}