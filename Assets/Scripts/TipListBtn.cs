using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipListBtn : MonoBehaviour
{
    public enum LstBtnType
    {
        LBT_TOWER,
        LBT_ENEMY,
        LBT_SETTING,
    }
    public LstBtnType btn_type;
    public int btn_lvl;
    public TipPanel tip_panel;

    public GameObject next_list;
    public GameObject next_list_title;
    public GameObject next_list_content;
    public GameObject tmp_next_list;
    public GameObject tmp_next_list_title;
    public GameObject tmp_next_list_content;

    bool possess_side_icon;
    Sprite icon_sprite;
    Color icon_color;

    string[] str_lst;
    string[] tower_lst = { "General", "Archer", "Cannon", "Magic" };
    string[] enemy_lst = { "SmallWyvern", "MidWyvern", "BigWyvern" };
    string[] setting_lst = { "Mouse", "Keypad" };

    Dictionary<string, string[]> tower_title_dict = new Dictionary<string, string[]> {
        {"General", new string[]{ "", ""} },
        {"Archer", new string[]{ "General", "Archer-Cannon", "Archer-Magic" } },
        {"Cannon", new string[]{ "General", "Cannon-Archer", "Cannon-Magic" } },
        {"Magic", new string[]{ "General", "Magic-Archer", "Magic-Cannon" } },
        };
    Dictionary<string, string[]> tower_united_keyword_dict = new Dictionary<string, string[]> {
        {"General", new string[]{ "", ""} },
        {"Archer", new string[]{ "Speedy", "Split", "Electric Chain" } },
        {"Cannon", new string[]{ "High Damage", "Repelling Shell", "Lightning Bolt, Thunder Area" } },
        {"Magic", new string[]{ "Consistent", "Slow Down", "Cut Speed" } },
        };
    Dictionary<string, string[]> tower_united_desp_dict = new Dictionary<string, string[]> {
        {"General", new string[]{ AppConst.Tower_General_Tip1, AppConst.Tower_General_Tip2 } },
        {"Archer", new string[]{ AppConst.Archer_General_Tip, AppConst.Archer_Cannon_Tip, AppConst.Archer_Magic_Tip } },
        {"Cannon", new string[]{ AppConst.Cannon_General_Tip, AppConst.Cannon_Archer_Tip, AppConst.Cannon_Magic_Tip } },
        {"Magic", new string[]{ AppConst.Magic_General_Tip, AppConst.Magic_Archer_Tip, AppConst.Magic_Cannon_Tip } },
        };

    Dictionary<string, string[]> enemy_title_dict = new Dictionary<string, string[]> {
        {"SmallWyvern", new string[]{ "General" } },
        {"MidWyvern", new string[]{ "General" } },
        {"BigWyvern", new string[]{ "General" } },
        };
    Dictionary<string, string[]> enemy_keyword_dict = new Dictionary<string, string[]> {
        {"SmallWyvern", new string[]{ ""} },
        {"MidWyvern", new string[]{ ""} },
        {"BigWyvern", new string[]{ ""} },
        };
    Dictionary<string, string[]> enemy_desp_dict = new Dictionary<string, string[]> {
        {"SmallWyvern", new string[]{ AppConst.SmallWyvern_General_Tip } },
        {"MidWyvern", new string[]{ AppConst.MidWyvern_General_Tip } },
        {"BigWyvern", new string[]{ AppConst.BigWyvern_General_Tip } },
        };

    Dictionary<string, string[]> setting_title_dict = new Dictionary<string, string[]> {
        {"Mouse", new string[]{ "Left-Button", "Wheel", "Right-Button" } },
        {"Keypad", new string[]{ "Move", "Rotate" } },
        };
    Dictionary<string, string[]> setting_keyword_dict = new Dictionary<string, string[]> {
        {"Mouse", new string[]{ "", "", "" } },
        {"Keypad", new string[]{ "", "" } },
        };
    Dictionary<string, string[]> setting_desp_dict = new Dictionary<string, string[]> {
        {"Mouse", new string[]{ AppConst.Left_Btn_Tip, AppConst.Wheel_Tip,  AppConst.Right_Btn_Tip } },
        {"Keypad", new string[]{ AppConst.Keypad_Move_Tip, AppConst.Keypad_Rotate_Tip } },
        };

    public void on_click()
    {
        update_panel_info();
        update_next_next_btn_info();
        next_list.gameObject.SetActive(true);
        next_list_title.GetComponentInChildren<Text>().text = GetComponentInChildren<Text>().text;
        switch (btn_type)
        {
            case LstBtnType.LBT_TOWER:
                str_lst = tower_lst;
                possess_side_icon = true;
                icon_color = new Color(1f, 1f, 1f);
                icon_sprite = Resources.Load<Sprite>(AppConst.dots);
                break;
            case LstBtnType.LBT_ENEMY:
                str_lst = enemy_lst;
                possess_side_icon = true;
                icon_color = new Color(1f, 1f, 1f);
                icon_sprite = Resources.Load<Sprite>(AppConst.dots);
                break;
            case LstBtnType.LBT_SETTING:
                str_lst = setting_lst;
                possess_side_icon = true;
                icon_color = new Color(1f, 1f, 1f);
                icon_sprite = Resources.Load<Sprite>(AppConst.dots);
                break;
            default:
                break;
        }

        switch (btn_lvl)
        {
            case 0:
                tip_panel.Btn_Lv0 = this;
                update_btn_lv0();
                break;
            case 1:
                tip_panel.Btn_Lv1 = this;
                update_btn_lv1();
                break;
        }  
    }

    void update_btn_lv0()
    {
        if (tip_panel.list_lv2.activeSelf)
        {
            tip_panel.list_lv2.SetActive(false);
        }
        update_btn_num(str_lst.Length, AppConst.TipListContentBtn_Prefab);
        //刷新
        for (int i = 0; i < next_list_content.transform.childCount; i++)
        {
            var btn = next_list_content.transform.GetChild(i);
            btn.GetComponentInChildren<Text>().text = str_lst[i];
            //btn_lv1有侧面图案才可以点击
            if (possess_side_icon)
            {
                btn.transform.Find("icon").GetComponent<Image>().sprite =
                    Resources.Load<Sprite>(AppConst.button_outlined);
                btn.transform.Find("icon").GetComponent<Image>().color = icon_color;

                btn.transform.Find("icon").Find("Image").GetComponent<Image>().sprite = icon_sprite;
                btn.transform.Find("icon").Find("Image").GetComponent<Image>().color = icon_color;
                btn.transform.parent = next_list_content.transform;
                btn.transform.Find("icon").gameObject.SetActive(true);
                btn.GetComponent<Button>().interactable = true;
            }
            else
            {
                btn.transform.Find("icon").gameObject.SetActive(false);
                btn.GetComponent<Button>().interactable = false;
            }
            btn.GetComponent<TipListBtn>().btn_lvl = btn_lvl + 1;
            btn.GetComponent<TipListBtn>().tip_panel = tip_panel;
            btn.GetComponent<TipListBtn>().next_list = tmp_next_list;
            btn.GetComponent<TipListBtn>().next_list_title = tmp_next_list_title;
            btn.GetComponent<TipListBtn>().next_list_content = tmp_next_list_content;
        }
    }

    void update_btn_lv1()
    {
        tip_panel.content_lv2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        var dic_key = tip_panel.Btn_Lv1.transform.Find("Text").GetComponent<Text>().text;
        switch (tip_panel.Btn_Lv0.btn_type)
        {
            case LstBtnType.LBT_TOWER:
                update_btn_num(tower_title_dict[dic_key].Length, AppConst.TipListContentDesp_Prefab);
                for (int i = 0; i < next_list_content.transform.childCount; i++)
                {
                    var btn = next_list_content.transform.GetChild(i);
                    var txt1 = btn.Find("TitleText").GetComponent<Text>();
                    var txt2 = btn.Find("KeywordText").GetComponent<Text>();
                    var txt3 = btn.Find("Description").GetComponent<Text>();

                    check_string(txt1, tower_title_dict[GetComponentInChildren<Text>().text][i]);
                    check_string(txt2, tower_united_keyword_dict[GetComponentInChildren<Text>().text][i]);
                    check_string(txt3, tower_united_desp_dict[GetComponentInChildren<Text>().text][i]);
                }
                break;
            case LstBtnType.LBT_ENEMY:
                update_btn_num(enemy_title_dict[dic_key].Length, AppConst.TipListContentDesp_Prefab);
                for (int i = 0; i < next_list_content.transform.childCount; i++)
                {
                    var btn = next_list_content.transform.GetChild(i);
                    var txt1 = btn.Find("TitleText").GetComponent<Text>();
                    var txt2 = btn.Find("KeywordText").GetComponent<Text>();
                    var txt3 = btn.Find("Description").GetComponent<Text>();

                    check_string(txt1, enemy_title_dict[GetComponentInChildren<Text>().text][i]);
                    check_string(txt2, enemy_keyword_dict[GetComponentInChildren<Text>().text][i]);
                    check_string(txt3, enemy_desp_dict[GetComponentInChildren<Text>().text][i]);
                }
                break;
            case LstBtnType.LBT_SETTING:
                update_btn_num(setting_title_dict[dic_key].Length, AppConst.TipListContentDesp_Prefab);
                for (int i = 0; i < next_list_content.transform.childCount; i++)
                {
                    var btn = next_list_content.transform.GetChild(i);
                    var txt1 = btn.Find("TitleText").GetComponent<Text>();
                    var txt2 = btn.Find("KeywordText").GetComponent<Text>();
                    var txt3 = btn.Find("Description").GetComponent<Text>();

                    check_string(txt1, setting_title_dict[GetComponentInChildren<Text>().text][i]);
                    check_string(txt2, setting_keyword_dict[GetComponentInChildren<Text>().text][i]);
                    check_string(txt3, setting_desp_dict[GetComponentInChildren<Text>().text][i]);
                }
                break;
        }
    }

    void update_btn_num(int max_num, string prefab_path)
    {
        //补足个数
        if (next_list_content.transform.childCount <= max_num)
        {
            int num = max_num - next_list_content.transform.childCount;
            if (num != 0)
            {
                for (int i = 0; i < num; i++)
                {
                    var btn = (GameObject)GameObject.Instantiate(Resources.Load(prefab_path));
                    btn.transform.parent = next_list_content.transform;
                }
            }
        }
        //删除多余
        else if (next_list_content.transform.childCount > max_num)
        {
            int num = next_list_content.transform.childCount - max_num;
            for (int i = 0; i < num; i++)
            {
                DestroyImmediate(next_list_content.transform.GetChild(i).gameObject);
            }
        }
    }

    void update_panel_info()
    {
        switch (btn_lvl)
        {
            case 0:
                if (this == tip_panel.Btn_Lv0)
                {
                    this.GetComponent<Button>().interactable = false;
                    tip_panel.Btn_Lv0.GetComponent<Button>().interactable = true;
                    tip_panel.Btn_Lv0 = this;
                }
                break;
            case 1:
                if (this == tip_panel.Btn_Lv1)
                {
                    this.GetComponent<Button>().interactable = false;
                    tip_panel.Btn_Lv1.GetComponent<Button>().interactable = true;
                    tip_panel.Btn_Lv1 = this;
                }
                break;
            case 2:
                if (this == tip_panel.Btn_Lv2)
                {
                    this.GetComponent<Button>().interactable = false;
                    tip_panel.Btn_Lv2.GetComponent<Button>().interactable = true;
                    tip_panel.Btn_Lv2 = this;
                }
                break;
        }
    }

    //给下下级的button赋值
    void update_next_next_btn_info()
    {
        switch (btn_lvl)
        {
            case 0:
                tmp_next_list = tip_panel.list_lv2;
                tmp_next_list_title = tip_panel.title_lv2;
                tmp_next_list_content = tip_panel.content_lv2;
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

    void check_string(Text txt, string text)
    {
        if (txt)
        {
            if (text == "")
            {
                txt.gameObject.SetActive(false);
            }
            else
            {
                txt.text = text;
                txt.gameObject.SetActive(true);
            }
        }
    }
}
