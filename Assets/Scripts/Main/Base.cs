using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    TMP_Text heart_txt;
    
    private void Awake()
    {
        heart_txt = GameControl.Instance.player_panel.transform.Find("HeartTxt").GetComponent<TMP_Text>();
        heart_txt.text = "x " + GameControl.Instance.cur_heart;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            GameControl.Instance.remove_enemy(other.transform.GetComponent<Enemy>());
            Destroy(other.gameObject);
            GameControl.Instance.update_heart(-1);
        }
    }


}
