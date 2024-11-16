using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EnemyFC : MonoBehaviour
{
    Dictionary<EnemyProduct.Type, EnemyProduct> enemy_lib = new Dictionary<EnemyProduct.Type, EnemyProduct>();

    private void init(string file_name)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(file_name);

        var root = doc.SelectSingleNode("Root") as XmlElement;
        foreach (XmlElement ele in root.ChildNodes)
        {
            EnemyProduct enemy_pro = new EnemyProduct();
            enemy_pro.init_from_xml(ele);

            enemy_lib.Add((EnemyProduct.Type)enemy_pro.id, enemy_pro);
        }
    }

    public EnemyProduct create_enemy(EnemyProduct.Type type)
    {
        if (enemy_lib.ContainsKey(type))
        {
            return new EnemyProduct(enemy_lib[type]);
        }
        return null;
    }

    EnemyFC()
    {
        init(AppConst.enemyConfigXMLUrl);
    }
    public static readonly EnemyFC Instance = new EnemyFC();
}
