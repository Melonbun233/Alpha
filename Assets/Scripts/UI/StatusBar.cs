using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public bool healthBarEnabled = true;

    public GameObject healthBar;
    public GameObject innerHealthBar;

    public Destroyable destroyable;

    // Start is called before the first frame update
    void Start()
    {
        if (!healthBarEnabled) {
            disableHealthBar();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyable == null) {
            return;
        }

        if (healthBarEnabled) {
            updateHealthBar();
        }

    }

    private void updateHealthBar() {
        int curHealth = destroyable.health;
        int maxHealth = destroyable.maxHealth;

        if (maxHealth == 0) {
            innerHealthBar.GetComponent<Image>().fillAmount = 0f;
        } else {
            innerHealthBar.GetComponent<Image>().fillAmount = (float)curHealth / (float)maxHealth;
        }
    }


    public void enableHealthBar() {
        healthBarEnabled = true;
        healthBar.SetActive(true);
    }

    public void disableHealthBar() {
        healthBarEnabled = false;
        healthBar.SetActive(false);
    }
}
