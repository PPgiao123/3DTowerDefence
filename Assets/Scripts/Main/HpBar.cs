using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    Image filled;

    void Awake()
    {
        filled = transform.GetComponentInChildren<Image>();
        filled.fillAmount = 1;
    }

    public void update_hp_bar(float progress)
    {
        filled.DOFillAmount(progress, 0.5f);
        //filled.fillAmount = progress;
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.parent.forward = Camera.main.transform.forward;
        transform.forward = Camera.main.transform.forward;
    }
}
