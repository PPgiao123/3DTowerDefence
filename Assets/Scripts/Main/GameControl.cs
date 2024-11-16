using Redcode.Pools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : NetworkBehaviour
{
    public PointNode[] birth_points;
    PointNode[] point_nodes;
    public PointNode[] last_points;

    bool test_mode;

    public Transform choose_panel;
    public GameObject player_panel;
    public GameObject exit_panel;
    public GameObject enemy_panel;

    public static TowerBtn cur_tower_btn;
    List<TowerBtn> tower_btn_list = new List<TowerBtn>();

    public List<Enemy> enemy_list = new List<Enemy>();

    public int cur_heart;
    public static int total_heart;
    static SettingPanel.GameMode game_mode;
    static int cur_gold;
    public static int Cur_Gold
    {
        get
        {
            return cur_gold;
        }
    }

    TMP_Text gold_txt;
    TMP_Text group_txt;
    TMP_Text heart_txt;
    public TMP_Text help_txt;
    public bool is_gameover;

    int enemy_total_cnt;//只在Normal中起作用
    private int cur_group;//当前是第几波怪物
    public int total_group;
    public int sml_dragon_cnt;
    public int mid_dragon_cnt;
    public int big_dragon_cnt;

    public GameObject start_btn_clection;

    int num_archer_tower;
    public int Num_Archer_Tower { get { return num_archer_tower; } set { num_archer_tower = value; } }
    int num_cannon_tower;
    public int Num_Cannon_Tower { get { return num_cannon_tower; } set { num_cannon_tower = value; } }
    int num_magic_tower;
    public int Num_Magic_Tower { get { return num_magic_tower; } set { num_magic_tower = value; } }

    public bool network_connected;
    bool is_defense;
    public bool Is_Defense { get { return is_defense; } set { is_defense = value; } }
    private bool is_win;
    public bool Is_Win { get { return is_win; } set { is_win = value; } }

    public List<Sprite> enemy_picked_sprite_list = new List<Sprite>();

    NetworkVariable<int> net_cur_gold = new NetworkVariable<int>(0);
    NetworkVariable<int> net_cur_heart = new NetworkVariable<int>(0);
    NetworkVariable<int> net_total_heart = new NetworkVariable<int>(0);
    NetworkVariable<int> net_cur_group = new NetworkVariable<int>(0);
    NetworkVariable<int> net_total_group = new NetworkVariable<int>(0);
    NetworkVariable<bool> net_defense_confirm = new NetworkVariable<bool>(false);
    NetworkVariable<bool> net_attack_confirm = new NetworkVariable<bool>(false);
    NetworkVariable<bool> net_if_game_start = new NetworkVariable<bool>(false);
    NetworkVariable<int> net_cur_tower_btn = new NetworkVariable<int>(0);
    NetworkVariable<ulong> net_cur_tower = new NetworkVariable<ulong>(0);
    NetworkVariable<int> net_cur_tower_pro = new NetworkVariable<int>(0);
    NetworkVariable<ulong> net_cur_tower_united_obj = new NetworkVariable<ulong>(0);

    private void Awake()
    {
        instance = this;
        point_nodes = GameObject.Find("RoadPointRoot").GetComponentsInChildren<PointNode>();
        foreach (var point in point_nodes)
        {
            if (point.next_points.Length == 0)
            {
                last_points.Append<PointNode>(point);
            }
        }

        int cnt = 0;
        for (int i = 0; i < GameObject.Find("3DCanvas").transform.childCount; i++)
        {
            for (int j = 0; j < GameObject.Find("3DCanvas").transform.GetChild(i).childCount; j++)
            {
                var btn = GameObject.Find("3DCanvas").transform.GetChild(i).transform.GetChild(j).GetComponent<TowerBtn>();
                btn.sequence = cnt;
                tower_btn_list.Add(btn);
                cnt++;
            }
        }

        choose_panel.gameObject.SetActive(false);
        help_txt.gameObject.SetActive(false);

        gold_txt = player_panel.transform.Find("GoldTxt").GetComponent<TMP_Text>();
        heart_txt = player_panel.transform.Find("HeartTxt").GetComponent<TMP_Text>();
        group_txt = player_panel.transform.Find("CurGroupTxt").GetComponent<TMP_Text>();

        network_connected = SettingPanel.Instance.Network_Connected;
        is_defense = SettingPanel.Instance.Is_Defense;
        test_mode = SettingPanel.Instance.test_toggle.isOn;

        exit_panel.gameObject.SetActive(false);
        enemy_panel.gameObject.SetActive(is_defense ? false : true);

        if (network_connected)
        {
            net_cur_gold.Value = ServerControl.Instance.Start_Coin;
            net_cur_heart.Value = ServerControl.Instance.Start_Heart;
            net_total_heart.Value = ServerControl.Instance.Start_Heart;
            net_total_group.Value = ServerControl.Instance.Total_Group;
        }

        net_cur_gold.OnValueChanged += (pre_value, new_value) =>
        {
            cur_gold = new_value;
            gold_txt.text = cur_gold + "$";
        };
        net_cur_heart.OnValueChanged += (pre_value, new_value) =>
        {
            cur_heart = new_value;
            heart_txt.text = "x " + cur_heart;
            StartCoroutine(if_game_over());
        };
        net_cur_group.OnValueChanged += (pre_value, new_value) =>
        {
            cur_group = new_value;
            group_txt.text = "CUR: " + cur_group;
        };
        net_if_game_start.OnValueChanged += (pre_value, new_value) =>
        {
            if (new_value == true)
            {
                StartCoroutine(if_game_start());
            }
        };
        net_total_group.OnValueChanged += (pre_value, new_value) =>
        {
            total_group = new_value;
            enemy_total_cnt = total_group * (sml_dragon_cnt + mid_dragon_cnt + big_dragon_cnt);
        };
        net_total_heart.OnValueChanged += (pre_value, new_value) =>
        {
            total_heart = new_value;
            cur_heart = total_heart;
            heart_txt.text = "x " + cur_heart;
        };
        net_cur_tower_btn.OnValueChanged += (pre_value, new_value) =>
        {
            cur_tower_btn = get_tower_btn_from_seq(new_value);
        };
        net_cur_tower.OnValueChanged += (pre_value, new_value) =>
        {
            cur_tower_btn.cur_tower = GetNetworkObject(new_value).GetComponent<Tower>();
        };
        net_cur_tower_pro.OnValueChanged += (pre_value, new_value) =>
        {
            if (cur_tower_btn.cur_tower)
            {
                cur_tower_btn.cur_tower.tower_pro = TowerFC.Instance.create_tower((TowerProduct.Type)new_value);
            }
        };
        net_cur_tower_united_obj.OnValueChanged += (pre_value, new_value) =>
        {
            cur_tower_btn.cur_tower.united_obj = GetNetworkObject(new_value).gameObject;
        };
        //StartCoroutine(game_start());//只能在自己文件中触发
    }

    private void Start()
    {
        if (network_connected)
        {
            //coin
            cur_gold = ServerControl.Instance.Start_Coin;
            //cur_group
            cur_group = 0;
            group_txt.text = "CUR: " + cur_group;
            //total_group
            total_group = ServerControl.Instance.Total_Group;
            if (total_heart != net_total_heart.Value)
            {
                if (is_defense)
                {
                    total_group = net_total_group.Value;
                }
                else
                {
                    update_total_group(total_group);
                }  
            }
            enemy_total_cnt = total_group * (sml_dragon_cnt + mid_dragon_cnt + big_dragon_cnt);
            //heart
            total_heart = ServerControl.Instance.Start_Heart;
            if (total_heart != net_total_heart.Value)
            {
                //当值不同时，判断以谁为准--Server(defense)的情况下,clients可能还没转换场景Server就发出了消息
                if (is_defense)
                {
                    update_total_heart(total_heart);
                }
                else
                {
                    total_heart = net_total_heart.Value;
                }
            }

            //enemy_panel
            //enemy_picked_list.Add();
            if (!is_defense)
            {
                foreach(var e in enemy_picked_sprite_list)
                {
                    var img = Instantiate<Image>(Resources.Load<Image>(AppConst.EnemyImg_Prefab));
                    img.sprite = e;
                    img.transform.parent = enemy_panel.transform;
                    img.transform.GetChild(0).GetComponent<Image>().sprite = e;
                }
            }
        }
        else
        {
            //测试模式
            if (test_mode)
            {
                //mode
                game_mode = SettingPanel.GameMode.PM_INFINITE;
                //gold
                cur_gold = 99999999;
                //heart
                total_heart = 999;
            }
            else
            {
                //mode
                game_mode = SettingPanel.Instance.Game_Mode;
                //gold
                cur_gold = SettingPanel.Instance.Coin;
                //heart
                total_heart = 10;
            }

            //group
            if (game_mode == SettingPanel.GameMode.PM_NORMAL)
            {
                total_group = SettingPanel.Instance.Enemy_Group;
                cur_group = total_group;
                enemy_total_cnt = total_group * (sml_dragon_cnt + mid_dragon_cnt + big_dragon_cnt);
                group_txt.gameObject.SetActive(false);
            }
            else if (game_mode == SettingPanel.GameMode.PM_INFINITE)
            {
                cur_group = 0;
                group_txt.text = "CUR: " + cur_group;
            }
        }

        cur_heart = total_heart;
        heart_txt.text = "x " + cur_heart;
        gold_txt.text = cur_gold + "$";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exit_panel.gameObject.SetActive(exit_panel.gameObject.activeSelf ? false : true);
        }
    }

    public void game_over()
    {
        is_gameover = true;
        is_win = false;
        SceneManager.LoadScene("End");
    }

    public void game_start_func()
    {
        StartCoroutine(game_start());
    }

    IEnumerator game_start()
    {
        if (network_connected)
        {
            if (is_defense)
            {
                update_def_confirm(true);
            }
            else
            {
                update_atk_confirm(true);
            }

            for (int j = 0; j < start_btn_clection.transform.childCount; j++)
            {
                start_btn_clection.transform.GetChild(j).gameObject.SetActive(true);
            }
            //双方只要点击盾牌就尝试去启动游戏
            update_if_game_start(true);
        }
        else
        {
            switch (game_mode)
            {
                case SettingPanel.GameMode.PM_NORMAL:
                    if (cur_group > 0)
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            yield return StartCoroutine(create_enemy(sml_dragon_cnt, EnemyProduct.Type.FireDragonSmall));
                            yield return StartCoroutine(create_enemy(mid_dragon_cnt, EnemyProduct.Type.FireDragonMid));
                            yield return StartCoroutine(create_enemy(big_dragon_cnt, EnemyProduct.Type.FireDragonBig));
                            yield return new WaitForSeconds(5);
                            if (cur_group > 1)
                            {
                                for (int j = 0; j < start_btn_clection.transform.childCount; j++)
                                {
                                    start_btn_clection.transform.GetChild(j).gameObject.SetActive(true);
                                }
                            }
                        }

                        cur_group -= 1;
                    }
                    break;
                case SettingPanel.GameMode.PM_INFINITE:
                    cur_group += 1;
                    group_txt.text = "CUR: " + cur_group;
                    for (int i = 0; i < 1; i++)
                    {
                        yield return StartCoroutine(create_enemy(sml_dragon_cnt, EnemyProduct.Type.FireDragonSmall));
                        yield return StartCoroutine(create_enemy(mid_dragon_cnt, EnemyProduct.Type.FireDragonMid));
                        yield return StartCoroutine(create_enemy(big_dragon_cnt, EnemyProduct.Type.FireDragonBig));
                        yield return new WaitForSeconds(5);
                        for (int j = 0; j < start_btn_clection.transform.childCount; j++)
                        {
                            start_btn_clection.transform.GetChild(j).gameObject.SetActive(true);
                        }
                    }
                    break;
            }
        }
    }

    IEnumerator create_enemy(int count, EnemyProduct.Type type)
    {
        if (is_gameover)
        {
            yield break;
        }
        for (int i = 0; i < count; i++)
        {
            //创建属性，对象
            EnemyProduct enemy_pro = EnemyFC.Instance.create_enemy(type);
            if (game_mode == SettingPanel.GameMode.PM_INFINITE)
            {
                enemy_pro.maxHp = (int)(enemy_pro.maxHp * (1 + cur_group * 2 * 0.01f));//无限模式下每波增强2%
            }
            var enemy = GameObject.Instantiate<Enemy>(Resources.Load<Enemy>(enemy_pro.prefab_path));
            enemy.init(enemy_pro);

            enemy_list.Add(enemy);

            if (network_connected && IsServer)
            {
                enemy.GetComponent<NetworkObject>().Spawn(true);
            }

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator if_game_start()
    {
        yield return new WaitForSeconds(2);//等待数据传输
        if (net_attack_confirm.Value == true && net_defense_confirm.Value == true)
        {
            update_group(1);
            if (IsServer)
            {
                for (int i = 0; i < 1; i++)
                {
                    yield return StartCoroutine(create_enemy(sml_dragon_cnt, EnemyProduct.Type.FireDragonSmall));
                    yield return StartCoroutine(create_enemy(mid_dragon_cnt, EnemyProduct.Type.FireDragonMid));
                    yield return StartCoroutine(create_enemy(big_dragon_cnt, EnemyProduct.Type.FireDragonBig));
                    yield return new WaitForSeconds(5);
                }
            }
        }
        else
        {
            update_if_game_start(false);//启动失败置为false
        }
    }

    public void show_choose_panel()
    {
        choose_panel.gameObject.SetActive(true);
    }

    public void update_gold(int num)
    {
        cur_gold += num;
        gold_txt.text = cur_gold + "$";

        if (network_connected)
        {
            if (IsServer)
            {
                net_cur_gold.Value = cur_gold;
            }
            else if (IsClient)
            {
                UpdateGoldServerRpc(cur_gold);
            }
        }
    }

    public void update_heart(int num)
    {
        cur_heart += num;
        heart_txt.text = "x" + cur_heart;

        if (network_connected)
        {
            if (IsServer)
            {
                net_cur_heart.Value = cur_heart;
                StartCoroutine(if_game_over());
            }
            else if (IsClient)
            {
                UpdateHeartServerRpc(cur_heart);
            }
        }
        else
        {
            if (cur_heart <= 0)
            {
                game_over();
            }
        }
    }

    public void update_group(int num)
    {
        cur_group += num;
        group_txt.text = "CUR" + cur_group;

        if (network_connected)
        {
            if (IsServer)
            {
                net_cur_group.Value = cur_group;
            }
            else if (IsClient)
            {
                UpdateGroupServerRpc(cur_group);
            }
        }
    }

    public void update_def_confirm(bool confirm)
    {
        if (network_connected)
        {
            if (IsServer)
            {
                net_defense_confirm.Value = confirm;
            }
            else if (IsClient)
            {
                UpdateDefConfirmServerRpc(confirm);
            }
        }
    }

    public void update_atk_confirm(bool confirm)
    {
        if (network_connected)
        {
            if (IsServer)
            {
                net_attack_confirm.Value = confirm;
            }
            else if (IsClient)
            {
                UpdateAtkConfirmServerRpc(confirm);
            }
        }
    }

    public void update_if_game_start(bool confirm)
    {
        if (network_connected)
        {
            if (IsServer)
            {
                net_if_game_start.Value = confirm;
            }
            else if (IsClient)
            {
                UpdateIfGameStartServerRpc(confirm);
            }
        }
    }

    public void update_total_group(int _total_group)
    {
        if (network_connected)
        {
            if (IsServer)
            {
                net_total_group.Value = _total_group;
            }
            else if (IsClient)
            {
                UpdateTotalGroupServerRpc(_total_group);
            }
        }
    }

    public void update_total_heart(int _total_heart)
    {
        if (network_connected)
        {
            if (IsServer)
            {
                net_total_heart.Value = _total_heart;
            }
            else if (IsClient)
            {
                UpdateTotalHeartServerRpc(_total_heart);
            }
        }
    }

    public void update_tower_united_type(int type)
    {
        if (network_connected)
        {
            if (IsServer)
            {
                cur_tower_btn.cur_tower.net_united_type.Value = type;
            }
            else if (IsClient)
            {
                UpdateTowerUnitedTypeServerRpc(type);
            }
        }
    }

    public void update_tower_fire_speed(float speed)
    {
        if (network_connected)
        {
            if (IsServer)
            {
                cur_tower_btn.cur_tower.net_fire_speed.Value = speed;
            }
            else if (IsClient)
            {
                UpdateTowerFireSpeedServerRpc(speed);
            }
        }
    }

    public void update_tower_atk(int atk)
    {
        if (network_connected)
        {
            if (IsServer)
            {
                cur_tower_btn.cur_tower.net_atk.Value = atk;
            }
            else if (IsClient)
            {
                UpdateTowerAtkServerRpc(atk);
            }
        }
    }

    public void net_create_tower(int tower_type, int tower_btn_seq)
    {
        if (network_connected)
        {
            if (IsServer)
            {
                var tower_pro = TowerFC.Instance.create_tower((TowerProduct.Type)tower_type);
                var tower = Instantiate<Tower>(Resources.Load<Tower>(tower_pro.prefab_path));//预制体
                tower.GetComponent<NetworkObject>().Spawn();
                tower.init(tower_pro);//属性
                tower.transform.position = cur_tower_btn.transform.position;//位置
                tower.draw_range();

                cur_tower_btn.cur_tower = tower;
                cal_tower_num_by_type(tower, true);

                net_cur_tower_btn.Value = tower_btn_seq;
                net_cur_tower.Value = tower.GetComponent<NetworkObject>().NetworkObjectId;
                net_cur_tower_pro.Value = tower_type;
            }
            else if (IsClient)
            {
                CreateTowerServerRpc(tower_type, tower_btn_seq);
            }
        }
    }
    public void net_create_united_obj(string obj_path)
    {
        if (network_connected)
        {
            if (IsServer)
            {
                var obj = (GameObject)Instantiate(Resources.Load(obj_path));
                obj.GetComponent<NetworkObject>().Spawn();
                obj.transform.position = cur_tower_btn.cur_tower.united_pos.position;
                obj.transform.parent = cur_tower_btn.cur_tower.transform;
                cur_tower_btn.cur_tower.united_obj = obj;

                net_cur_tower_united_obj.Value = obj.GetComponent<NetworkObject>().NetworkObjectId;
            }
            else if (IsClient)
            {
                CreateUnitedObjServerRpc(obj_path);
            }
        }
    }

    public void net_destroy_tower()
    {
        if (network_connected)
        {
            if (IsServer)
            {
                Destroy(cur_tower_btn.cur_tower.gameObject);
            }
            else if (IsClient)
            {
                DestroyTowerServerRpc();
            }
        }
    }

    public void net_destroy_united_obj()
    {
        if (network_connected)
        {
            if (IsServer)
            {
                //cur_tower_btn.cur_tower.united_obj.GetComponent<NetworkObject>().Despawn();
                Destroy(cur_tower_btn.cur_tower.united_obj.gameObject);
            }
            else if (IsClient)
            {
                DestroyUnitedObjServerRpc();
            }
        }
    }

    public void net_destroy_lightning()
    {
        if (network_connected)
        {
            if (IsServer)
            {
                if (cur_tower_btn.cur_tower.tower_pro.atk_type == TowerProduct.AttackType.Magic &&
                cur_tower_btn.cur_tower.GetComponent<MagicTower>().Lightning)
                {
                    Destroy(cur_tower_btn.cur_tower.GetComponent<MagicTower>().Lightning.gameObject);
                }
            }
            else if (IsClient)
            {
                DestroyLightningServerRpc();
            }
        }
    }

    IEnumerator if_game_over()
    {
        yield return new WaitForSeconds(0.1f);
        if (cur_heart <= 0)
        {
            is_gameover = true;
            is_win = is_defense ? false : true;
            if (IsServer)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("End", LoadSceneMode.Single);
            }
        }
    }

    public void remove_enemy(Enemy enemy)
    {
        if (network_connected)
        {
            if (IsServer)
            {
                enemy_list.Remove(enemy);
                enemy_total_cnt -= 1;
                if (enemy_total_cnt <= 0 && !is_gameover)
                {
                    is_win = true;
                    SceneManager.LoadScene("End");
                }
            }
        }
        else
        {
            enemy_list.Remove(enemy);
            if (game_mode == SettingPanel.GameMode.PM_NORMAL)
            {
                enemy_total_cnt -= 1;
                if (enemy_total_cnt <= 0 && !is_gameover)
                {
                    is_win = true;
                    SceneManager.LoadScene("End");
                }
            }
        }
    }

    //计算某点周围小怪
    public List<Enemy> find_enemies(Vector3 center, float range, int emy_num)
    {
        List<Enemy> emy_lst = new List<Enemy>();
        foreach (Enemy emy in GameControl.Instance.enemy_list)
        {
            if (emy_lst.Count >= emy_num)
            {
                return emy_lst;
            }
            var dis = Vector3.Distance(center, emy.transform.position);
            if (dis <= range)
            {
                emy_lst.Add(emy);
            }
        }
        return emy_lst;
    }

    public void cal_tower_num_by_type(Tower _tower, bool is_add)
    {
        switch (_tower.tower_pro.atk_type)
        {
            case TowerProduct.AttackType.Archer:
                num_archer_tower = is_add ? num_archer_tower += 1 : num_archer_tower -= 1;
                break;
            case TowerProduct.AttackType.Cannon:
                num_cannon_tower = is_add ? num_cannon_tower += 1 : num_cannon_tower -= 1;
                break;
            case TowerProduct.AttackType.Magic:
                num_magic_tower = is_add ? num_magic_tower += 1 : num_magic_tower -= 1;
                break;
        }
    }

    public TowerBtn get_tower_btn_from_seq(int _seq)
    {
        foreach (var e in tower_btn_list)
        {
            if (_seq == e.sequence)
            {
                return e;
            }
        }
        return null;
    }

    public void if_net_func(Action net_func, Action off_func)
    {
        if (network_connected)
        {
            net_func();
        }
        else
        {
            off_func();
        }
    }

    #region ServerRpc
    [ServerRpc(RequireOwnership = false)]
    public void UpdateGoldServerRpc(int gold)
    {
        net_cur_gold.Value = gold;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateHeartServerRpc(int heart)
    {
        net_cur_heart.Value = heart;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateGroupServerRpc(int group)
    {
        net_cur_group.Value = group;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateDefConfirmServerRpc(bool confirm)
    {
        net_defense_confirm.Value = confirm;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateAtkConfirmServerRpc(bool confirm)
    {
        net_attack_confirm.Value = confirm;
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void UpdateIfGameStartServerRpc(bool confirm)
    {
        net_if_game_start.Value = confirm;
    }
    [ServerRpc(RequireOwnership = false)]
    public void UpdateTotalGroupServerRpc(int _total_group)
    {
        net_total_group.Value = _total_group;
    }
    [ServerRpc(RequireOwnership = false)]
    public void UpdateTotalHeartServerRpc(int _total_heart)
    {
        net_total_heart.Value = _total_heart;
    }
    [ServerRpc(RequireOwnership = false)]
    public void UpdateTowerUnitedTypeServerRpc(int type)
    {
        cur_tower_btn.cur_tower.net_united_type.Value = type;
    }
    [ServerRpc(RequireOwnership = false)]
    public void UpdateTowerFireSpeedServerRpc(float speed)
    {
        cur_tower_btn.cur_tower.net_fire_speed.Value = speed;
    }
    [ServerRpc(RequireOwnership = false)]
    public void UpdateTowerAtkServerRpc(int atk)
    {
        cur_tower_btn.cur_tower.net_atk.Value = atk;
    }
    [ServerRpc(RequireOwnership = false)]
    public void CreateTowerServerRpc(int tower_type, int tower_btn_seq)
    {
        var tower_pro = TowerFC.Instance.create_tower((TowerProduct.Type)tower_type);
        
        var tower = Instantiate<Tower>(Resources.Load<Tower>(tower_pro.prefab_path));//预制体
        tower.GetComponent<NetworkObject>().Spawn();
        tower.init(tower_pro);//属性
        cur_tower_btn = get_tower_btn_from_seq(tower_btn_seq);
        tower.transform.position = cur_tower_btn.transform.position;//位置
        tower.draw_range();
        cur_tower_btn.cur_tower = tower;

        net_cur_tower_btn.Value = tower_btn_seq;
        net_cur_tower.Value = tower.GetComponent<NetworkObject>().NetworkObjectId;
        net_cur_tower_pro.Value = tower_type;

        Instance.cal_tower_num_by_type(tower, true);
    }
    [ServerRpc(RequireOwnership = false)]
    public void CreateUnitedObjServerRpc(string obj_path)
    {
        var obj = (GameObject)Instantiate(Resources.Load(obj_path));
        obj.GetComponent<NetworkObject>().Spawn();
        obj.transform.position = cur_tower_btn.cur_tower.united_pos.position;
        obj.transform.parent = cur_tower_btn.cur_tower.transform;
        cur_tower_btn.cur_tower.united_obj = obj;

        net_cur_tower_united_obj.Value = obj.GetComponent<NetworkObject>().NetworkObjectId;
    }
    [ServerRpc(RequireOwnership = false)]
    public void DestroyTowerServerRpc()
    {
        Destroy(cur_tower_btn.cur_tower.gameObject);
    }
    [ServerRpc(RequireOwnership = false)]
    public void DestroyUnitedObjServerRpc()
    {
        Debug.Log("1111");
        if (cur_tower_btn.cur_tower.united_obj)
        {
            Debug.Log("222");
            //cur_tower_btn.cur_tower.united_obj.GetComponent<NetworkObject>().Despawn();
            Destroy(cur_tower_btn.cur_tower.united_obj.gameObject);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void DestroyLightningServerRpc()
    {
        if (cur_tower_btn.cur_tower.tower_pro.atk_type == TowerProduct.AttackType.Magic &&
                cur_tower_btn.cur_tower.GetComponent<MagicTower>().Lightning)
        {
            Destroy(cur_tower_btn.cur_tower.GetComponent<MagicTower>().Lightning.gameObject);
        }
    }
    #endregion

    #region 单例
    private static GameControl instance;
    public static GameControl Instance
    {
        get
        {
            return instance;
        }
    }
    
    #endregion
}
