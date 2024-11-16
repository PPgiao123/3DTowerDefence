using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : Tower
{
    protected override void fire()
    {
        if (this.Target)
        {
            base.fire();
            if (fire_time > tower_pro.fire_speed)//∑¢…‰∆µ¬ øÿ÷∆
            {
                if (Is_United)
                {
                    switch (United_Type)
                    {
                        case UnitedType.UT_CANNON_ARCHER:
                            for (int i = 0; i < united_fire_pos_list.Length; i++)
                            {
                                var missile_arrow = GameObject.Instantiate<CannonBall>
                                (Resources.Load<CannonBall>(prefab_path));
                                missile_arrow.init_bullet(this, i, prefab_path);
                            }
                            break;
                        case UnitedType.UT_CANNON_MAGIC:
                            var cannon_ball = GameObject.Instantiate<CannonBall>
                            (Resources.Load<CannonBall>(prefab_path));
                            cannon_ball.init_bullet(this, 0, prefab_path);
                            break;
                    }
                }
                else
                {
                    for (int i = 0; i < fire_pos_list.Length; i++)
                    {
                        var cannon_ball = GameObject.Instantiate<CannonBall>(Resources.Load<CannonBall>(prefab_path));
                        cannon_ball.init_bullet(this, i, prefab_path);
                    }
                }
                AudioManage.Instance.play_audio(sound_effect);
                fire_time = 0;
            }
        }
    }
}
