using System;
using UnityEngine;

public class ArenaEnemyBehavior : MonoBehaviour
{
    private ArenaEnemyScriptable currentEnemy;

    private int enemyPower;
    private int maxEnemyHealth;
    private int currentEnemyHealth;
    private int enemyDamage;
    private string enemyName;
    private int enemyReward;

    BotCanvas botCanvas;
    private float attackInterval = 3;
    private float attackTimer;

    private bool isFightState;
    private bool canAttack;

    private Animator animator;
    GameObject enemyModel;
    public static event Action EnemyLost;
    public static event Action<float> EnemyAttacked;
    public static event Action<int> EnemyAttacking;

    private void Awake()
    {
        botCanvas = GetComponentInChildren<BotCanvas>();
    }
    
    void Start()
    {
        ResetTimer();
    }

    public void SetArenaEnemy(ArenaEnemyScriptable arenaEnemy)
    {
        currentEnemy = arenaEnemy;

        Destroy(enemyModel);
        enemyModel = Instantiate(currentEnemy.enemyModel, transform);
        
        enemyName = currentEnemy.enemyName;
        enemyPower = currentEnemy.enemyPower;
        enemyReward = currentEnemy.enemyReward;
        animator = enemyModel.GetComponentInChildren<Animator>();
       
        maxEnemyHealth = enemyPower / 10;
        enemyDamage = enemyPower / 10;
        ResetHealth();
        botCanvas.ShowName(enemyName);
        botCanvas.ShowPower(enemyPower);
        botCanvas.ShowReward(enemyReward);
    }

    void Update()
    {
        if (isFightState)
            Timer();
    }

    public void GetHit(int damage)
    {
        currentEnemyHealth -= damage;
        EnemyAttacked?.Invoke((float)currentEnemyHealth / maxEnemyHealth);
        if (currentEnemyHealth <= 0)
        {
            CanAttack(false);
            Death();
            EnemyLost?.Invoke();
        }
    }

    void Attack()
    {
        EnemyAttacking?.Invoke(enemyDamage);
        animator.SetTrigger("attack");
        Debug.Log("Attack");
    }

    void Death()
    {
        isFightState = false;
        animator.ResetTrigger("attack");
        animator.SetTrigger("death");
    }

    void Timer()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0 && canAttack)
        {
            Attack();
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        attackTimer = attackInterval;
    }

    public void SetFightState(bool fight)
    {
        animator.ResetTrigger("attack");
        animator.ResetTrigger("win");

        if (fight)
        {
            botCanvas.ToggleCanvas(false);
            ResetHealth();
            ResetTimer();
        }
        else botCanvas.ToggleCanvas(true);
        isFightState = fight;
        animator.SetBool("isFight", isFightState);       
    }

    void ResetHealth()
    {
        currentEnemyHealth = maxEnemyHealth;
    }
    public void WinAnimation()
    {
        animator.SetTrigger("win");
    }
    public void CanAttack(bool state)
    {
        canAttack = state;
    }

    public int GetEnemyReward()
    {
        return enemyReward;
    }
}
