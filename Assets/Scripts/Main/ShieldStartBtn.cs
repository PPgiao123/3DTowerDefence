using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldStartBtn : MonoBehaviour
{
    public AudioClip audio_clip;
    

    void Awake()
    {
        
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void on_click()
    {
        AudioSource.PlayClipAtPoint(audio_clip, Camera.main.transform.position);
        GameControl.Instance.game_start_func();//µ÷ÓÃº¯Êý
        for (int i = 0; i < GameControl.Instance.start_btn_clection.transform.childCount; i++)
        {
            GameControl.Instance.start_btn_clection.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
