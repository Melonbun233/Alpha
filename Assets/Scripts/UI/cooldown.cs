using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class cooldown : MonoBehaviour
{

    public GameObject towerTab;
    private float cd;
    private Image cdIcon;
    void Start()
    {
        cd = towerTab.GetComponent<TowerOption>().cd;
        cdIcon = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (towerTab.GetComponent<TowerOption>().isCd)
        {
            cdIcon.fillAmount += 1 / cd * Time.deltaTime;
        }

        if(cdIcon.fillAmount >= 1)
        {
            towerTab.GetComponent<TowerOption>().isCd = false;
            cdIcon.fillAmount = 0f;
        }
    }
}
