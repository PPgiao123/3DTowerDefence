using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndControl : MonoBehaviour
{
    public GameObject victory_pnl;
    public GameObject failure_pnl;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (GameControl.Instance.Is_Win)
        {
            victory_pnl.gameObject.SetActive(true);
        }
        else
        {
            failure_pnl.gameObject.SetActive(true);
        }
    }

    private static EndControl instance;
    public static EndControl Instance { get { return instance; } }
}
