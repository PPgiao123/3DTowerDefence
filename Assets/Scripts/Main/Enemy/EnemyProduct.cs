using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EnemyProduct
{
    public enum Type
    {
        FireDragonSmall = 1000,
        FireDragonMid,
        FireDragonBig,
    }

    public int id;
    public float speed;
    public float maxSpeed;
    public float acc;
    public int def;
    public int cur_hp;
    public int maxHp;
    public int gold;
    public string name;
    public string prefab_path;

    public EnemyProduct()
    {
        
    }

    public EnemyProduct(EnemyProduct _pro)
    {
        id = _pro.id;
        speed = _pro.speed;
        maxSpeed = _pro.maxSpeed;
        acc = _pro.acc;
        def = _pro.def;
        cur_hp = _pro.cur_hp;
        maxHp = _pro.maxHp;
        gold = _pro.gold;
        name = _pro.name;
        prefab_path = _pro.prefab_path;
    }

    public void init_from_xml(XmlElement xml_ele)
    {
        id = int.Parse(xml_ele.GetAttribute("id"));
        speed = float.Parse(xml_ele.GetAttribute("speed"));
        maxSpeed = float.Parse(xml_ele.GetAttribute("maxSpeed"));
        acc = float.Parse(xml_ele.GetAttribute("acc"));
        def = int.Parse(xml_ele.GetAttribute("def"));
        maxHp = int.Parse(xml_ele.GetAttribute("maxHp"));
        gold = int.Parse(xml_ele.GetAttribute("gold"));
        name = xml_ele.GetAttribute("name");
        prefab_path = xml_ele.GetAttribute("prefab_path");
        cur_hp = maxHp;
    }
}
