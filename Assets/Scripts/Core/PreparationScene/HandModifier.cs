using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Used to modify the hand in the game controller, contains functions to add/modify 
    some basic ally options in hands.
**/
public class HandModifier : MonoBehaviour
{
    private GameController gameController;

    void Awake() {
        gameController = GameController.getGameController();
    }

    public void addAllyOption(AllyData allyData) {
        gameController.gameStatus.hand.Add(allyData);
    }

    public void addDefaultRangerAllyOption() {
        addAllyOption(DefaultAllyData.defaultRangerData);
    }

    public void addDefaultBloackerAllyOption() {
        addAllyOption(DefaultAllyData.defaultBlockerData);
    }

    // This function is not normally called as the game controller
    // is set during awake
    public void setGameController(GameController gameController) {
        this.gameController = gameController;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
