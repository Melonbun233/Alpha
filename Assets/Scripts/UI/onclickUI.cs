using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickUI : MonoBehaviour
{

    private static PlacementController placementController;
    private Ally ally;

    void Awake() {
        placementController = LevelController.getPlacementController();
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            placementController.instStatus(ally);
        }
    }


    private void Start()
    {
        ally = transform.GetComponent<Ally>();
    }
}
