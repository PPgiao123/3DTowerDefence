using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArrow : Bullet
{
    int split_num;
    int lgtning_emy_num;

    void Start()
    {
        
    }

    public override void init_bullet(Tower _tower, int fire_pos_idx, string prefab_path)
    {
        base.init_bullet(_tower, fire_pos_idx, prefab_path);
        transform.position = Tower.fire_pos_list[fire_pos_idx].transform.position;
        transform.LookAt(Tower.Target.head_pos.transform);
        rb.velocity = transform.forward * speed;
        split_num = 4;
        lgtning_emy_num = 5;
        shades_atk_cannon = (int)(TowerFC.Instance.create_tower(TowerProduct.Type.Cannon1).atk / 2f);
        shades_atk_magic = (int)(TowerFC.Instance.create_tower(TowerProduct.Type.Magic1).atk / 2f);
    }

    //不是从塔发出来的
    public override void init_random_bullet(Vector3 fire_pos, TowerProduct.Type _type,
        Tower.UnitedType united_type, int shade_value, int _lvl)
    {
        base.init_random_bullet(fire_pos, _type, united_type, shade_value, _lvl);
        shades_atk_archer = shade_value;
        speed = 80;
    }

    void Update()
    {
        if (is_shade)
        {
            track_target(shade_target.head_pos.transform);
        }
        if (Tower && Tower.Target)
        {
            track_target(Tower.Target.head_pos.transform);
        }
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x - 90,
            transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        //联合塔
        if (Tower && Tower.Is_United)
        {
            switch (Tower.United_Type)
            {
                case Tower.UnitedType.UT_ARCHER_CANNON:
                    foreach (var vector in create_spread_vector3(split_num))
                    {
                        var ball = GameObject.Instantiate<CannonBall>
                            (Resources.Load<CannonBall>(AppConst.Canon_Lvl1_Ball_Prefab));
                        var ball_pos = transform.position + vector * 3;
                        ball.init_random_bullet(ball_pos, TowerProduct.Type.Cannon1, Tower.United_Type,
                            shades_atk_cannon, 1);
                        ball.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        var ball_speed = TowerFC.Instance.
                            create_tower(TowerProduct.Type.Cannon1).bullet_speed / 10;
                        var ball_rb = ball.GetComponent<Rigidbody>();
                        ball_rb.velocity = vector * ball_speed;
                    }
                    break;
                case Tower.UnitedType.UT_ARCHER_MAGIC:
                    List<Enemy> emy_lst = new List<Enemy>();
                    if (Tower.Target)
                    {
                        foreach (var e in GameControl.Instance.find_enemies
                        (Tower.Target.transform.position, 20f, lgtning_emy_num))
                        {
                            emy_lst.Add(e);
                        }
                    }
                    if (emy_lst.Count < 2)
                    {
                        break;
                    }
                    //最多5个敌人4条电链
                    for (int i = 0; i < emy_lst.Count - 1; i++)
                    {
                        var lgt = GameObject.Instantiate<MagicLightning>
                            (Resources.Load<MagicLightning>(AppConst.SimpleLightning_Prefab));
                        lgt.init_random_bullet(transform.position, TowerProduct.Type.Magic1, Tower.United_Type,
                            shades_atk_magic, 1);
                        lgt.GetComponent<LightningBoltScript>().StartObject = emy_lst[i].gameObject;
                        lgt.GetComponent<LightningBoltScript>().EndObject = emy_lst[i+1].gameObject;
                    }
                    break;
            }
        }
    }
}
