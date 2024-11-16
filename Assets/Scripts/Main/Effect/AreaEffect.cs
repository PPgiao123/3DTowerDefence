using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffect : MonoBehaviour
{
    protected ConcurrentDictionary<Enemy, int> emy_dic = new ConcurrentDictionary<Enemy, int>();

    protected void Update()
    {
        //Ë¢ÐÂµÐÈËÑªÁ¿
        foreach (var e in emy_dic.Keys)
        {
            emy_dic.TryUpdate(e, e.Enemy_Pro.cur_hp, emy_dic[e]);
        }  
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            emy_dic.TryAdd(other.GetComponent<Enemy>(), other.GetComponent<Enemy>().Enemy_Pro.cur_hp);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            emy_dic.TryRemove(other.GetComponent<Enemy>(), out other.GetComponent<Enemy>().Enemy_Pro.cur_hp);
        }
    }
}
