using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Check the project google drive folder Gamestatus.drawio
// for the complete status diagram!!!
public enum GameState {
    // Status in the menu
    Menu,
    // Status during a level
    Level,
    // Status for inter-levels
    Preparation,
}

// Contains the status of the current running game
public class GameStatus {
    /** 
        Specify which scene we are at right now.
        Initial scene is Menu scene.
    **/
    public GameState state {get; set;}
    GameState defaultGameState = GameState.Menu;

    /**
        The hand of the current player. Contains all ally options for the player.
        Initial hand is empty
    **/
    public List<AllyData> hand {get; set;}
    List<AllyData> defaultHand = new List<AllyData>();

    /**
        Specify the number of ally options the player can change in their hand.
        Initial number is 0.
    **/
    public int handUpgradeNum {get; set;}
    int defaultHandUpgradeNum = 0;

    /**
        Specify the number of basic ally options the player can add to their hand.
        Initial number is 2.
    **/
    public int handAddNum {get; set;}
    int defaultHandAddNum = 2;

    /**
        Specify the current level of the game is at.
        Initial level is 0.
    **/
    public int level {get; set;}
    int defaultLevel = 0;

    /**
        Specify the current gold number the player posesses.
        Initial gold number is 0.
    **/
    public int goldNum {get; set;}
    int defaultGoldNum = 0;


    public GameStatus() {
        resetGameStatus();
    }

    // Reset the game status to be the default game status
    public void resetGameStatus() {
        state = defaultGameState;
        level = defaultLevel;
        goldNum = defaultGoldNum;
        handUpgradeNum = defaultHandUpgradeNum;
        handAddNum = defaultHandAddNum;
        hand = defaultHand;
    }
}