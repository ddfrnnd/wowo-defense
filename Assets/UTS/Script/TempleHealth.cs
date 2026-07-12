using UnityEngine;
using UnityEngine.UI;

public class TempleHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Slider healthBar;

    [Header("Game UI")]
    public GameUIManager gameUI;

    void Start()
    {
        currentHealth = maxHealth;

        // Find GameUIManager if not assigned
        if (gameUI == null)
            gameUI = FindAnyObjectByType<GameUIManager>();

        // Find TempleHealthBar slider if not assigned
        if (healthBar == null)
        {
            GameObject hpBarObj = GameObject.Find("TempleHealthBar");
            if (hpBarObj != null)
                healthBar = hpBarObj.GetComponent<Slider>();
        }

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log("Kuil kena damage! HP sekarang: " + currentHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log("GAME OVER!");

            // Show game over panel
            if (gameUI != null)
                gameUI.ShowGameOver();
        }
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        // Also update via GameUIManager
        if (gameUI != null)
            gameUI.UpdateTempleHP(currentHealth, maxHealth);
    }
}