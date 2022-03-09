using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegCol : MonoBehaviour
{
    [SerializeField] GameObject bossObject;

    //ï«ÇÃÇ∑ÇËî≤ÇØëŒçÙ

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (bossObject.transform.position.x < 517)
            {
                bossObject.transform.position -= bossObject.transform.forward * Time.deltaTime * 20;
            }
            else if (bossObject.transform.position.x > 551)
            {
                bossObject.transform.position += bossObject.transform.forward * Time.deltaTime * 20;
            }
        }
    }
}
