using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    [SerializeField]
    private float minSpeed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private NavMeshAgent agent;
    private BotsSystem botsSystem;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    [Tooltip("Бот стоит на месте и только проигрывает аниматор, без NavMesh.")]
    private bool animateOnly;

    private Transform destinationPoint;
    [SerializeField]
    private Vector3 spawnVector;

    [SerializeField]
    private float breakTimer = 0;
    private float breakDuration = 2f;
    [SerializeField]
    private bool isMoving = true;

    public bool isNextDestinationPointFinish;
    Transform nextPoint;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        botsSystem = FindObjectOfType<BotsSystem>();
    }

    void Start()
    {
        spawnVector = transform.position;
        if (animateOnly)
        {
            if (agent != null)
                agent.enabled = false;
            return;
        }
        SetRandomSpeed();
        ResetTimer();
    }
    void Update()
    {
        if (animateOnly)
        {
            return;
        }

        animator.SetFloat("speed", agent.velocity.magnitude);
        animator.SetFloat("verticalSpeed", agent.velocity.y);

        if (!isMoving)
        {
            BreakTimer();
            return;
        }
        if (ReachedDestination())
        {
            agent.velocity = Vector3.zero;
            isMoving = false;
            ResetTimer();
            SetRandomSpeed();
        }      
      
    }

    bool ReachedDestination()
    {
        return Vector3.Distance(transform.position, destinationPoint.position) <= agent.stoppingDistance
            && agent.velocity.sqrMagnitude <= 1f;
    }

    void BreakTimer()
    {
        breakTimer -= Time.deltaTime;
        if(breakTimer <= 0 ) 
        {
            isMoving = true;
            SetDestinationPoint();           
        }
    }
    void SetDestinationPoint()
    {
        agent.enabled = true;      
        destinationPoint = botsSystem.GetRandomDestinationPoint();

        agent.SetDestination(destinationPoint.position);
    }

    public void SetNextPoint(Transform point)
    {
        nextPoint = point;
    }
    public void ReturnToSpawn()
    {
        if (animateOnly)
            return;

        agent.speed = 0;
        agent.enabled = false;
        transform.position = spawnVector;
        ResetTimer();
        SetRandomSpeed();
        isMoving = false;
    }

   
    void SetRandomSpeed()
    {
        agent.speed = Random.Range(minSpeed, maxSpeed);
        //agent.acceleration = agent.speed/2;
    }
    void ResetTimer()
    {
        breakDuration = Random.Range(2, 7);
        breakTimer = breakDuration;
    }

}
