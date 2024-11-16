using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    public TowerProduct.Type type;
    Button btn;
    public Tower cur_tower;
    public int sequence;//ÍøÂçÍ¬²½client

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            if (GameControl.Instance.network_connected && !GameControl.Instance.Is_Defense)
            {
                return;
            }

            GameControl.Instance.show_choose_panel();
            GameControl.cur_tower_btn = this;
            if (GameControl.cur_tower_btn.cur_tower)
            {
                ContentPanel.Instance.update_display_tower(GameControl.cur_tower_btn.cur_tower.tower_pro);
            }
            else
            {
                ContentPanel.Instance.update_display_tower();
            }
        });
    }

    public void pointer_enter()
    {
        if (cur_tower)
        {
            cur_tower.lineRenderer.enabled = true;
        }
    }

    public void pointer_exit()
    {
        if (cur_tower)
        {
            cur_tower.lineRenderer.enabled = false;
        }
    }
}
