using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ButtonManager;

public class Reminder : MonoBehaviour
{
    TMP_Text txt;
    public ButtonManagerType btn_type;
    string[] str1 = {"Waiting for clients .", "Waiting for clients ..", "Waiting for clients ..."};
    string[] str2 = { "Searching server .", "Searching server ..", "Searching server ..." };

    void Start()
    {
        txt = GetComponentInChildren<TMP_Text>();
        StartCoroutine(update_string());
    }

    IEnumerator update_string()
    {
        string[] tar = str1;
        switch (btn_type)
        {
            case ButtonManagerType.BMT_SERVER:
            case ButtonManagerType.BMT_HOST:
                tar = str1;
                break;
            case ButtonManagerType.BMT_CLIENT:
                tar = str2;
                break;
        }
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForSeconds(0.5f);
            txt.text = tar[i % 3];
        }
    }
}
