using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricGround : AreaEffect
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
                foreach (var e in emy_dic.Keys)
                {
                    if (e.Enemy_Pro.cur_hp <= 10)
                    {
                        emy_dic.TryRemove(e, out e.Enemy_Pro.cur_hp);
                    }
                    if (e != null)
                    {
                        e.get_hurt(10);
                    }
                }
            }
        }
    }
}
