using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLightning : Bullet
{
    TowerProduct tower_pro;
    float timer;
    float shade_remain_time;

    public void init_bullet(Tower _tower, int fire_pos_idx, string prefab_path, TowerProduct _tower_pro)
    {
        base.init_bullet(_tower, fire_pos_idx, prefab_path);
        tower_pro = _tower_pro;
    }

    //不是从塔发出来的
    public override void init_random_bullet(Vector3 fire_pos, TowerProduct.Type _type,
        Tower.UnitedType united_type, int shade_value, int _lvl)
    {
        base.init_random_bullet(fire_pos, _type, united_type, shade_value, _lvl);
        shades_atk_magic = shade_value;
        shade_remain_time = 0.5f;
        Invoke("self_destroy", shade_remain_time);
    }

    void Update()
    {
        if (is_shade)
        {
            timer += Time.deltaTime;
            if (timer >= shade_remain_time)
            {
                if (GetComponent<LightningBoltScript>().StartObject)
                {
                    GetComponent<LightningBoltScript>().StartObject.GetComponent<Enemy>().get_hurt(shades_atk_magic);
                }
                if (GetComponent<LightningBoltScript>().EndObject)
                {
                    GetComponent<LightningBoltScript>().EndObject.GetComponent<Enemy>().get_hurt(shades_atk_magic);
                }
                timer = 0;
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= tower_pro.fire_speed)
            {
                if (Tower.Target)
                {
                    Tower.Target.get_hurt(tower_pro.atk, Tower);
                }
                timer = 0;
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    private void self_destroy()
    {
        Destroy(this.gameObject);
    }
}
