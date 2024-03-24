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
            t.name = "Card_" + skinsCard[i].idNumber +"_"+ skinsCard[i].skinName;
            i++;

        }

        if (transform.childCount == skinsCard.Length)
            return;

        foreach (var card in skinsCard)
        {
            Instantiate(prefab, transform);
        }
        if(transform.childCount != skinsCard.Length)
        {
            Debug.LogError("Несоответсвие количества");
        }
        
        

    }
}
