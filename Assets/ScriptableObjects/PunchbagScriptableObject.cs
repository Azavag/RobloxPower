using System;
using UnityEngine;

[Serializable]
public class PunchbagStats
{
    public int maxClicks;
    public int maxStreak;
}


[CreateAssetMenu(fileName = "NewPunchbagScriptableObject", menuName = "Custom/Create Punchbag Scriptable Object")]
public class PunchbagScriptableObject : ScriptableObject
{
    public int punchbagIdNumber;
    public int punchbagPrice;
    public string rusName;
    public string engName;
    public Sprite punchbagSprite;
    public PunchbagStats punchbagStats;
    public GameObject punchbagObject;

}
