using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PantsSkinCard : SkinCard, IPointerClickHandler
{
    public static event Action<SkinCard> PantsCardClicked;
    public void OnPointerClick(PointerEventData eventData)
    {
        PantsCardClicked?.Invoke(this);
    }

    public void ChangeImageColor(Color color)
    {
        skinImage.color = color;
    }
}
