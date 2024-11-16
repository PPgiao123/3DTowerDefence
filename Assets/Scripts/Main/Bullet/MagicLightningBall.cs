using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLightningBall : Bullet
{
    float speed_down; 

    void Update()
    {
        
    }

    public override void init_bullet(Tower _tower, int fire_pos_idx, string prefab_path)
    {
        base.init_bullet(_tower, fire_pos_idx, prefab_path);
        //united_fire_pos就用fire_pos代替
        transform.position = _tower.United_Type == Tower.UnitedType.UT_CANNON_ARCHER ?
            Tower.united_fire_pos_list[fire_pos_idx].transform.position :
            Tower.fire_pos_list[fire_pos_idx].transform.position;
        shades_atk_archer = (int)(TowerFC.Instance.create_tower(TowerProduct.Type.Archer1).atk);
        shades_atk_magic = (int)(TowerFC.Instance.create_tower(TowerProduct.Type.Magic1).atk);
        if (Tower && Tower.Target)
        {
            track_target(Tower.Target.head_pos.transform);
        }
        speed_down = 0.5f;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag != "Explosion")
        {
            //联合塔
            switch (Tower.United_Type)
            {
                case Tower.UnitedType.UT_MAGIC_ARCHER:
                    if (other.gameObject && other.tag == "Enemy")
                    {
                        Enemy emy = other.gameObject.GetComponent<Enemy>();
                        emy.Enemy_Pro.speed -= speed_down;
                    }
                    break;
                case Tower.UnitedType.UT_MAGIC_CANNON:
                    var elec_ball_expl = GameObject.Instantiate<ElectricBallExplosion>
                            (Resources.Load<ElectricBallExplosion>(AppConst.MagicCannonExplosion_Prefab));
                    elec_ball_expl.transform.position = transform.position;
                    if (Tower.Target)
                    {
                        elec_ball_expl.transform.position = Tower.Target.transform.position;
                    }
                    break;
            }
        }
    }
}
