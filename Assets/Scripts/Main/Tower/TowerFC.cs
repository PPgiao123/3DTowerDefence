using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TowerFC : MonoBehaviour
{
    Dictionary<TowerProduct.Type, TowerProduct> tower_lib = new Dictionary<TowerProduct.Type, TowerProduct>();
    public List<TowerProduct.Type> archer_type_list = new List<TowerProduct.Type>();
    public List<TowerProduct.Type> cannon_type_list = new List<TowerProduct.Type>();
    public List<TowerProduct.Type> magic_type_list = new List<TowerProduct.Type>();
    public Dictionary<TowerProduct.Type, TowerProduct> Tower_Lib
    {
        get
        {
            return tower_lib;
        }
    }

    private void init(string file_name)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(file_name);

        var root = doc.SelectSingleNode("Root") as XmlElement;
        foreach (XmlElement ele in root.ChildNodes)
        {
            TowerProduct tower_pro = new TowerProduct();
            tower_pro.init_from_xml(ele);
            var tmp_type = tower_pro.id / 100;
            switch (tmp_type)
            {
                case 20:
                    archer_type_list.Add((TowerProduct.Type)tower_pro.id);
                    tower_pro.atk_type = TowerProduct.AttackType.Archer;
                    break;
                case 21:
                    cannon_type_list.Add((TowerProduct.Type)tower_pro.id);
                    tower_pro.atk_type = TowerProduct.AttackType.Cannon;
                    break;
                case 22:
                    magic_type_list.Add((TowerProduct.Type)tower_pro.id);
                    tower_pro.atk_type = TowerProduct.AttackType.Magic;
                    break;
            }

            tower_lib.Add((TowerProduct.Type)tower_pro.id, tower_pro);
        }
    }

    public TowerProduct create_tower(TowerProduct.Type type)
    {
        if (tower_lib.ContainsKey(type))
        {
            return new TowerProduct(tower_lib[type]);
        }
        return null;
    }

    public int get_tower_price(TowerProduct.Type type)
    {
        if (tower_lib.ContainsKey(type))
        {
            return tower_lib[type].gold;
        }
        return 0;
    }

    public string get_tower_name(TowerProduct.Type type)
    {
        if (tower_lib.ContainsKey(type))
        {
            return tower_lib[type].name;
        }
        return null;
    }

    public int get_tower_sale_price(TowerProduct tower_pro)
    {
        int refund = 0;
        List < TowerProduct.Type > tower_list = new List<TowerProduct.Type>();
        switch (tower_pro.atk_type)
        {
            case TowerProduct.AttackType.Archer:
                tower_list = archer_type_list;
                break;
            case TowerProduct.AttackType.Cannon:
                tower_list = cannon_type_list;
                break;
            case TowerProduct.AttackType.Magic:
                tower_list = magic_type_list;
                break;
        }

        for (int i = 0; i < tower_pro.lvl; i++)
        {
            refund += get_tower_price(tower_list[i]);
        }
        return refund * 4 / 5;
    }

    public List<TowerProduct.AttackType> get_other_tower_atk_type(TowerProduct tower_pro)
    {
        List<TowerProduct.AttackType> atk_type_list = new List<TowerProduct.AttackType>();
        switch (tower_pro.atk_type)
        {
            case TowerProduct.AttackType.Archer:
                atk_type_list.Add(TowerProduct.AttackType.Cannon);
                atk_type_list.Add(TowerProduct.AttackType.Magic);
                break;
            case TowerProduct.AttackType.Cannon:
                atk_type_list.Add(TowerProduct.AttackType.Archer);
                atk_type_list.Add(TowerProduct.AttackType.Magic);
                break;
            case TowerProduct.AttackType.Magic:
                atk_type_list.Add(TowerProduct.AttackType.Cannon);
                atk_type_list.Add(TowerProduct.AttackType.Archer);
                break;
        }
        return atk_type_list;
    }

    //根据id得到指定的type类型
    public TowerProduct.Type get_type_from_id(int id)
    {
        var tmp_type = id / 100;
        var tmp_index = id % 10;
        switch (tmp_type)
        {
            case 20:
                switch (tmp_index)
                {
                    case 0:
                        return TowerProduct.Type.Archer1;
                    case 1:
                        return TowerProduct.Type.Archer2;
                    case 2:
                        return TowerProduct.Type.Archer3;
                    case 3:
                        return TowerProduct.Type.Archer4A;
                    case 4:
                        return TowerProduct.Type.Archer4B;
                }
                break;
            case 21:
                switch (tmp_index)
                {
                    case 0:
                        return TowerProduct.Type.Cannon1;
                    case 1:
                        return TowerProduct.Type.Cannon2;
                    case 2:
                        return TowerProduct.Type.Cannon3;
                    case 3:
                        return TowerProduct.Type.Cannon4A;
                    case 4:
                        return TowerProduct.Type.Cannon4B;
                }
                break;
            case 22:
                switch (tmp_index)
                {
                    case 0:
                        return TowerProduct.Type.Magic1;
                    case 1:
                        return TowerProduct.Type.Magic2;
                    case 2:
                        return TowerProduct.Type.Magic3;
                    case 3:
                        return TowerProduct.Type.Magic4A;
                    case 4:
                        return TowerProduct.Type.Magic4B;
                }
                break;
        }
        return 0;
    }

    TowerFC()
    {
        init(AppConst.towerConfigXMLUrl);
    }
    public static readonly TowerFC Instance = new TowerFC();
}
