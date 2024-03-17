using System;
using UnityEngine;

public class SkinCharacteristics : MonoBehaviour
{
    private int passiveStats;
    private int activeStats;

    public static event Action CharacteristicsChanged;

    public void SetStatsFromSkin(SkinCard skinCard)
    {
        passiveStats = skinCard.skinScriptable.skinStats.passiveStats;
        activeStats = skinCard.skinScriptable.skinStats.activeStats;
        CharacteristicsChanged?.Invoke();
    }

    public int GetPassiveStats()
    {
        return passiveStats;
    }
    public int GetActiveStats()
    {
        return activeStats;
    }
}
