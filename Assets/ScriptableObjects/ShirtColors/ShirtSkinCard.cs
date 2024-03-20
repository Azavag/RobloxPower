using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShirtSkinCard : SkinCard, IPointerClickHandler
{
    public static event Action<SkinCard> ShirtCardClicked;
    public void OnPointerClick(PointerEventData eventData)
    {
        ShirtCardClicked?.Invoke(this);
    }

    public void ChangeImageColor(Color color)
    {
        skinImage.color = color;
    }
}
