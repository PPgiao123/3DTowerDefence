using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.UI;
using static Tower;

public class ChooseBtn : MonoBehaviour
{
    TowerProduct.Type type;
    public TowerProduct.Type Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
        }
    }
    TowerProduct.AttackType united_type;
    public TowerProduct.AttackType United_Type
    {
        get
        {
            return united_type;
        }
        set
        {
            united_type = value;
        }
    }
    int united_price;
    public int United_Price
    {
        get
        {
            return united_price;
        }
        set
        {
            united_price = value;
        }
    }

    int price;

    void Start()
    {
        price = TowerFC.Instance.get_tower_price(type);
    }

    public void create_tower()
    {
        if (GameControl.Cur_Gold < price)
        {
            GameControl.Instance.help_txt.color = Color.red;
            GameControl.Instance.help_txt.text = "Insufficient Money!";
            GameControl.Instance.help_txt.gameObject.SetActive(true);
            Invoke("update_help_txt", 2);
            return;
        }

        if (GameControl.cur_tower_btn.cur_tower)
        {
            if (GameControl.Instance.network_connected)
            {
                if (GameControl.cur_tower_btn.cur_tower.united_obj)
                {
                    GameControl.Instance.net_destroy_united_obj();
                }
                GameControl.Instance.net_destroy_tower();
            }
            else
            {
                if (GameControl.cur_tower_btn.cur_tower.united_obj)
                {
                    Destroy(GameControl.cur_tower_btn.cur_tower.united_obj.gameObject);
                }
                Destroy(GameControl.cur_tower_btn.cur_tower.gameObject);
            }
            
            GameControl.Instance.cal_tower_num_by_type(GameControl.cur_tower_btn.cur_tower, false);
        }
        GameControl.Instance.update_gold(-price);

        var tower_pro = TowerFC.Instance.create_tower(type);
        if (GameControl.Instance.network_connected)
        {
            GameControl.Instance.net_create_tower((int)type, GameControl.cur_tower_btn.sequence);
        }
        else
        {
            var tower = GameObject.Instantiate<Tower>(Resources.Load<Tower>(tower_pro.prefab_path));//预制体
            tower.init(tower_pro);//属性
            tower.transform.position = GameControl.cur_tower_btn.transform.position;//位置
            tower.draw_range();
            GameControl.cur_tower_btn.cur_tower = tower;
            GameControl.Instance.cal_tower_num_by_type(tower, true);
        }

        GameControl.Instance.choose_panel.gameObject.SetActive(false);
    }

    public static void sale_tower()
    {
        var tar_tower = GameControl.cur_tower_btn.cur_tower;
        var sale_price = TowerFC.Instance.get_tower_sale_price(tar_tower.tower_pro);
        if (!tar_tower)
        {
            return;
        }
        GameControl.Instance.cal_tower_num_by_type(tar_tower, false);

        if (GameControl.Instance.network_connected)
        {
            GameControl.Instance.net_destroy_united_obj();
            GameControl.Instance.net_destroy_lightning();
            GameControl.Instance.net_destroy_tower();
        }
        else
        {
            Destroy(tar_tower.gameObject);
            Destroy(tar_tower.united_obj.gameObject);
            if (tar_tower.tower_pro.atk_type == TowerProduct.AttackType.Magic &&
                tar_tower.GetComponent<MagicTower>().Lightning)
            {
                Destroy(tar_tower.GetComponent<MagicTower>().Lightning.gameObject);
            }
        }
        

        GameControl.Instance.update_gold(+sale_price);
        GameControl.Instance.choose_panel.gameObject.SetActive(false);
    }

    public void united_tower()
    {
        var tar_tower = GameControl.cur_tower_btn.cur_tower;
        tar_tower.Is_United = true;
        if (tar_tower.united_obj)
        {
            if (GameControl.Instance.network_connected)
            {
                GameControl.Instance.net_destroy_united_obj();
            }
            else
            {
                Destroy(tar_tower.united_obj.gameObject);
            }
        }

        switch (united_type)
        {
            case TowerProduct.AttackType.Archer:
                create_united_obj(AppConst.United_Bow);
                switch (tar_tower.tower_pro.atk_type)
                {
                    //炮+弓
                    case TowerProduct.AttackType.Cannon:
                        tar_tower.United_Type = Tower.UnitedType.UT_CANNON_ARCHER;
                        if (TowerFC.Instance.get_type_from_id(tar_tower.tower_pro.id)
                            == TowerProduct.Type.Cannon4A)
                        {
                            tar_tower.tower_pro.fire_speed =
                            TowerFC.Instance.create_tower(TowerProduct.Type.Archer4B).fire_speed;
                        }
                        else if (TowerFC.Instance.get_type_from_id(tar_tower.tower_pro.id)
                            == TowerProduct.Type.Cannon4B)
                        {
                            tar_tower.tower_pro.fire_speed =
                            TowerFC.Instance.create_tower(TowerProduct.Type.Cannon1).fire_speed;
                        }
                        break;
                    //电+弓
                    case TowerProduct.AttackType.Magic:
                        tar_tower.United_Type = Tower.UnitedType.UT_MAGIC_ARCHER;
                        tar_tower.tower_pro.fire_speed =
                            TowerFC.Instance.create_tower(TowerProduct.Type.Archer3).fire_speed;
                        if (tar_tower.GetComponent<MagicTower>().Lightning)
                        {
                            Destroy(tar_tower.GetComponent<MagicTower>().Lightning.gameObject);
                        }
                        break;
                }
                break;
            case TowerProduct.AttackType.Cannon:
                create_united_obj(AppConst.United_Canon);
                switch (tar_tower.tower_pro.atk_type)
                {
                    //弓+炮
                    case TowerProduct.AttackType.Archer:
                        tar_tower.United_Type = Tower.UnitedType.UT_ARCHER_CANNON;
                        tar_tower.tower_pro.fire_speed =
                            TowerFC.Instance.create_tower(TowerProduct.Type.Cannon1).fire_speed;
                        break;
                    //电+炮
                    case TowerProduct.AttackType.Magic:
                        tar_tower.United_Type = Tower.UnitedType.UT_MAGIC_CANNON;
                        tar_tower.tower_pro.fire_speed =
                            TowerFC.Instance.create_tower(TowerProduct.Type.Cannon4B).fire_speed;
                        tar_tower.tower_pro.atk =
                            TowerFC.Instance.create_tower(TowerProduct.Type.Cannon4A).atk;
                        if (tar_tower.GetComponent<MagicTower>().Lightning)
                        {
                            if (GameControl.Instance.network_connected)
                            {
                                GameControl.Instance.net_destroy_lightning();
                            }
                            else
                            {
                                Destroy(tar_tower.GetComponent<MagicTower>().Lightning.gameObject);
                            }
                        }
                        break;
                }
                break;
            case TowerProduct.AttackType.Magic:
                create_united_obj(AppConst.United_Magic);
                switch (tar_tower.tower_pro.atk_type)
                {
                    //弓+电
                    case TowerProduct.AttackType.Archer:
                        tar_tower.United_Type = Tower.UnitedType.UT_ARCHER_MAGIC;
                        tar_tower.tower_pro.fire_speed =
                            TowerFC.Instance.create_tower(TowerProduct.Type.Archer1).fire_speed;
                        break;
                    //炮+电
                    case TowerProduct.AttackType.Cannon:
                        tar_tower.United_Type = Tower.UnitedType.UT_CANNON_MAGIC;
                        break;
                }
                break;
        }

        GameControl.Instance.update_tower_united_type((int)tar_tower.United_Type);
        GameControl.Instance.update_tower_fire_speed(tar_tower.tower_pro.fire_speed);
        GameControl.Instance.update_tower_atk(tar_tower.tower_pro.atk);

        GameControl.Instance.update_gold(-united_price);
        GameControl.Instance.choose_panel.gameObject.SetActive(false);
    }

    void create_united_obj(string path)
    {
        if (GameControl.Instance.network_connected)
        {
            GameControl.Instance.net_create_united_obj(path);
        }
        else
        {
            var tar_tower = GameControl.cur_tower_btn.cur_tower;
            var obj = (GameObject)Instantiate(Resources.Load(path));
            tar_tower.united_obj = obj;
            tar_tower.united_obj.transform.position = tar_tower.united_pos.position;
        }
    }

    public void update_help_txt()
    {
        GameControl.Instance.help_txt.text = "";
        GameControl.Instance.help_txt.gameObject.SetActive(false);
    }
}
