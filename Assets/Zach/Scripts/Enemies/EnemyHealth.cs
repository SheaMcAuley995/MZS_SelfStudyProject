using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct EnemyData
{
    [Header("Stats")]
    public float health;
    public float startHealth;

    //[Header("UI Elements")]
    //public Text healthText;
    //public Image healthBar;
}

public class EnemyHealth : MonoBehaviour, IDamageable<float>
{
    public EnemyData enemy;

	// Use this for initialization
	void Start ()
    {
        enemy.health = enemy.startHealth;
        //HandleUI();
	}

    public void TakeDamage(float amount)
    {
        enemy.health -= amount;

        if (enemy.health <= 0)
        {
            Die();
        }

        //HandleUI();
    }

    public void Die()
    {
        Destroy(gameObject); // A destroy call to own the libs
    }

    //void HandleUI()
    //{
    //    enemy.healthText.text = ((enemy.health / enemy.startHealth) * 100).ToString() + "%";
    //    enemy.healthBar.fillAmount = enemy.health / enemy.startHealth;
    //}
}
