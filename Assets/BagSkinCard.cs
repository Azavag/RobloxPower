using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BagSkinCard : SkinCard, IPointerClickHandler
{
    public static event Action<SkinCard> BagCardClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        BagCardClicked?.Invoke(this);
    }

}
