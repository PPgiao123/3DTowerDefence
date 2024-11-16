using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : MonoBehaviour
{
    TipListBtn btn_lv0;
    public TipListBtn Btn_Lv0 { get { return btn_lv0; } set { btn_lv0 = value; } }
    TipListBtn btn_lv1;
    public TipListBtn Btn_Lv1 { get { return btn_lv1; } set { btn_lv1 = value; } }
    TipListBtn btn_lv2;
    public TipListBtn Btn_Lv2 { get { return btn_lv2; } set { btn_lv2 = value; } }

    public GameObject list_lv0;
    public GameObject list_lv1;
    public GameObject list_lv2;
    public GameObject title_lv0;
    public GameObject title_lv1;
    public GameObject title_lv2;
    public GameObject content_lv0;
    public GameObject content_lv1;
    public GameObject content_lv2;

    private void Start()
    {
        this.gameObject.SetActive(false);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        Camera.main.GetComponent<CameraControl>().Is_Pause_Active = false;
    }

    private void OnDisable()
    {
        if (Camera.main.GetComponent<CameraControl>())
        {
            Camera.main.GetComponent<CameraControl>().Is_Pause_Active = true;
        }
    }
}
