using System;
using TMPro;
using UnityEngine;

public class ArenaEnemyBehavior : MonoBehaviour
{
    private int enemyPower;
    private int maxEnemyHealth;
    private int currentEnemyHealth;
    private int enemyDamage;
    private string enemyName;
    private int enemyReward;
    private int enemyKoef = 5;
   [Header("Timers")]
    private float attackInterval = 2.1f;
    private float attackTimer;

    private bool isDead = false;
    private float deathTimer;
    TimeSpan ts;

    private bool isFightState;
    private bool canAttack;

    [Header("Refs")]
    private ParticleSystem stunEffect;
    private ArenaEnemyScriptable currentEnemy;
    private Animator animator;
    private GameObject enemyModel;
    private ArenaFight arenaFight;
    private BotCanvas botCanvas;

    [SerializeField]
    private Canvas deadTimerCanvas;
    [SerializeField]
    private TextMeshProUGUI timerText;

    public static event Action<float> EnemyAttacked;

    private void Awake()
    {
        botCanvas = GetComponentInChildren<BotCanvas>();
        arenaFight = GetComponentInParent<ArenaFight>();
    }
    
    void Start()
    {
        ResetAttackTimer();       
        ToggleDeathTimerCanvas(false);
    }

    public void SetArenaEnemy(ArenaEnemyScriptable arenaEnemy)
    {
        currentEnemy = arenaEnemy;

        Destroy(enemyModel);
        enemyModel = Instantiate(currentEnemy.enemyModel, transform);
        
        if(Language.Instance.languageName == LanguageName.Rus)
            enemyName = currentEnemy.enemyRusName;
        else enemyName = currentEnemy.enemyEngName;

        enemyPower = currentEnemy.enemyPower;
        enemyReward = currentEnemy.enemyReward;
        animator = enemyModel.GetComponentInChildren<Animator>();
        stunEffect = enemyModel.GetComponentInChildren<ParticleSystem>();
        maxEnemyHealth = enemyPower;
        enemyDamage = enemyPower / enemyKoef;
        ResetHealth();
        botCanvas.ShowName(enemyName);
        botCanvas.ShowPower(enemyPower);
        botCanvas.ShowReward(enemyReward);
    }

    void Update()
    {
        if (canAttack)
            AttackTimer();

        if (isDead)
            DeathTimer();
    }
    public void SetFightState(bool fight)
    {
        isFightState = fight;
        animator.ResetTrigger("attack");
        animator.ResetTrigger("win");
        animator.SetBool("isFight", isFightState);

        if (isFightState)
        {
            botCanvas.ToggleCanvas(false);
            ResetHealth();
            ResetAttackTimer();
        }
        else
        {
            botCanvas.ToggleCanvas(true);
            stunEffect.Stop();
        }
    }
    public void GetHit(int damage)
    {
        currentEnemyHealth -= damage;
        EnemyAttacked?.Invoke((float)currentEnemyHealth / maxEnemyHealth);
        if (currentEnemyHealth <= 0)
        {
            CanAttack(false);
            Death();
            arenaFight.OnEnemyLost();
        }
    }

    void Attack()
    {
        arenaFight.OnEnemyAttacking(enemyDamage);
        animator.SetTrigger("attack");
    }

    public void Death()
    {
        isFightState = false;
        animator.ResetTrigger("attack");
        animator.SetTrigger("death");
        stunEffect.Play();
    }

    void AttackTimer()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0 && canAttack)
        {
            Attack();
            ResetAttackTimer();          
        }
    }

    void ResetAttackTimer()
    {
        attackTimer = attackInterval;
    }

    public void StartDeathTimer(float duration)
    {
        isDead = true;
        ToggleDeathTimerCanvas(isDead);
        ResetDeathTimer(duration);
    }
    void DeathTimer()
    {
        deathTimer -= Time.deltaTime;
        ts = TimeSpan.FromSeconds(deathTimer);
        timerText.text = ts.ToString(@"m\:ss");
        if (deathTimer <= 0)
        {
            isDead = false;
            //stunEffect.Stop();
            ToggleDeathTimerCanvas(isDead);
            SetFightState(false);
        }
    }
  
    void ResetDeathTimer(float duration)
    {
        deathTimer = duration;
    }

    void ToggleDeathTimerCanvas(bool toggle)
    {
        deadTimerCanvas.enabled = toggle;
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
