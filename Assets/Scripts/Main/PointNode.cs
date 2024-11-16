using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PointNode : MonoBehaviour
{
    public PointNode[] next_points;// ÷∂Ø∏≥÷µ
    List<PointNode> child_pt_list = new List<PointNode>();


    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < next_points.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, next_points[i].transform.position);
        }
    }

    public int get_enemy_num_in_point()
    {
        int emy_num = 0;
        foreach (var e in GameControl.Instance.enemy_list)
        {
            if (e.Next_Point == this)
            {
                emy_num++;
            }
        }
        return emy_num;
    }
}
