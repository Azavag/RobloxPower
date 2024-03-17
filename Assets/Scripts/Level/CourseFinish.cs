using System;
using TMPro;
using UnityEngine;

public class CourseFinish : MonoBehaviour
{
    [SerializeField]
    private int coinReward;
    [SerializeField]
    private TextMeshProUGUI rewardText;

    SpawnManager spawnManager;
    
    public static event Action<int> CourseFinished;

    private void Awake()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    private void Start()
    {
        rewardText.text = coinReward.ToString();
    }

    void FinishCourse()
    {
        CourseFinished.Invoke(coinReward);
        spawnManager.FinishCourse();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            FinishCourse();
            return;
        }
        if(other.CompareTag("Bot"))
        {
            other.GetComponent<BotController>().ReturnToSpawn();
        }
    }
}
