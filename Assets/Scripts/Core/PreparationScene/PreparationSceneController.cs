using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Controls the preparation scene main flow.
**/
public class PreparationSceneController : MonoBehaviour
{
    private static PreparationSceneController preparationSceneController;
    private static bool cannotFindPreparationSceneController;

     
    void Awake() {
        getPreparationSceneController();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static PreparationSceneController getPreparationSceneController() {
        if (preparationSceneController != null) {
            return preparationSceneController;
        }

        if (cannotFindPreparationSceneController) {
            return null;
        }

        GameObject obj = GameObject.Find("PreparationSceneController");
        if (obj == null) {
            Debug.LogError("There shuold be a PreparationSceneController game object in the" + 
            " preparation scene");
            cannotFindPreparationSceneController = true;
            return null;
        }

        preparationSceneController = obj.GetComponent<PreparationSceneController>();
        if (preparationSceneController == null) {
            Debug.LogError("There should be a PreparationSceneController component in the " +
            "PreparationSceneController game object");
            cannotFindPreparationSceneController = true;
            return null;
        }

        return preparationSceneController;
    }
}
