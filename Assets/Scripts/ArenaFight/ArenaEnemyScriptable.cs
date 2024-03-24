using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyScriptableObject", menuName = "Custom/Create Enemy Scriptable Object")]
public class ArenaEnemyScriptable : ScriptableObject
{ 
    public int idNumber;
    public string enemyRusName;
    public string enemyEngName;
    public int enemyPower;
    public int enemyReward;
    public Sprite faceSprite;
    public Sprite blackWhiteFaceSprite;
    public GameObject enemyModel;
}
