using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : Tower
{
    protected override void fire()
    {
        if (this.Target)
        {
            base.fire();
            if (fire_time > tower_pro.fire_speed)//·¢ÉäÆµÂÊ¿ØÖÆ
            {
                for (int i = 0; i < fire_pos_list.Length; i++)
                {
                    var arrow = GameObject.Instantiate<ArcherArrow>(Resources.Load<ArcherArrow>(prefab_path));
                    arrow.init_bullet(this, i, prefab_path);
                    AudioManage.Instance.play_audio(sound_effect);
                }
                fire_time = 0;
            }
        }
    }
}
