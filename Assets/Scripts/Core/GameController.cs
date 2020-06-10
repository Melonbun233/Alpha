using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Check the project google drive folder Gamestatus.drawio
// for the complete status diagram!!!
public enum GameStatus {
    // Status in the menu
    Menu,
    // Status during a level
    Level,
    // Status for inter-levels
    Upgrade,
}

// Used to control the whole game
public class GameController : MonoBehaviour
{
    public static readonly string MenuSceneName = "MenuScene";
    public static readonly string UpgradeSceneName = "UpgradeScene";
    public static readonly string LevelSceneName = "LevelScene";
    public List<AllyData> hand;
    public Character player;
    public GameStatus status;
    public int level;

    GameObject loadingUI;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);

        // Initialize all parameters
        hand = new List<AllyData>();
        level = 0;
        status = GameStatus.Menu;

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
        if (status != GameStatus.Menu) {
            Debug.LogWarning("startGame() should only be called at menu");
            return;
        }
        
        StartCoroutine(gotoUpgradeScene());
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
        if (status != GameStatus.Level || status != GameStatus.Upgrade) {
            Debug.LogWarning("endGame() should only be called during a run");
            return;
        }

        resetGameData();
        StartCoroutine(gotoMenuScene());
    }

    // Discard all current data and restart a new game
    // Goes to the first upgrade scene
    public void restartGame() {
        if (status != GameStatus.Level || status != GameStatus.Upgrade) {
            Debug.LogWarning("restartGame() should only be called during a run");
            return;
        }

        resetGameData();
        StartCoroutine(gotoUpgradeScene());
    }

    // Increment level and Start a new level, should be called within upgrade scene
    // Goes to the level scene
    public void startLevel() {
        if (status != GameStatus.Upgrade) {
            Debug.LogWarning("startLevel() should only be called in upgrade scene");
            return;
        }

        level++;
        StartCoroutine(gotoLevelScene());
    }

    // End the current level, shuold be called only in level scenes
    // Goes to the upgrade scene
    public void endLevel() {
        if (status != GameStatus.Level) {
            Debug.LogWarning("endLevel() should only be called within a level");
        }

        StartCoroutine(gotoUpgradeScene());
    }

    IEnumerator gotoLevelScene() {
        showSceneLoading("Level");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(LevelSceneName);

        while (!asyncLoad.isDone) {
            yield return null;
        }

        LevelController.getLevelController().setupLevel(level, player, hand);
        hideSceneLoading("Level");

        status = GameStatus.Level;
    }

    IEnumerator gotoUpgradeScene() {
        showSceneLoading("upgrade");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(UpgradeSceneName);

        while (!asyncLoad.isDone) {
            yield return null;
        }

        
        hideSceneLoading("Upgrade");

        status = GameStatus.Upgrade;
    }

    IEnumerator gotoMenuScene() {
        showSceneLoading("Menu");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(MenuSceneName);

        while (!asyncLoad.isDone) {
            yield return null;
        }

        
        hideSceneLoading("Menu");

        status = GameStatus.Menu;
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
        hand.Clear();
        level = 0;
    }

    public static bool isBossLevel(int level) {
        if (level > 0 && level % 5 == 0) {
            return true;
        }
        return false;
    }
}
