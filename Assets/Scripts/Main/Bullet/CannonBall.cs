using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : Bullet
{
    bool is_track;
    Object explosion_effect;

    float acc;

    void Start()
    {
        
    }

    public override void init_bullet(Tower _tower, int fire_pos_idx, string prefab_path)
    {
        base.init_bullet(_tower, fire_pos_idx, prefab_path);
        //united_fire_pos就用fire_pos代替
        transform.position = _tower.United_Type == Tower.UnitedType.UT_CANNON_ARCHER ? 
            Tower.united_fire_pos_list[fire_pos_idx].transform.position : 
            Tower.fire_pos_list[fire_pos_idx].transform.position;
        load_explosion((TowerProduct.Type)_tower.tower_pro.id);
        track_compute(_tower.tower_pro.lvl);
        shades_atk_archer = (int)(TowerFC.Instance.create_tower(TowerProduct.Type.Archer1).atk);
        shades_atk_magic = (int)(TowerFC.Instance.create_tower(TowerProduct.Type.Magic1).atk);
    }

    //不是从塔发出来的
    public override void init_random_bullet(Vector3 fire_pos, TowerProduct.Type _type, 
        Tower.UnitedType united_type, int shade_value, int _lvl)
    {
        base.init_random_bullet(fire_pos, _type, united_type, shade_value, _lvl);
        shades_atk_cannon = shade_value;
        load_explosion(_type);
    }

    public void load_explosion(TowerProduct.Type _type)
    {
        switch (_type)
        {
            case TowerProduct.Type.Cannon1:
                explosion_effect = Resources.Load(AppConst.Explo_Small_Red_NoSmoke_Prefab);
                break;
            case TowerProduct.Type.Cannon2:
                explosion_effect = Resources.Load(AppConst.Explo_Medium_Red_Prefab);
                break;
            case TowerProduct.Type.Cannon3:
                acc = 0.25f;
                explosion_effect = Resources.Load(AppConst.Explo_Large_Red_Prefab);
                break;
            case TowerProduct.Type.Cannon4A:
                acc = 0.5f;
                explosion_effect = Resources.Load(AppConst.Explo_Large_Joker_Prefab);
                break;
            case TowerProduct.Type.Cannon4B:
                acc = 0.1f;
                explosion_effect = Resources.Load(AppConst.Explo_Large_Purple_Prefab);
                break;
        }
    }

    public void track_compute(int _lvl)
    {
        transform.LookAt(Tower.Target.head_pos.transform);
        if (_lvl < 2)
        {
            var ahead_pos = new Vector3(Tower.Target.head_pos.position.x,
                Tower.Target.head_pos.position.y, Tower.Target.head_pos.position.z + 2);
            transform.LookAt(ahead_pos);
        }

        
        rb.velocity = transform.forward * speed;

        if (_lvl > 2 && Tower.United_Type != Tower.UnitedType.UT_CANNON_ARCHER)
        {
            is_track = true;
        }
    }

    void Update()
    {
        if (Tower && Tower.Target && is_track)
        {
            track_target(Tower.Target.head_pos.transform);
            speed += acc;
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x + 90,
            transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag != "Explosion")//base中的判断可能拦不住
        {
            var explotion = (GameObject)GameObject.Instantiate(explosion_effect);
            explotion.transform.position = this.transform.position;
            //联合塔
            if (Tower && Tower.Is_United)
            {
                switch (Tower.United_Type)
                {
                    case Tower.UnitedType.UT_CANNON_ARCHER:
                        break;
                    case Tower.UnitedType.UT_CANNON_MAGIC:
                        var thunderbolt = GameObject.Instantiate<ThunderBolt>
                                (Resources.Load<ThunderBolt>(AppConst.Thunderbolt_Prefab));
                        thunderbolt.transform.position = transform.position;
                        if (Tower.Target)
                        {
                            thunderbolt.transform.position = Tower.Target.transform.position;
                        }
                        break;
                }
            }
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
