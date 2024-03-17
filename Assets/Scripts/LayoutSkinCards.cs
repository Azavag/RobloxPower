using UnityEngine;

public class LayoutSkinCards : MonoBehaviour
{
    [SerializeField]
    private SkinScriptableObejct[] skinsCard;
    [SerializeField]
    private int activatedCards;
  private void OnValidate()
    {
        if(transform.childCount != skinsCard.Length)
        {
            Debug.LogError("Несоответсвие количества");
        }
        int i = 0;
        foreach (Transform t in transform)
        {
            if (i >= activatedCards)
                t.gameObject.SetActive(false);
            else t.gameObject.SetActive(true);
            t.GetComponent<SkinCard>().skinScriptable = skinsCard[i];
            t.name = skinsCard[i].skinName + "Card";
            i++;
                     
        }

    }
}
