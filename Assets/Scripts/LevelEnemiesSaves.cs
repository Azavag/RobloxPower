using System;
using UnityEngine;

public class LevelEnemiesSaves : MonoBehaviour
{
    [SerializeField]
    private ArenaFight[] arenaFights;

    private void OnEnable()
    {
        ArenaFight.SavedTimer += OnSavedTimer;
    }

    private void OnDisable()
    {
        ArenaFight.SavedTimer -= OnSavedTimer;
    }
    private void OnSavedTimer(ArenaFight fight, float timer)
    {
        int index = Array.IndexOf(arenaFights, fight);
        Bank.Instance.playerInfo.levelEnemiesTimers[index] = timer;
    }

    private void Start()
    {
        int i = 0;
        foreach(var arenaFight in arenaFights)
        {
            if (Bank.Instance.playerInfo.levelEnemiesTimers[i] > 0)
                arenaFight.SavesInitializationTimer(Bank.Instance.playerInfo.levelEnemiesTimers[i]);
            i++;
        }

    }

}
