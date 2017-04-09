using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    #region Variables

    // Public Properties
    public float MaximumHealth { get; set; }
    public float HurtingTimer { get; set; }
    public float HurtingDelay { get; set; }
    public bool IsHurting { get; set; }
    public bool IsDead { get; set; }
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value > MaximumHealth) { currentHealth = MaximumHealth; }
            else if (value < 0.0f) { currentHealth = 0.0f; }
            else if (value <= MaximumHealth && value >= 0.0f) { currentHealth = value; }
        }
    }

    public float startHealth = 100f;
    public float currentHealth = 100f;

    #endregion


    #region MonoBehaviour

    // Constructor
    protected void Awake()
    {
    }

    // Use this for initialization
    protected void Start()
    {
        IsHurting = false;
        IsDead = false;
        MaximumHealth = 100.0f;
        HurtingDelay = 1.0f;

        currentHealth = startHealth;
    }

    #endregion


    #region Public Functions

    //
    public void Reset()
    {
        IsHurting = false;
        IsDead = false;
        MaximumHealth = 100.0f;
        HurtingDelay = 1.0f;

        currentHealth = startHealth;
    }

    // 
    public void ChangeHealth(float healthChange)
    {
        IsHurting = true;
        HurtingTimer = Time.time;
        currentHealth += healthChange;

        if (currentHealth <= 0.0f)
        {
            currentHealth = 0.0f;
            IsDead = true;
        }
    }

    #endregion
}