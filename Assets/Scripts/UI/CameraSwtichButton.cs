using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwtichButton : MonoBehaviour
{
    LevelController levelController;

    void Awake() {
        levelController = LevelController.getLevelController();
    }

    private void OnMouseUp()
    {
        levelController.switchCameraAngle();
    }

}
