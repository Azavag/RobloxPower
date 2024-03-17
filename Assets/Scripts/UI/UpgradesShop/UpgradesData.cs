using System;
using UnityEngine;

[Serializable]
public class PassiveUpgradesData
{
    public int upgradeValue;
    public int upgradePrice;
}
[Serializable]
public class ActiveUpgradesData
{
    public int upgradeValue;
    public int upgradePrice;
}


public class UpgradesData : MonoBehaviour
{
    [SerializeField]
    public PassiveUpgradesData[] passiveUpgrades;

    [SerializeField]
    public ActiveUpgradesData[] activeUpgrades;

    public int GetPassiveUpgradePrice(int number)
    {
        return passiveUpgrades[number].upgradePrice;       
    }
    public int GetPassiveUpgradeValue(int number)
    {
        return passiveUpgrades[number].upgradeValue;
    }
    public int GetActiveUpgradePrice(int number)
    {
        return activeUpgrades[number].upgradePrice;
    }
    public int GetActiveUpgradeValue(int number)
    {
        return activeUpgrades[number].upgradeValue;
    }



}


