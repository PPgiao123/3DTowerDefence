using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public enum GameMode
    {
        PM_NORMAL,
        PM_INFINITE,
    }

    public TMP_Dropdown mode_dw;
    public TMP_Dropdown coin_dw;
    public TMP_Dropdown emy_grp_dw;
    public Toggle test_toggle;
    int coin;
    public int Coin
    {
        get
        {
            return coin;
        }
        set
        {
            coin = value;
        }
    }
    int enemy_group;
    public int Enemy_Group
    {
        get
        {
            return enemy_group;
        }
        set
        {
            enemy_group = value;
        }
    }
    GameMode game_mode;
    public GameMode Game_Mode
    {
        get
        {
            return game_mode;
        }
        set
        {
            game_mode = value;
        }
    }
    bool network_connected;
    public bool Network_Connected { get { return network_connected; } set { network_connected = value; } }
    bool is_defense;
    public bool Is_Defense { get { return is_defense; } set { is_defense = value; } }

    void Awake()
    {
        instance = this;
        var mode_label = mode_dw.transform.Find("Image").Find("Text");
        mode_label.GetComponent<TMP_Text>().text = "Mode";
        var coin_label = coin_dw.transform.Find("Image").Find("Text");
        coin_label.GetComponent<TMP_Text>().text = "Coin";
        var emy_grp_label = emy_grp_dw.transform.Find("Image").Find("Text");
        emy_grp_label.GetComponent<TMP_Text>().text = "Enemy Group";
        emy_grp_label.GetComponent<TMP_Text>().fontSize = 41f;
    }

    static SettingPanel instance;
    public static SettingPanel Instance
    {
        get
        {
            return instance;
        }
    }
}
