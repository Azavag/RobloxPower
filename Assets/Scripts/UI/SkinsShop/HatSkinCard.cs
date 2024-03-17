using System;
using UnityEngine.EventSystems;

public class HatSkinCard : SkinCard, IPointerClickHandler
{
    public static event Action<SkinCard> HatCardClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        HatCardClicked?.Invoke(this);
    }
}
