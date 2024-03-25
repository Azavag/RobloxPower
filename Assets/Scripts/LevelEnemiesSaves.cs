using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnemiesSaves : MonoBehaviour
{
    [SerializeField]
    private ArenaFight[] arenaFights;


    private float saveInterval = 2f;
    private float saveTimer = 0;

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
        Bank.Instance.playerInfo.levelEnemiesTimer[index] = timer;
    }

    private void Start()
    {
        int i = 0;
        foreach(var arenaFight in arenaFights)
        {
            if (Bank.Instance.playerInfo.levelEnemiesTimer[i] > 0)
                arenaFight.SavesInitializationTimer(Bank.Instance.playerInfo.levelEnemiesTimer[i]);
            i++;
        }
        saveTimer = saveInterval;
    }

}
