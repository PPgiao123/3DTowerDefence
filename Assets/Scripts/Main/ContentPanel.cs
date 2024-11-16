using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ContentPanel : MonoBehaviour
{
    int target_lvl;
    public List<TowerProduct.Type> display_tower_list = new List<TowerProduct.Type>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    //func可以在外面调用
    private ChooseBtn create_choose_btn(TowerProduct.Type _type = TowerProduct.Type.None, 
        Sprite sprite = null, string top_descrip = null, 
        string bottom_descrip = null, UnityAction func = null)
    {
        ChooseBtn chos_btn = GameObject.Instantiate<ChooseBtn>(Resources.Load<ChooseBtn>(AppConst.ChooseBtn_Prefab));
        switch (_type)
        {
            case TowerProduct.Type.Archer1:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Archer_lv1);
                break;
            case TowerProduct.Type.Archer2:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Archer_lv2);
                break;
            case TowerProduct.Type.Archer3:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Archer_lv3);
                break;
            case TowerProduct.Type.Archer4A:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Archer_lv4A);
                break;
            case TowerProduct.Type.Archer4B:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Archer_lv4B);
                break;
            case TowerProduct.Type.Cannon1:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Canon_lv1);
                break;
            case TowerProduct.Type.Cannon2:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Canon_lv2);
                break;
            case TowerProduct.Type.Cannon3:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Canon_lv3);
                break;
            case TowerProduct.Type.Cannon4A:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Canon_lv4A);
                break;
            case TowerProduct.Type.Cannon4B:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Canon_lv4B);
                break;
            case TowerProduct.Type.Magic1:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Magic_lv1);
                break;
            case TowerProduct.Type.Magic2:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Magic_lv2);
                break;
            case TowerProduct.Type.Magic3:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Magic_lv3);
                break;
            case TowerProduct.Type.Magic4A:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Magic_lv4A);
                break;
            case TowerProduct.Type.Magic4B:
                chos_btn.GetComponent<Image>().sprite = Resources.Load<Sprite>(AppConst.Magic_lv4B);
                break;
            default:
                chos_btn.GetComponent<Image>().sprite = sprite;
                break;
        }

        if (_type != TowerProduct.Type.None)
        {
            chos_btn.transform.Find("Price_Text").GetComponent<TMP_Text>().text =
                    TowerFC.Instance.get_tower_price(_type) + "$";
            chos_btn.transform.Find("Description_Text").GetComponent<TMP_Text>().text =
                TowerFC.Instance.get_tower_name(_type);
            chos_btn.transform.parent = this.transform;
            chos_btn.Type = _type;
            chos_btn.GetComponent<Button>().onClick.AddListener(chos_btn.create_tower);
        }
        else
        {
            chos_btn.transform.Find("Price_Text").GetComponent<TMP_Text>().text = bottom_descrip;
            chos_btn.transform.Find("Description_Text").GetComponent<TMP_Text>().text = top_descrip;
            chos_btn.transform.parent = this.transform;
            if (func != null)
            {
                chos_btn.GetComponent<Button>().onClick.AddListener(func);
            }
        }

        return chos_btn;
    }

    public void update_display_tower(TowerProduct tw_pro = null)
    {
        display_tower_list.Clear();
        var cur_lvl = tw_pro == null ? 0 : tw_pro.lvl;
        target_lvl = cur_lvl + 1;
        
        if (tw_pro == null)
        {
            //显示全部lvl 1
            foreach (var type in add_other_lvl1_tower(TowerProduct.AttackType.None, true))
            {
                display_tower_list.Add(type);
            }
        }
        else
        {
            if (target_lvl < 4)//cur_lv 1,2
            {
                //显示其他lvl 1和 下一等级的同类型
                display_tower_list.Add(TowerFC.Instance.get_type_from_id(tw_pro.id + 1));

                foreach (var type in add_other_lvl1_tower(tw_pro.atk_type, false))
                {
                    display_tower_list.Add(type);
                }
            }
            else if (target_lvl == 4)//cur_lv 3
            {
                //显示其他lvl 1 和 下一等级的同类型的A和B
                display_tower_list.Add(TowerFC.Instance.get_type_from_id(tw_pro.id + 1));//4A
                display_tower_list.Add(TowerFC.Instance.get_type_from_id(tw_pro.id + 2));//4B

                foreach (var type in add_other_lvl1_tower(tw_pro.atk_type, false))
                {
                    display_tower_list.Add(type);
                }
            }
            else if (target_lvl == 5)//cur_lv 4
            {
                //显示全部lvl 1
                foreach (var type in add_other_lvl1_tower(tw_pro.atk_type, false))
                {
                    display_tower_list.Add(type);
                }
            }
        }

        foreach (var e in this.GetComponentsInChildren<ChooseBtn>())
        {
            Destroy(e.gameObject);
        }
        foreach (var type in display_tower_list)
        {
            create_choose_btn(type);
        }
        
        if (tw_pro != null)
        {
            //多塔联动 增加其他联合塔的图标(其他顶级塔的8折)
            if (cur_lvl == 4)
            {
                string sprite_path = null;
                int united_price = 0;
                foreach (var type in TowerFC.Instance.get_other_tower_atk_type(tw_pro))
                {
                    switch (type)
                    {
                        case TowerProduct.AttackType.Archer:
                            sprite_path = AppConst.Bow;
                            united_price = TowerFC.Instance.
                                get_tower_sale_price(TowerFC.Instance.create_tower(TowerProduct.Type.Archer4A));
                            break;
                        case TowerProduct.AttackType.Cannon:
                            sprite_path = AppConst.Bomb;
                            united_price = TowerFC.Instance.
                                get_tower_sale_price(TowerFC.Instance.create_tower(TowerProduct.Type.Cannon4A));
                            break;
                        case TowerProduct.AttackType.Magic:
                            sprite_path = AppConst.Lightning;
                            united_price = TowerFC.Instance.
                                get_tower_sale_price(TowerFC.Instance.create_tower(TowerProduct.Type.Magic4A));
                            break;
                    }
                    var chos_btn = create_choose_btn(TowerProduct.Type.None, Resources.Load<Sprite>(sprite_path),
                        "United", united_price + "$", null);
                    chos_btn.United_Type = type;
                    chos_btn.United_Price = united_price;
                    chos_btn.GetComponent<Button>().onClick.AddListener(chos_btn.united_tower);
                }
            }
            //增加出售图标
            create_choose_btn(TowerProduct.Type.None, Resources.Load<Sprite>(AppConst.Coin),
                TowerFC.Instance.get_tower_sale_price(tw_pro) + "$", "SALE", ChooseBtn.sale_tower);
        }
    }

    public List<TowerProduct.Type> add_other_lvl1_tower(TowerProduct.AttackType type, 
        bool need_all = false)
    {
        List<TowerProduct.Type> tg_list = new List<TowerProduct.Type>();
        if (need_all)
        {
            tg_list.Add(TowerProduct.Type.Archer1);
            tg_list.Add(TowerProduct.Type.Cannon1);
            tg_list.Add(TowerProduct.Type.Magic1);
        }
        else
        {
            switch (type)
            {
                case TowerProduct.AttackType.Archer:
                    tg_list.Add(TowerProduct.Type.Cannon1);
                    tg_list.Add(TowerProduct.Type.Magic1);
                    break;
                case TowerProduct.AttackType.Cannon:
                    tg_list.Add(TowerProduct.Type.Archer1);
                    tg_list.Add(TowerProduct.Type.Magic1);
                    break;
                case TowerProduct.AttackType.Magic:
                    tg_list.Add(TowerProduct.Type.Archer1);
                    tg_list.Add(TowerProduct.Type.Cannon1);
                    break;
            }
        }
        return tg_list;
    }

    #region 单例
    private static ContentPanel instance;
    public static ContentPanel Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion
}
