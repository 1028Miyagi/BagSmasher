using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Player player;

    void HitEnemy()
    {
        player.HitEnemy();
    }

    void Test()
    {
        player.F_ShotA();
    }
}
