using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GlovesSkinCard : SkinCard, IPointerClickHandler
{
    public static event Action<SkinCard> GlovesCardClicked;
    public void OnPointerClick(PointerEventData eventData)
    {
        GlovesCardClicked?.Invoke(this);
    }

    public void ChangeImageColor(Color color)
    {
        skinImage.color = color;
    }
}
