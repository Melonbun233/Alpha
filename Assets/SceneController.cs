using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void OnMouseDown()
    {
        LevelController.levelCtr.rows = 10;
        LevelController.levelCtr.colums = 10;
        SceneManager.LoadScene("map generation");
    }
}
