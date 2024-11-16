using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static UnityEngine.UI.Image;
using Color = UnityEngine.Color;

public class RayDetect : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            bool res = Physics.Raycast(ray, out hit);
            if (res && hit.transform.GetComponent<ShieldStartBtn>())
            {
                hit.transform.GetComponent<ShieldStartBtn>().on_click();
            }
        }
    }
}
