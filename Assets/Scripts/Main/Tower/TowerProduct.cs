using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TowerProduct
{
    public enum Type
    {
        None,
        Archer1 = 2000,
        Archer2,
        Archer3,
        Archer4A,
        Archer4B,
        Cannon1 = 2100,
        Cannon2,
        Cannon3,
        Cannon4A,
        Cannon4B,
        Magic1 = 2200,
        Magic2,
        Magic3,
        Magic4A,
        Magic4B,
    }

    public enum AttackType
    {
        None,
        Archer,
        Cannon,
        Magic
    }

    public int id;
    public int atk;
    public int gold;
    public int lvl;
    public float range;
    public int max_lvl;
    public string name;
    public string prefab_path;
    public float fire_speed;
    public float bullet_speed;
    public AttackType atk_type;//ÔÚFCÖÐ¸³Öµ

    public TowerProduct()
    {
        
    }

    public TowerProduct(TowerProduct _pro)
    {
        id = _pro.id;
        atk = _pro.atk;
        gold = _pro.gold;
        lvl = _pro.lvl;
        range = _pro.range;
        max_lvl = _pro.max_lvl;
        name = _pro.name;
        prefab_path = _pro.prefab_path;
        fire_speed = _pro.fire_speed;
        bullet_speed = _pro.bullet_speed;
        atk_type = _pro.atk_type;
    }

    public void init_from_xml(XmlElement xml_ele)
    {
        id = int.Parse(xml_ele.GetAttribute("id"));
        atk = int.Parse(xml_ele.GetAttribute("atk"));
        gold = int.Parse(xml_ele.GetAttribute("gold"));
        lvl = int.Parse(xml_ele.GetAttribute("lvl"));
        range = float.Parse(xml_ele.GetAttribute("range"));
        max_lvl = int.Parse(xml_ele.GetAttribute("max_lvl"));
        name = xml_ele.GetAttribute("name");
        prefab_path = xml_ele.GetAttribute("prefab_path");
        fire_speed = float.Parse(xml_ele.GetAttribute("fire_speed"));
        bullet_speed = float.Parse(xml_ele.GetAttribute("bullet_speed"));
    }
}
