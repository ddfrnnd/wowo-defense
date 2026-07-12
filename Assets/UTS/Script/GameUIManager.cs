using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    [Header("HUD Elements")]
    public TMP_Text waveText;
    public TMP_Text ammoText;
    public TMP_Text templeHPText;
    public Slider templeHealthBar;

    [Header("Instructions Panel")]
    public GameObject instructionsPanel;
    public Button startGameButton;

    [Header("Pause Menu")]
    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button mainMenuButton;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public Button retryButton;
    public Button gameOverMenuButton;

    [Header("Victory")]
    public GameObject victoryPanel;
    public Button nextLevelButton;
    public Button victoryMenuButton;

    [Header("Mobile UI")]
    public GameObject mobileControlsPanel;

    private bool isPaused = false;
    private bool isGameOver = false;
    private bool isShowingInstructions = false;

    void Awake()
    {
        var canvas = GameObject.Find("GameplayCanvas");
        if (canvas == null) return;

        instructionsPanel = FindInCanvas(canvas, "InstructionsPanel");
        pausePanel = FindInCanvas(canvas, "PausePanel");
        gameOverPanel = FindInCanvas(canvas, "GameOverPanel");
        victoryPanel = FindInCanvas(canvas, "VictoryPanel");
        mobileControlsPanel = FindInCanvas(canvas, "MobileControls");

        pauseButton = FindButtonInCanvas(canvas, "PauseButton");
        waveText = FindTextInCanvas(canvas, "WaveText");
        ammoText = FindTextInCanvas(canvas, "AmmoText");
        templeHPText = FindTextInCanvas(canvas, "TempleHPText");
        templeHealthBar = FindSliderInCanvas(canvas, "TempleHealthBar");

        startGameButton = FindButtonInCanvas(canvas, "StartButton");
        resumeButton = FindButtonInCanvas(canvas, "ResumeButton");
        mainMenuButton = FindButtonInCanvas(canvas, "MainMenuButton");
        retryButton = FindButtonInCanvas(canvas, "RetryButton");
        gameOverMenuButton = FindButtonInCanvas(canvas, "MenuButton");
        nextLevelButton = FindButtonInCanvas(canvas, "NextLevelButton");
        victoryMenuButton = FindButtonInCanvas(canvas, "MenuButton");
    }

    static GameObject FindInCanvas(GameObject canvas, string name)
    {
        foreach (Transform child in canvas.transform)
            if (child.name == name) return child.gameObject;
        return null;
    }

    static Button FindButtonInCanvas(GameObject canvas, string name)
    {
        var allButtons = canvas.GetComponentsInChildren<Button>(true);
        foreach (var btn in allButtons)
            if (btn.name == name) return btn;
        return null;
    }

    static TMP_Text FindTextInCanvas(GameObject canvas, string name)
    {
        var allTexts = canvas.GetComponentsInChildren<TMP_Text>(true);
        foreach (var t in allTexts)
            if (t.name == name) return t;
        return null;
    }

    static Slider FindSliderInCanvas(GameObject canvas, string name)
    {
        var allSliders = canvas.GetComponentsInChildren<Slider>(true);
        foreach (var s in allSliders)
            if (s.name == name) return s;
        return null;
    }

    void Start()
    {
        WireButtonListeners();

        if (instructionsPanel != null) instructionsPanel.SetActive(true);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        bool showingInstructions = instructionsPanel != null && instructionsPanel.activeSelf;
        if (mobileControlsPanel != null)
            mobileControlsPanel.SetActive(!showingInstructions);

        Time.timeScale = showingInstructions ? 0f : 1f;
        isShowingInstructions = showingInstructions;
        isPaused = false;
        isGameOver = false;

        Cursor.lockState = showingInstructions ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = showingInstructions;
    }

    /// <summary>
    /// Wires onClick listeners for all buttons. Called once in Start().
    /// </summary>
    void WireButtonListeners()
    {
        if (startGameButton != null)
        {
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(StartGameplay);
        }
        if (pauseButton != null)
        {
            pauseButton.onClick.RemoveAllListeners();
            pauseButton.onClick.AddListener(PauseGame);
        }
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(ResumeGame);
        }
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
        if (retryButton != null)
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(RetryLevel);
        }
        if (gameOverMenuButton != null)
        {
            gameOverMenuButton.onClick.RemoveAllListeners();
            gameOverMenuButton.onClick.AddListener(GoToMainMenu);
        }
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(NextLevel);
        }
        if (victoryMenuButton != null)
        {
            victoryMenuButton.onClick.RemoveAllListeners();
            victoryMenuButton.onClick.AddListener(GoToMainMenu);
        }
    }

    void Update()
    {
        if (isGameOver || isShowingInstructions) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // ===== INSTRUCTIONS / GUIDE =====

    public void StartGameplay()
    {
        isShowingInstructions = false;
        isPaused = false;
        Time.timeScale = 1f;

        if (instructionsPanel != null)
            instructionsPanel.SetActive(false);

        if (mobileControlsPanel != null)
            mobileControlsPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ===== PAUSE SYSTEM =====

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (mobileControlsPanel != null)
            mobileControlsPanel.SetActive(false);

        if (pauseButton != null)
            pauseButton.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (mobileControlsPanel != null)
            mobileControlsPanel.SetActive(true);

        if (pauseButton != null)
            pauseButton.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ===== GAME OVER =====

    public void ShowGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (pauseButton != null)
            pauseButton.gameObject.SetActive(false);

        if (mobileControlsPanel != null)
            mobileControlsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ===== VICTORY =====

    public void ShowVictory()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        if (pauseButton != null)
            pauseButton.gameObject.SetActive(false);

        if (mobileControlsPanel != null)
            mobileControlsPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        isGameOver = false;

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Level1")
            SceneManager.LoadScene("Level2");
        else if (currentScene == "Level2")
            SceneManager.LoadScene("MainMenu");
        else
            SceneManager.LoadScene("MainMenu");
    }

    // ===== NAVIGATION =====

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }

    // ===== HUD UPDATES =====

    public void UpdateWaveText(int currentWave, int totalWaves)
    {
        if (waveText != null)
            waveText.text = "WAVE " + currentWave + " / " + totalWaves;
    }

    public void UpdateAmmoText(int current, int max)
    {
        if (ammoText != null)
        {
            ammoText.text = current + " / " + max;

            if (current <= 5)
                ammoText.color = new Color(1f, 0.3f, 0.3f, 1f);
            else if (current <= 10)
                ammoText.color = new Color(1f, 0.8f, 0.2f, 1f);
            else
                ammoText.color = Color.white;
        }
    }

    public void UpdateTempleHP(int currentHP, int maxHP)
    {
        if (templeHPText != null)
            templeHPText.text = "Temple HP: " + currentHP + " / " + maxHP;

        if (templeHealthBar != null)
        {
            templeHealthBar.maxValue = maxHP;
            templeHealthBar.value = currentHP;
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}
