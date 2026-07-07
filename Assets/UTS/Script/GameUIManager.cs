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

    [Header("Pause Menu")]
    public GameObject instructionsPanel;
    public Button startGameButton;
    public GameObject pausePanel;
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

    void Start()
    {
        if (mobileControlsPanel == null)
        {
            var canvas = FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                var t = canvas.transform.Find("MobileControls");
                if (t != null) mobileControlsPanel = t.gameObject;
            }
        }
        if (mobileControlsPanel == null)
        {
            var mc = FindFirstObjectByType<MobileControlsUI>();
            if (mc != null && mc.transform.parent != null)
            {
                mobileControlsPanel = mc.transform.parent.gameObject;
            }
        }

        if (startGameButton != null)
            startGameButton.onClick.AddListener(StartGameplay);

        // Set up Pause button listeners
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        // Set up Game Over button listeners
        if (retryButton != null)
            retryButton.onClick.AddListener(RetryLevel);
        if (gameOverMenuButton != null)
            gameOverMenuButton.onClick.AddListener(GoToMainMenu);

        // Set up Victory button listeners
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(NextLevel);
        if (victoryMenuButton != null)
            victoryMenuButton.onClick.AddListener(GoToMainMenu);

        // Ensure all panels are hidden at start
        if (instructionsPanel != null) instructionsPanel.SetActive(true);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        if (mobileControlsPanel != null)
            mobileControlsPanel.SetActive(instructionsPanel == null || !instructionsPanel.activeSelf);

        Time.timeScale = instructionsPanel != null ? 0f : 1f;
        isShowingInstructions = instructionsPanel != null;
        isPaused = false;
        isGameOver = false;

        Cursor.lockState = instructionsPanel != null ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = instructionsPanel != null;
    }

    void Update()
    {
        // Don't allow pause if game is over
        if (isGameOver || isShowingInstructions) return;

        // Toggle pause with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // ===== PAUSE SYSTEM =====

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

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (mobileControlsPanel != null)
            mobileControlsPanel.SetActive(false);

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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ===== GAME OVER =====

    public void ShowGameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

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
        isGameOver = true;
        Time.timeScale = 0f;

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

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
            SceneManager.LoadScene("MainMenu"); // Back to menu after final level
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
