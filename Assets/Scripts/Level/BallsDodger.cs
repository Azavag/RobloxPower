using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class BallsDodger : MonoBehaviour
{
    [SerializeField] RollingBall[] rollingBalls;
    [SerializeField]
    private int ballsAmount;

    [SerializeField]
    private float breakInterval;
    private float breakTimer = 0;
    bool areBallsReady;
    bool isTimerGoing;
    List<int> randomBallsNumbers = new List<int>();
    bool[] readyStateArray;


    void Update()
    {
        CheckRollingBallsOnReady();
        if (!areBallsReady)
            return;
        if(areBallsReady && !isTimerGoing)
            ResetTimer();

        if (IsTimerReady())
        {
            randomBallsNumbers = GenerateUniqueRandomNumbers(ballsAmount, 0, rollingBalls.Length - 1);
            StartRollBalls(randomBallsNumbers);
            areBallsReady = false;
            isTimerGoing = false;
        }
    }
    bool IsTimerReady()
    {
        breakTimer -= Time.deltaTime;
        return breakTimer < 0;
    }
    void ResetTimer()
    {
        breakTimer = breakInterval;
        isTimerGoing = true;
    }

    void CheckRollingBallsOnReady()
    {
        readyStateArray = new bool[rollingBalls.Length];
        for (int i = 0; i < rollingBalls.Length; i++)
        {
            readyStateArray[i] = rollingBalls[i].IsReadyToRoll;
        }
        if (readyStateArray.All(b => b))
        {
            areBallsReady = true;
        } 
    }

    void StartRollBalls(List<int> numbers)
    {
        for(int i = 0; i < numbers.Count; i++)
        {
            RollBall(numbers[i]);
        }
    }
    void RollBall(int ballNumber)
    {
        rollingBalls[ballNumber].StartRolling();
    }

    public static List<int> GenerateUniqueRandomNumbers(int count, int minValue, int maxValue)
    {
        if (count > (maxValue - minValue + 1) || count < 0)
        {
            throw new ArgumentException("Ќевозможно сгенерировать указанное количество уникальных чисел в заданном диапазоне.");
        }

        List<int> uniqueNumbers = new List<int>();
        HashSet<int> uniqueCheckSet = new HashSet<int>();

        while (uniqueNumbers.Count < count)
        {
            int randomNumber = UnityEngine.Random.Range(minValue, maxValue + 1);
            
            if (uniqueCheckSet.Add(randomNumber))
            {
                uniqueNumbers.Add(randomNumber);
            }
        }

        return uniqueNumbers;
    }
   

    
}
