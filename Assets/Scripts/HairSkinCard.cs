using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HairSkinCard : SkinCard, IPointerClickHandler
{
    public static event Action<SkinCard> HairCardClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        HairCardClicked?.Invoke(this);
    }
}
