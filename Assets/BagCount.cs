using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagCount : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject bag1;
    [SerializeField] GameObject bag2;
    [SerializeField] GameObject bag3;

    //écíeêîÇÃï\é¶

    void Update()
    {
        if (player.Bag_in_enemy == 0)
        {
            bag1.SetActive(false);
            bag2.SetActive(false);
            bag3.SetActive(false);
        }
        if (player.Bag_in_enemy == 1)
        {
            bag1.SetActive(true);
            bag2.SetActive(false);
            bag3.SetActive(false);
        }
        if (player.Bag_in_enemy == 2)
        {
            bag1.SetActive(true);
            bag2.SetActive(true);
            bag3.SetActive(false);
        }
        if (player.Bag_in_enemy == 3)
        {
            bag1.SetActive(true);
            bag2.SetActive(true);
            bag3.SetActive(true);
        }
    }
}
