using DigitalRuby.LightningBolt;
using Redcode.Pools;
using System.Collections;
using System.Collections.Generic;
/*using System.Drawing;*/
using System.Net;
using TMPro;
using Unity.Netcode;


/*using System.Numerics;*/
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class Tower : NetworkBehaviour
{
    public enum UnitedType
    {
        UT_ARCHER_CANNON,
        UT_ARCHER_MAGIC,
        UT_CANNON_ARCHER,
        UT_CANNON_MAGIC,
        UT_MAGIC_ARCHER,
        UT_MAGIC_CANNON,
    }

    public TowerProduct tower_pro;
    Enemy target;
    public Enemy Target
    {
        get
        {
            return target;
        }
    }
    /*List<Enemy> target_list;
    public List<Enemy> Target_list
    {
        get
        {
            return target_list;
        }
    }*/

    public Bullet tower_bullet;
    public Transform[] fire_pos_list;
    protected float fire_time;
    protected string prefab_path;
    protected string sound_effect;
    public string Sound_Effect
    {
        get 
        {
            return sound_effect;
        }
        set
        {
            sound_effect = value;
        }
    }

    public LineRenderer lineRenderer;
    public int positionCount;
    public Material material;

    bool is_united;
    public bool Is_United {
        get 
        {
            return is_united;
        }
        set
        {
            is_united = value;
        }
    }
    public GameObject united_obj;
    public Transform united_pos;
    public Transform[] united_fire_pos_list;
    UnitedType united_type;
    public UnitedType United_Type
    {
        get
        {
            return united_type;
        }
        set
        {
            united_type = value;
        }
    }

    public RecordText record_txt;
    int enemy_killed_num;
    public int Enemy_Killed_Num { get { return enemy_killed_num; } set { enemy_killed_num = value; } }

    NetworkVariable<int> net_record_txt = new NetworkVariable<int>(0);
    //public NetworkVariable<int> net_tower_type = new NetworkVariable<int>(0);
    public NetworkVariable<int> net_united_type = new NetworkVariable<int>(0);
    public NetworkVariable<int> net_atk = new NetworkVariable<int>(0);
    public NetworkVariable<float> net_fire_speed = new NetworkVariable<float>(0);

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        record_txt = transform.Find("RecordText").GetComponent<RecordText>();

        net_record_txt.OnValueChanged += (pre_value, new_value) =>
        {
            enemy_killed_num = new_value;
            record_txt.GetComponent<TMP_Text>().text = enemy_killed_num.ToString();
        };
        net_united_type.OnValueChanged += (pre_value, new_value) =>
        {
            united_type = (UnitedType)new_value;
        };
        net_atk.OnValueChanged += (pre_value, new_value) =>
        {
            tower_pro.atk = new_value;
        };
        net_fire_speed.OnValueChanged += (pre_value, new_value) =>
        {
            tower_pro.fire_speed = new_value;
        };
    }

    public virtual void init(TowerProduct _tower_pro)
    {
        tower_pro = _tower_pro;

        update_record_text();
    }

    void Update()
    {
        find_target();
        fire();
        rotate_united_obj();
    }

    protected virtual void find_target()
    {
        if (target)
        {
            float dis = UnityEngine.Vector3.Distance(transform.position, target.transform.position);
            if (dis > tower_pro.range)
            {
                target = null;
            }
            else
            {
                return;
            }
        }

        foreach (var enemy in GameControl.Instance.enemy_list)
        {
            if (enemy)
            {
                float dis = UnityEngine.Vector3.Distance(transform.position, enemy.transform.position);
                if (dis <= tower_pro.range)
                {
                    target = enemy;
                    return;
                }
            }
        }
    }

    protected virtual void fire()
    {
        fire_time += Time.deltaTime;
        if (target)
        {
            switch ((TowerProduct.Type)tower_pro.id)
            {
                case TowerProduct.Type.Archer1:
                    prefab_path = AppConst.Archer_Arrow_Prefab;
                    sound_effect = "ArcherEject";
                    break;
                case TowerProduct.Type.Archer2:
                    prefab_path = AppConst.Archer_Arrow_Prefab;
                    sound_effect = "ArcherEject";
                    break;
                case TowerProduct.Type.Archer3:
                    prefab_path = AppConst.Archer_Arrow_Prefab;
                    sound_effect = "ArcherEject";
                    break;
                case TowerProduct.Type.Archer4A:
                    prefab_path = AppConst.Archer_Arrow_Prefab;
                    sound_effect = "ArcherEject";
                    break;
                case TowerProduct.Type.Archer4B:
                    prefab_path = AppConst.Archer_Arrow_Prefab;
                    sound_effect = "ArcherEject";
                    break;
                case TowerProduct.Type.Cannon1:
                    prefab_path = AppConst.Canon_Lvl1_Ball_Prefab;
                    sound_effect = "CannonEject";
                    break;
                case TowerProduct.Type.Cannon2:
                    prefab_path = AppConst.Canon_Lvl2_Ball_Prefab;
                    sound_effect = "CannonEject";
                    break;
                case TowerProduct.Type.Cannon3:
                    prefab_path = AppConst.Canon_Lvl3_Rocket_Prefab;
                    sound_effect = "CannonEject";
                    break;
                case TowerProduct.Type.Cannon4A:
                    prefab_path = AppConst.Canon_Lvl4A_Rocket_Prefab;
                    sound_effect = "CannonEject";
                    if (united_type == UnitedType.UT_CANNON_ARCHER)
                    {
                        prefab_path = AppConst.CannonArrow_Prefab;
                        sound_effect = "CannonArrowEject";
                    }
                    break;
                case TowerProduct.Type.Cannon4B:
                    prefab_path = AppConst.Canon_Lvl4B_Rocket_Prefab;
                    sound_effect = "CannonEject";
                    if (united_type == UnitedType.UT_CANNON_ARCHER)
                    {
                        prefab_path = AppConst.CannonArrow_Prefab;
                        sound_effect = "CannonArrowEject";
                    }
                    break;
                case TowerProduct.Type.Magic1:
                    prefab_path = AppConst.SimpleLightning_Prefab;
                    sound_effect = "MagicEject";
                    break;
                case TowerProduct.Type.Magic2:
                    prefab_path = AppConst.SimpleLightning_Prefab;
                    sound_effect = "MagicEject";
                    break;
                case TowerProduct.Type.Magic3:
                    prefab_path = AppConst.IntensiveLightning_Prefab;
                    sound_effect = "MagicEject";
                    break;
                case TowerProduct.Type.Magic4A:
                    prefab_path = AppConst.IntensiveLightning_Prefab;
                    sound_effect = "MagicEject";
                    switch (United_Type)
                    {
                        case UnitedType.UT_MAGIC_ARCHER:
                            prefab_path = AppConst.MagicArrowBall_Prefab;
                            sound_effect = "MagicArrowEject";
                            break;
                        case UnitedType.UT_MAGIC_CANNON:
                            prefab_path = AppConst.MagicCannonBall_Prefab;
                            sound_effect = "MagicArrowEject";
                            break;
                    }
                    break;
                case TowerProduct.Type.Magic4B:
                    prefab_path = AppConst.IntensiveLightning_Prefab;
                    sound_effect = "MagicEject";
                    switch (United_Type)
                    {
                        case UnitedType.UT_MAGIC_ARCHER:
                            prefab_path = AppConst.MagicArrowBall_Prefab;
                            sound_effect = "MagicArrowEject";
                            break;
                        case UnitedType.UT_MAGIC_CANNON:
                            prefab_path = AppConst.MagicCannonBall_Prefab;
                            sound_effect = "MagicArrowEject";
                            break;
                    }
                    break;
                default:
                    prefab_path = "";
                    sound_effect = "";
                    break;
            }
        }
    }

    public void draw_range()
    {
        Vector3 center = transform.position;
        float radius = tower_pro.range;
        lineRenderer.positionCount = 360;
        //将LineRenderer绘制线的宽度 即圆的宽度 设为0.04
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        //每一度求得一个在圆上的坐标点
        lineRenderer.loop = true;
        Quaternion direction = Quaternion.FromToRotation(Vector3.up, Vector3.up);
        //每一度求得一个在圆上的坐标点
        lineRenderer.material = material;

        for (int i = 0; i < 360; i++)
        {
            float x = center.x + radius * Mathf.Cos(i * Mathf.PI / 180f);
            float z = center.z + radius * Mathf.Sin(i * Mathf.PI / 180f);
            Vector3 pos = new Vector3(x, 0, z);
            pos = direction * pos;

            lineRenderer.SetPosition(i, pos);
        }
        lineRenderer.enabled = false;
    }

    protected virtual void rotate_united_obj()
    {
        if (united_obj)
        {
            united_obj.transform.RotateAround(transform.position, transform.up, 100 * Time.deltaTime);
        }
    }

    public void update_record_text()
    {
        record_txt.GetComponent<TMP_Text>().text = enemy_killed_num.ToString();
        if (GameControl.Instance.network_connected)
        {
            if (IsServer)
            {
                net_record_txt.Value = enemy_killed_num;
            }
            else if (IsClient)
            {
                UpdateRecordTxtServerRpc(enemy_killed_num);
            }
        }
    }

    public void update_atk()
    {
        float num = tower_pro.atk * (enemy_killed_num) * 0.01f;
        num = num < 1 ? 1 : num;
        tower_pro.atk = (int)(tower_pro.atk + num);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (target)
        {
            for (int i = 0; i < fire_pos_list.Length; i++)
            {
                Gizmos.DrawLine(fire_pos_list[i].transform.position, target.head_pos.transform.position);
            }
        }

        Gizmos.color = Color.yellow;
        if (target)
        {
            Gizmos.DrawWireSphere(transform.position, tower_pro.range);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateRecordTxtServerRpc(int enemy_killed_num)
    {
        net_record_txt.Value = enemy_killed_num;
    }
}
