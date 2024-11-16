using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : NetworkBehaviour
{
    PointNode cur_point;
    public PointNode Cur_Point
    {
        get
        {
            return cur_point;
        }
    }
    PointNode next_point;
    public PointNode Next_Point
    {
        get
        {
            return next_point;
        }
    }
    Rigidbody rb;
    Animator animator;
    EnemyProduct enemy_pro;
    public EnemyProduct Enemy_Pro
    {
        get 
        {
            return enemy_pro;
        }
    }

    public Transform head_pos;
    float jammed_timer;

    bool is_confused;
    public bool Is_Confused { get { return is_confused; } set { is_confused = value; } }

    NetworkVariable<float> net_speed = new NetworkVariable<float>(0);


    void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
    }

    public void init(EnemyProduct _enemy_pro)
    {
        var idx = Random.Range(0, GameControl.Instance.birth_points.Length);
        cur_point = GameControl.Instance.birth_points[idx];
        next_point = get_next_point();
        transform.position = cur_point.transform.position;
        enemy_pro = _enemy_pro;
        jammed_timer = 0;
    }

    void Update()
    {
        move();
    }

    private void move()
    {
        if (GameControl.Instance.network_connected)
        {
            if (IsServer)
            {
                if (cur_point.next_points.Length > 0)
                {
                    update_velocity();
                    //切换目标点
                    update_next_point();
                    net_speed.Value = enemy_pro.speed;
                }
            }
        }
        else
        {
            if (cur_point.next_points.Length > 0)
            {
                update_velocity();
                //切换目标点
                update_next_point();
            }
        }
    }

    private void update_velocity()
    {
        transform.LookAt(next_point.transform);
        rb.velocity = transform.forward * enemy_pro.speed;
        animator.SetFloat("Speed", enemy_pro.speed);

        enemy_pro.speed += enemy_pro.acc * Time.deltaTime;
        enemy_pro.speed = enemy_pro.speed > enemy_pro.maxSpeed ? enemy_pro.maxSpeed : enemy_pro.speed;
    }

    public void get_hurt(int hurt, Tower _tower=null)
    {
        enemy_pro.cur_hp -= hurt;
        if (enemy_pro.cur_hp <= 0)
        {
            if (_tower)
            {
                _tower.Enemy_Killed_Num++;
                _tower.update_record_text();
                _tower.update_atk();
            }
            GameControl.Instance.remove_enemy(this);
            GameControl.Instance.update_gold(enemy_pro.gold);
            Destroy(this.gameObject);
            AudioManage.Instance.play_audio("EnemyDeath");
            return;
        }
        
        float filled = enemy_pro.cur_hp * 1.0f / enemy_pro.maxHp * 1.0f;
        transform.GetComponentInChildren<HpBar>().update_hp_bar(filled);
    }

    //配置下一路径点(避免堵塞)
    public PointNode get_next_point()
    {
        PointNode min_emy_num_pt = cur_point.next_points[0];
        if (cur_point.next_points.Length > 1)
        {
            Dictionary<PointNode, int> emy_num_list = new Dictionary<PointNode, int>();
            foreach (var e in cur_point.next_points)
            {
                int num = e.get_enemy_num_in_point();
                emy_num_list.Add(e, num);
            }

            foreach (var pt in emy_num_list.Keys)//选取下一子节点怪物最少的一个
            {
                if (emy_num_list[pt] < emy_num_list[min_emy_num_pt])
                {
                    min_emy_num_pt = pt;
                }
            }
        }
        return min_emy_num_pt;
    }

    public void update_next_point()
    {
        float dis = Vector3.Distance(transform.position, next_point.transform.position);
        if (dis < 6)
        {
            jammed_timer += Time.deltaTime;
            if (jammed_timer > 1 || dis < 2)
            {
                cur_point = next_point;
                next_point = get_next_point();
                jammed_timer = 0;
            }
        }
    }

    /*public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 20f);
    }*/
}
