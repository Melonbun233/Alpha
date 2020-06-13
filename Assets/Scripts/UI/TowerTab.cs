using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


// Controller for one tower tab/ one card in the hand
public class TowerTab : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("GameObject reference field.")]
    public Image type1Image;
    public Image type2Image;
    public Text type1LevelText;
    public Text type2LevelText;
    public GameObject towerVisual; // preview in the tower tab
    public Text costText;
    public Image highlight;
    public Color highlightedColor;
    public GameObject[] towerPrefabs;
    public GameObject[] towerPreviewPrefabs;

    [Header("UI icons")]
    public Sprite rangerIcon;
    public Sprite blockerIcon;
    public Sprite physicalIcon; // What's the use of physical icon ??
    public Sprite fireIcon;
    public Sprite waterIcon;
    public Sprite windIcon;
    public Sprite thunderIcon;
    public Image cdIcon;

    [Header("Stuffs set automatically")]
    public GameObject towerPrefab; // prefab used for actual tower building
    public GameObject towerPreview; // preview for the placement when this tower tab is selected

    public AllyData allyData {get; set;} 
    public static TowerTab selectedTowerTab {get; set;}

    // ???
    private static GameObject status;

    // Whether this tower option is in cd
    private bool isCd;
    // Whether this tower tab is selected
    private bool isSelected {get {return selectedTowerTab == this;}}
    // Whether this tower tab is setup and ready
    private bool isTowerTabReady;

    private LevelController levelController;
    private PlacementController placementController;

    void Awake()
    {
        levelController = LevelController.getLevelController();
        placementController = LevelController.getPlacementController();
    }

    // Setup this tower tab using an existing ally data
    public void setupTowerTab(AllyData allyData) {
        this.allyData = allyData;
        // Setup tower preview in the tower tab
        GameObject preview = Instantiate(getTowerPreviewPrefab(allyData.getMainType()), 
            towerVisual.transform);
        preview.transform.localScale = new Vector3(20, 20, 20);

        // Setup icons based on ally's type
        setupTowerTabType(type1Image, type1LevelText, allyData.getMainType(), 
            allyData.getMainTypeLevel());
        setupTowerTabType(type2Image, type2LevelText, allyData.getSubType(),
            allyData.getSubTypeLevel());
        costText.text = $"Cost: {allyData.allyLevelData.cost}";
        
        // Setup tower prefab and tower preview
        towerPrefab = getTowerPrefab(allyData.getMainType());
        towerPreview = Instantiate(getTowerPreviewPrefab(allyData.getMainType()));
        towerPreview.SetActive(false);

        // Setup tower tab ready
        isTowerTabReady = true;
        isCd = false;
    }
    
    // Build this tower on a specific position and rotation
    public GameObject buildTower(Vector3 position, Quaternion rotation, Transform parent) {
        GameObject tower = Ally.spawn(towerPrefab, allyData, position, rotation);
        tower.transform.parent = parent;

        isCd = true;

        // Unselect the tower tab
        deselectTowerTab();

        return tower;
    }

    // Set the tower tab as the selected tower tab
    // If there's already an existing selected tower tab, this method
    // will automatically unselect the previous tower tab
    public void selectTowerTab() {
        if (selectedTowerTab != null) {
            deselectTowerTab();
        }
        towerPreview.SetActive(true);
        highlight.color = highlightedColor;

        selectedTowerTab = this;
    }

    // unselect the current selected tower tab
    public void deselectTowerTab() {
        if (!isSelected) {
            return;
        }
        towerPreview.SetActive(false);
        highlight.color = Color.white;

        selectedTowerTab = null;
    }


    void setupTowerTabType(Image iconImage, Text levelText, 
        AllyType type, int typeLevel) {

        switch (type)
        {
            case AllyType.Fire:
                iconImage.sprite = fireIcon;
                levelText.text = typeLevel.ToString();
                break;
            case AllyType.Water:
                iconImage.sprite = waterIcon;
                levelText.text = typeLevel.ToString();
                break;
            case AllyType.Thunder:
                iconImage.sprite = thunderIcon;
                levelText.text = typeLevel.ToString();
                break;
            case AllyType.Wind:
                iconImage.sprite = windIcon;
                levelText.text = typeLevel.ToString();
                break;
            case AllyType.Ranger:
                iconImage.sprite = rangerIcon;
                levelText.text = typeLevel.ToString();
                break;
            case AllyType.Blocker:
                iconImage.sprite = blockerIcon;
                levelText.text = typeLevel.ToString();
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isTowerTabReady) {
            return;
        }

        if (Input.GetMouseButtonDown(1) && status==null)
        {
            status = placementController.instStatus(allyData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isTowerTabReady) {
            return;
        }

        if (status != null)
        {
            Destroy(status);
        }
    }

    // Select this tower tab only when this tab is ready, 
    // and the player has enought mana, and this tower tab is not in cd
    // However, if this tower tab is already selected, unselect this tower tab
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isTowerTabReady) { 
            return;
        }

        if (!placementController.hasEnoughMana(allyData.allyLevelData.cost))
        {
            print("Not Enough Mana");
            return;
        }

        if (isSelected) {
            deselectTowerTab();
            return;
        }

        if (!isCd)
        {
            selectTowerTab();
        } else {
            print("This Tower is on CD!");
            return;
        }
    }

    void updateCD() {
        if (!isTowerTabReady) {
            return;
        }

        if (isCd)
        {
            cdIcon.fillAmount += 1 / allyData.cd * Time.deltaTime;
        }

        if(cdIcon.fillAmount >= 1)
        {
            isCd = false;
            cdIcon.fillAmount = 0f;
        }
    }


    // Update is called once per frame
    void Update()
    {
        updateCD();

        if (levelController.levelEnded()) {
            enabled = false;
        }
    }


    public GameObject getTowerPrefab(AllyType type)
    {
        switch (type)
        {
            case AllyType.Ranger:
                return towerPrefabs[0];
            case AllyType.Blocker:
                return towerPrefabs[1];
            case AllyType.Fire:
                return towerPrefabs[0];
            case AllyType.Thunder:
                return towerPrefabs[0];
            case AllyType.Water:
                return towerPrefabs[0];
            case AllyType.Wind:
                return towerPrefabs[0];
        }
        return null;
    }

    public GameObject getTowerPreviewPrefab(AllyType type)
    {
        switch (type)
        {
            case AllyType.Ranger:
                return towerPreviewPrefabs[0];
            case AllyType.Blocker:
                return towerPreviewPrefabs[1];
            case AllyType.Fire:
                return towerPreviewPrefabs[0];
            case AllyType.Thunder:
                return towerPreviewPrefabs[0];
            case AllyType.Water:
                return towerPreviewPrefabs[0];
            case AllyType.Wind:
                return towerPreviewPrefabs[0];
        }

        return null;
    }
}
