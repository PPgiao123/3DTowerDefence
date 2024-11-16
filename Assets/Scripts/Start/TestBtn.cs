using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBtn : MonoBehaviour
{
    Text test_txt;

    void Start()
    {
        test_txt = GetComponentInChildren<Text>();
        test_txt.text = "Test";
    }

    public void on_click()
    {
        GetComponent<Toggle>().isOn = GetComponent<Toggle>().isOn ? false : true;
    }
}
