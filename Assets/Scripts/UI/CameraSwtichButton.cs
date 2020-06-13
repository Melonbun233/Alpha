using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwtichButton : MonoBehaviour
{
    LevelSceneController levelSceneController;

    void Awake() {
        levelSceneController = LevelSceneController.getLevelSceneController();
    }

    private void OnMouseUp()
    {
        levelSceneController.switchCameraAngle();
    }

}
