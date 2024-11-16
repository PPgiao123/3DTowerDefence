using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerControl : MonoBehaviour
{
    private int start_coin;
    public int Start_Coin { get { return start_coin; } }
    private int start_heart;
    public int Start_Heart { get { return start_heart; } }
    private int total_group;
    public int Total_Group { get { return total_group; } }

    private void Awake()
    {
        instance = this;
        start_coin = 20000000;//防守方
        start_heart = 10;//防守方
        total_group = 20;//进攻方
    }

    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            Debug.Log("Server Started");
            //Mode
            SettingPanel.Instance.Game_Mode = SettingPanel.GameMode.PM_NORMAL;
            //Coin
            SettingPanel.Instance.Coin = start_coin;
        };

        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            Debug.Log("A new client connected, id = " + id);
            //每有一个新Client就会读取一次
            if (SettingPanel.Instance.Is_Defense == false)
            {
                total_group = int.Parse(GameObject.Find("GroupInput").GetComponent<TMP_InputField>().text);
            }
            else
            {
                start_heart = int.Parse(GameObject.Find("HeartInput").GetComponent<TMP_InputField>().text);
            }

            if (NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClients.Count > 1)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("Main", LoadSceneMode.Single);
            }
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            Debug.Log("A client disconnected, id = " + id);
        };
    }


    private static ServerControl instance;
    public static ServerControl Instance
    {
        get { return instance; }
    }
}
