using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody rb;
    protected float speed;

    Tower tower;
    public Tower Tower
    {
        get
        {
            return tower;
        }
    }
    public Material[] materials;
    public string prefab_path;

    public bool is_shade;
    public Enemy shade_target;
    public Tower.UnitedType shade_tower_type;
    public int shades_atk_archer;
    public int shades_atk_cannon;
    public int shades_atk_magic;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void init_bullet(Tower _tower, int fire_pos_idx, string prefab_path)
    {
        tower = _tower;
        speed = Tower.tower_pro.bullet_speed;
    }

    //不是从塔发出来的
    public virtual void init_random_bullet(Vector3 fire_pos, TowerProduct.Type _type, 
        Tower.UnitedType united_type, int shade_value, int _lvl=1)
    {
        shade_tower_type = united_type;
        is_shade = true;
        transform.position = fire_pos;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet" || other.tag == "Explosion")
        {
            return;
        }
        switch (other.tag)
        {
            case "Background":
                break;
            case "Enemy":
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                int hurt_value = cal_hurt_value();
                enemy.get_hurt(hurt_value, Tower);
                break;
        }
        Destroy(this.gameObject);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bullet" || collision.transform.tag == "Explosion")
        {
            return;
        }
        switch (collision.transform.tag)
        {
            case "Background":
                break;
            case "Enemy":
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                int hurt_value = cal_hurt_value();
                enemy.get_hurt(hurt_value, Tower);
                break;
        }
        Destroy(this.gameObject);
    }

    protected void track_target(Transform _tar)
    {
        transform.LookAt(_tar);
        rb.velocity = transform.forward * speed;
    }

    public List<Vector3> create_spread_vector3(int _split_num, bool is_horizon=false)
    {
        List<Vector3> vec3_list = new List<Vector3>();
        float angle = Random.Range(0, 360);
        float radian = 0;//弧度
        if (_split_num == 0)
        {
            return null;
        }

        float delta = 360f / _split_num;
        for (int i = 0; i < _split_num; i++)
        {
            radian = angle * Mathf.PI / 180f;
            if (is_horizon)
            {
                vec3_list.Add(new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian)).normalized);
            }
            else
            {
                vec3_list.Add(new Vector3(Mathf.Sin(radian), 1f, Mathf.Cos(radian)).normalized);
            }
            angle += delta;
        }
        
        return vec3_list;
    }

    private int cal_hurt_value()
    {
        int hurt_value = 0;
        if (is_shade)
        {
            switch (shade_tower_type)
            {
                case Tower.UnitedType.UT_CANNON_ARCHER:
                case Tower.UnitedType.UT_MAGIC_ARCHER:
                    hurt_value = shades_atk_archer;
                    break;
                case Tower.UnitedType.UT_ARCHER_CANNON:
                case Tower.UnitedType.UT_MAGIC_CANNON:
                    hurt_value = shades_atk_cannon;
                    break;
                case Tower.UnitedType.UT_ARCHER_MAGIC:
                case Tower.UnitedType.UT_CANNON_MAGIC:
                    hurt_value = shades_atk_magic;
                    break;
            }
        }
        else
        {
            hurt_value = tower.tower_pro.atk;
        }
        return hurt_value;
    }

    
}
