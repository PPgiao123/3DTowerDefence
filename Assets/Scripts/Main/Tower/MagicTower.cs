using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MagicTower : Tower
{
    MagicLightning lightning;
    public MagicLightning Lightning
    {
        get
        {
            return lightning;
        }
        set
        {
            lightning = value;
        }
    }
    LightningBoltScript lgtng;
    
    public LightningBoltScript Lgtng
    {
        get
        {
            return lgtng;
        }
        set
        {
            lgtng = value;
        }
    }
    
    public bool is_lightning_on()
    {
        if (Lgtng == null)
        {
            return false;
        }
        return true;
    }

    protected override void fire()
    {
        base.fire();
        if (Is_United)
        {
            create_lightning_ball(United_Type);
        }
        else
        {
            create_lightning_chain();
        }
    }

    void create_lightning_chain()
    {
        if (Target)
        {
            if (!this.is_lightning_on())
            {
                for (int i = 0; i < fire_pos_list.Length; i++)
                {
                    lightning = GameObject.Instantiate<MagicLightning>(Resources.Load<MagicLightning>(prefab_path));
                    lightning.init_bullet(this, i, prefab_path, tower_pro);
                    this.Lgtng = lightning.transform.GetComponent<LightningBoltScript>();
                    this.Lgtng.StartObject = this.fire_pos_list[i].gameObject;
                    this.Lgtng.EndObject = this.Target.transform.Find("Head").gameObject;
                }
            }
            else//更换目标
            {
                this.Lgtng.EndObject = this.Target.transform.Find("Head").gameObject;
            }
        }
        else//目标消失
        {
            //销毁闪电
            if (this.is_lightning_on())
            {
                Destroy(this.Lgtng.gameObject);
            }
        }
    }

    void create_lightning_ball(UnitedType utd_type)
    {
        if (Target && fire_time > tower_pro.fire_speed)//发射频率控制
        {
            for (int i = 0; i < united_fire_pos_list.Length; i++)
            {
                //联合塔
                switch (utd_type)
                {
                    case UnitedType.UT_MAGIC_ARCHER:
                        var lgt_arr_ball = GameObject.Instantiate<MagicLightningBall>
                                (Resources.Load<MagicLightningBall>(prefab_path));
                        lgt_arr_ball.init_bullet(this, i, prefab_path);
                        break;
                    case UnitedType.UT_MAGIC_CANNON:
                        var lgt_can_ball = GameObject.Instantiate<MagicLightningBall>
                                (Resources.Load<MagicLightningBall>(prefab_path));
                        lgt_can_ball.init_bullet(this, i, prefab_path);
                        break;
                }
                
            }
            AudioManage.Instance.play_audio(sound_effect);
            fire_time = 0;
        }
    }
}
