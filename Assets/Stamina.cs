using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Stamina : MonoBehaviour
{
    public float maxStamina = 100f;
    public float staminaRegenCooldown = 1f;
    public float staminaRegenRate = 30f; // Stamina per detik
    public Slider staminaBar;

    private float staminaTimer;

    void Start()
    {
        SetMaxStamina(maxStamina);
    }

    void Update()
    {
        if (staminaTimer > 0)
        {
            staminaTimer -= Time.deltaTime;
        }
        else if (staminaBar.value < maxStamina)
        {
            staminaBar.value += staminaRegenRate * Time.deltaTime;
            if (staminaBar.value > maxStamina)
                staminaBar.value = maxStamina;
        }
    }

    public void SetMaxStamina(float maxStamina)
    {
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    public void useStamina(float amount)
    {
        if (staminaBar.value >= amount)
        {
            staminaBar.value -= amount;
            staminaTimer = staminaRegenCooldown;
        }
    }
}
