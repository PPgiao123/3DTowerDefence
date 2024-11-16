using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public ButtonManagerType btn_type;

    public Reminder reminder;

    public enum ButtonManagerType
    {
        BMT_NONE,
        BMT_BACK,
        BMT_SINGLE_PLAYER,
        BMT_MULTI_PLAYER,
        BMT_SERVER,
        BMT_CLIENT,
        BMT_HOST,
        BMT_DEFENSE,
        BMT_ATTACK,
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (reminder && reminder.gameObject.activeSelf)
            {
                return;
            }
            btn_func();
        });
    }

    public void btn_func()
    {
        switch (btn_type)
        {
            case ButtonManagerType.BMT_BACK:
                load_start_scene();
                break;
            case ButtonManagerType.BMT_SINGLE_PLAYER:
                SettingPanel.Instance.Network_Connected = false;
                load_main_scene();
                break;
            case ButtonManagerType.BMT_MULTI_PLAYER:
                SettingPanel.Instance.Network_Connected = true;
                load_server_scene();
                break;
            case ButtonManagerType.BMT_SERVER:
                server_btn_react();
                NetworkManager.Singleton.StartServer();
                
                break;
            case ButtonManagerType.BMT_CLIENT:
                server_btn_react();
                NetworkManager.Singleton.StartClient();
                break;
            case ButtonManagerType.BMT_HOST:
                server_btn_react();
                NetworkManager.Singleton.StartHost();
                
                break;
            case ButtonManagerType.BMT_DEFENSE:
                SettingPanel.Instance.Is_Defense = true;
                break;
            case ButtonManagerType.BMT_ATTACK:
                SettingPanel.Instance.Is_Defense = false;
                break;
        }
    }

    private void load_start_scene()
    {
        SceneManager.LoadScene("Start");
    }

    private void load_main_scene()
    {
        //Mode
        var mode_dw = SettingPanel.Instance.mode_dw;
        switch (mode_dw.value)
        {
            case 0:
                SettingPanel.Instance.Game_Mode = SettingPanel.GameMode.PM_NORMAL;
                break;
            case 1:
                SettingPanel.Instance.Game_Mode = SettingPanel.GameMode.PM_INFINITE;
                break;
        }
        //Coin
        var coin_dw = SettingPanel.Instance.coin_dw;
        SettingPanel.Instance.Coin = int.Parse(coin_dw.options[coin_dw.value].text);
        //Group
        if (SettingPanel.Instance.Game_Mode == SettingPanel.GameMode.PM_NORMAL)
        {
            var emy_grp_dw = SettingPanel.Instance.emy_grp_dw;
            SettingPanel.Instance.Enemy_Group = int.Parse(emy_grp_dw.options[emy_grp_dw.value].text);
        }
        
        SceneManager.LoadScene("Main");
    }

    private void load_server_scene()
    {
        
        SceneManager.LoadScene("Server");
    }

    private void server_btn_react()
    {
        reminder.gameObject.SetActive(true);
        reminder.btn_type = btn_type;
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }
}
