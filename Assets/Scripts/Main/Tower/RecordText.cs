using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class RecordText : MonoBehaviour
{
    public Tower tower;

    void Update()
    {
        Vector3 dir = transform.position - Camera.main.transform.position;
        transform.forward = dir;
    }
}
