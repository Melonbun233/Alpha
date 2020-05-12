using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public bool healthBarEnabled = true;
    public bool manaBarEnabled = true;

    public GameObject healthBar;
    public GameObject innerHealthBar;
    public GameObject manaBar;
    public GameObject innerManaBar;

    public Destroyable destroyable;

    // Start is called before the first frame update
    void Start()
    {
        if (!healthBarEnabled) {
            disableHealthBar();
        }

        if (!manaBarEnabled) {
            disableManaBar();
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

        if (manaBarEnabled) {
            updateManaBar();
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

    private void updateManaBar() {
        // only unit has mana
        if (!(destroyable is Unit)) {
            return;
        }

        int curMana = ((Unit)destroyable).mana;
        int maxMana = ((Unit)destroyable).mana;

        if (maxMana == 0) {
            innerManaBar.GetComponent<Image>().fillAmount = 0f;
        } else {
            innerManaBar.GetComponent<Image>().fillAmount = (float)curMana / (float)maxMana;
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

    public void enableManaBar() {
        manaBarEnabled = true;
        manaBar.SetActive(true);
    }

    public void disableManaBar() {
        manaBarEnabled = false;
        manaBar.SetActive(false);
    }
}
