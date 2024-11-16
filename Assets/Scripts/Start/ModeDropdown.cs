using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModeDropdown : MonoBehaviour
{
    public SettingPanel setng_pnl;

    public void on_click()
    {
        if (setng_pnl.mode_dw.value == 0)
        {
            setng_pnl.emy_grp_dw.gameObject.SetActive(true);
        }
        else if (setng_pnl.mode_dw.value == 1)
        {
            setng_pnl.emy_grp_dw.gameObject.SetActive(false);
        }
    }
}
