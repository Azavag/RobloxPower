using UnityEditor;
using UnityEngine;

public class LayoutSkinCards : MonoBehaviour
{
    [SerializeField]
    private SkinScriptableObject[] skinsCard;
    [SerializeField]
    private int activatedCards;

    [SerializeField]
    private GameObject prefab;

    private void OnValidate()
    {
        int i = 0;
        foreach (Transform t in transform)
        {
            if (i >= activatedCards)
                t.gameObject.SetActive(false);
            else t.gameObject.SetActive(true);
            t.GetComponent<SkinCard>().skinScriptable = skinsCard[i];
            skinsCard[i].idNumber = i;
            t.name = skinsCard[i].skinName + "Card_" + skinsCard[i].idNumber;
            i++;

        }

        if (transform.childCount == skinsCard.Length)
            return;

        foreach (var card in skinsCard)
        {
            GameObject clone = PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;
        }
        if(transform.childCount != skinsCard.Length)
        {
            Debug.LogError("Несоответсвие количества");
        }
        
        

    }
}
