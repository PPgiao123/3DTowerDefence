using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBallExplosion : AreaEffect
{
    private void Start()
    {
        StartCoroutine(check_enemy());
    }

    IEnumerator check_enemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (emy_dic.Count > 0)
            {
                //ElectricGround也有扣血效果，会造成血条混乱
                foreach (var e in emy_dic.Keys)
                {
                    e.Enemy_Pro.speed = 5;
                }
            }
        }
    }
}
