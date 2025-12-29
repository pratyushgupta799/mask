using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Image healthBar;
    private float currentHealth;
    [SerializeField] private Image gameOverScreen;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        AudioManager.Instance.PlayPlayerHurt();
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            gameOverScreen.gameObject.SetActive(true);
        }
    }

    public void Heal(float health)
    {
        currentHealth = Math.Min(currentHealth + health, maxHealth);
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
