using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Used to control the whole game
public class GameController : MonoBehaviour
{
    public static readonly string MenuSceneName = "MenuScene";
    public static readonly string PreparationSceneName = "PreparationScene";
    public static readonly string LevelSceneName = "LevelScene";
    public Character player;
    public GameStatus gameStatus;

    GameObject loadingUI;

    private static GameController gameController;
    private static bool cannotFindGameController;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        getGameController();

        // Initialize the game status
        gameStatus = new GameStatus();

        // Scene loading UI
        loadingUI = GameObject.Find("SceneLoadingCanvas");
        DontDestroyOnLoad(loadingUI);
        loadingUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Start a new game from menu menu
    // Goes to the first upgrade scene
    public void startGame() {
        if (gameStatus.state != GameState.Menu) {
            Debug.LogWarning("startGame() should only be called at menu");
            return;
        }
        
        StartCoroutine(gotoPreparationScene());
    }

    // Quit the game to desktop, can be called in any scene
    public void quitGame() {
        Debug.Log("Quit game");
        Application.Quit();
    }

    // Discard all current data and return to menu menu
    // This should only be called in a level or upgrade
    // Goes to menu scene
    public void endGame() {
        if (gameStatus.state != GameState.Level || gameStatus.state != GameState.Preparation) {
            Debug.LogWarning("endGame() should only be called during a run");
            return;
        }

        resetGameData();
        StartCoroutine(gotoMenuScene());
    }

    // Discard all current data and restart a new game
    // Goes to the first upgrade scene
    public void restartGame() {
        if (gameStatus.state != GameState.Level || gameStatus.state != GameState.Preparation) {
            Debug.LogWarning("restartGame() should only be called during a run");
            return;
        }

        resetGameData();
        StartCoroutine(gotoPreparationScene());
    }

    // Increment level and Start a new level, should be called within upgrade scene
    // Goes to the level scene
    public void startLevel() {
        if (gameStatus.state != GameState.Preparation) {
            Debug.LogWarning("startLevel() should only be called in upgrade scene");
            return;
        }

        gameStatus.level ++;
        StartCoroutine(gotoLevelScene());
    }

    // End the current level, shuold be called only in level scenes
    // Goes to the upgrade scene
    public void endLevel() {
        if (gameStatus.state != GameState.Level) {
            Debug.LogWarning("endLevel() should only be called within a level");
        }

        StartCoroutine(gotoPreparationScene());
    }

    IEnumerator gotoLevelScene() {
        showSceneLoading("Level");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LevelSceneName);

        while (!asyncLoad.isDone) {
            yield return null;
        }

        LevelSceneController.getLevelSceneController().setupLevel();
        hideSceneLoading("Level");

        gameStatus.state = GameState.Level;
    }

    IEnumerator gotoPreparationScene() {
        showSceneLoading("upgrade");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(PreparationSceneName);

        while (!asyncLoad.isDone) {
            yield return null;
        }

        
        hideSceneLoading("Preparation");

        gameStatus.state = GameState.Preparation;
    }

    IEnumerator gotoMenuScene() {
        showSceneLoading("Menu");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(MenuSceneName);

        while (!asyncLoad.isDone) {
            yield return null;
        }

        
        hideSceneLoading("Menu");

        gameStatus.state = GameState.Menu;
    }

    void showSceneLoading(string name) {
        Debug.Log($"Loading {name} scene...");
        GameObject.Find("MenuCanvas").SetActive(false);
        loadingUI.SetActive(true);
    }

    void hideSceneLoading(string name) {
        Debug.Log($"Scene {name} loaded");
        loadingUI.SetActive(false);
    }

    void resetGameData() {
        gameStatus.resetGameStatus();
    }

    public static GameController getGameController() {
        if (gameController != null) {
            return gameController;
        }

        if (cannotFindGameController) {
            return null;
        }

        GameObject obj = GameObject.Find("GameController");
        if (obj == null) {
            Debug.LogError("There should be a GameController game object in the menu scene");
            cannotFindGameController = true;
            return null;
        }

        gameController = obj.GetComponent<GameController>();
        if (gameController == null) {
            Debug.LogError("There should be a GameController component in the GameController object");
            cannotFindGameController = true;
            return null;
        }

        return gameController;
    }

}
