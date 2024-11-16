using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt : MonoBehaviour
{
    ElectricGround ele_gnd;

    private void Awake()
    {
        ele_gnd = GameObject.Instantiate<ElectricGround>
            (Resources.Load<ElectricGround>(AppConst.ElectricGround_Prefab));
        
    }

    private void Start()
    {
        ele_gnd.transform.position = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Background":
                break;
            case "Enemy":
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                //落雷伤害由所种植的闪电塔个数而来20* num
                int hurt_value = 20 + 20 * GameControl.Instance.Num_Magic_Tower;
                enemy.get_hurt(hurt_value);
                break;
        }
    }
}
