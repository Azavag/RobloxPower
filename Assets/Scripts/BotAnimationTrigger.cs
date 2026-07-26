using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotAnimationTrigger : MonoBehaviour
{
    const string TrainBool = "isTrain";

    [SerializeField]
    float standDuration = 1.5f;
    [SerializeField]
    float standingSpeedThreshold = 0.15f;
    [SerializeField]
    Transform lookTarget;

    readonly Dictionary<Collider, BotSession> sessions = new Dictionary<Collider, BotSession>();
    readonly List<Collider> toRemove = new List<Collider>();

    Vector3 lookPoint;

    sealed class BotSession
    {
        public Transform Transform;
        public Animator Animator;
        public NavMeshAgent Agent;
        public float StandTimer;
        public bool IsTraining;
    }

    void Start()
    {
        lookPoint = lookTarget != null ? lookTarget.position : transform.position;
        lookPoint.y = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bot") || sessions.ContainsKey(other))
            return;

        Animator animator = other.GetComponentInChildren<Animator>();
        if (animator == null)
            return;

        sessions.Add(other, new BotSession
        {
            Transform = other.transform,
            Animator = animator,
            Agent = other.GetComponent<NavMeshAgent>(),
            StandTimer = 0f,
            IsTraining = false
        });
    }

    void OnTriggerExit(Collider other)
    {
        if (!sessions.TryGetValue(other, out var session))
            return;

        StopTraining(session);
        sessions.Remove(other);
    }

    void Update()
    {
        if (sessions.Count == 0)
            return;

        toRemove.Clear();

        foreach (var pair in sessions)
        {
            var session = pair.Value;
            if (session.Transform == null || session.Animator == null)
            {
                toRemove.Add(pair.Key);
                continue;
            }

            if (IsStanding(session))
            {
                session.StandTimer += Time.deltaTime;
                if (!session.IsTraining && session.StandTimer >= standDuration)
                    StartTraining(session);
            }
            else
            {
                session.StandTimer = 0f;
                StopTraining(session);
            }
        }

        for (int i = 0; i < toRemove.Count; i++)
        {
            StopTraining(sessions[toRemove[i]]);
            sessions.Remove(toRemove[i]);
        }
    }

    void LateUpdate()
    {
        foreach (var pair in sessions)
        {
            var session = pair.Value;
            if (!session.IsTraining || session.Transform == null)
                continue;

            Vector3 lookAt = lookPoint;
            lookAt.y = session.Transform.position.y;
            session.Transform.LookAt(lookAt);
        }
    }

    bool IsStanding(BotSession session)
    {
        if (session.Agent == null || !session.Agent.enabled)
            return true;

        return session.Agent.velocity.sqrMagnitude
            <= standingSpeedThreshold * standingSpeedThreshold;
    }

    static void StartTraining(BotSession session)
    {
        session.IsTraining = true;
        session.Animator.SetBool(TrainBool, true);
    }

    static void StopTraining(BotSession session)
    {
        if (session == null || !session.IsTraining)
            return;

        session.IsTraining = false;
        if (session.Animator != null)
            session.Animator.SetBool(TrainBool, false);
    }
}
