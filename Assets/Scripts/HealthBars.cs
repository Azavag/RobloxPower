using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBars : MonoBehaviour
{
    [SerializeField]
    private Image playerHealthBar;
    [SerializeField]
    private Image botHealthBar;
    [SerializeField]
    private Gradient barGradient;

   private void OnEnable()
    {
        ArenaEnemyBehavior.EnemyAttacked += OnEnemyHitted;
    }

    private void OnDisable()
    {
        ArenaEnemyBehavior.EnemyAttacked -= OnEnemyHitted;
    }

    public void ResetFightHealthBar()
    {
        UpdateBotHealthBar(1f);
        UpdatePlayerHealthBar(1f);
    }

    private void OnEnemyHitted(float percent)
    {
        UpdateBotHealthBar(percent);
    }

    private void UpdateBotHealthBar(float persent)
    {
        botHealthBar.fillAmount = persent;
        botHealthBar.color = barGradient.Evaluate(1 - persent);
    }
    public void UpdatePlayerHealthBar(float persent)
    {
        playerHealthBar.fillAmount = persent;
        playerHealthBar.color = barGradient.Evaluate(1 - persent);
    }

}
