using System;
using UnityEngine.EventSystems;

public class TrailSkinCard : SkinCard, IPointerClickHandler
{
    public static event Action<SkinCard> TrailCardClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        TrailCardClicked?.Invoke(this);
    }
}
