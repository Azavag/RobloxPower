using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HairColorSkinCard : SkinCard, IPointerClickHandler
{
    public static event Action<SkinCard> HairColorCardClicked;
    public void OnPointerClick(PointerEventData eventData)
    {
        HairColorCardClicked?.Invoke(this);
    }

    public void ChangeImageColor(Color color)
    {
        skinImage.color = color;
    }
}
