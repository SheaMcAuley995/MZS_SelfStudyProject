using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable<float>
{
    #region singleton

    public static PlayerHealth instance;

    private void Awake()
    {

        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    [Header("Animation")]
    public Animator animator;
    string lowHealthParameter = "lowHealth";

    [Header("Health Settings")]
    public float startHealth = 100f;
    public float currentHealth;
    float lowHealth;
    float noHealth = 0f;
    float oneHundred = 100f;

    [Header("UI Elements")]
    public Image healthBar;
    public Text healthText;
    string percentage = "%";
    string playerDeath = "Player has died - NYI";

	// Use this for initialization
	void Start ()
    {
        SetHealth();
	}

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        HandleUI();

        if (currentHealth <= lowHealth)
        {
            animator.SetBool(lowHealthParameter, true);
        }
        else
        {
            animator.SetBool(lowHealthParameter, false);
        }

        if (currentHealth <= noHealth)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log(playerDeath);
    }

    void SetHealth()
    {
        currentHealth = startHealth;
        lowHealth = startHealth * 0.25f;
        HandleUI();
    }

    void HandleUI()
    {
        currentHealth = Mathf.RoundToInt(currentHealth);
        currentHealth = Mathf.Clamp(currentHealth, noHealth, startHealth);
        healthBar.fillAmount = currentHealth / startHealth;
        healthText.text = ((currentHealth / startHealth) * oneHundred).ToString() + percentage;
    }
}
