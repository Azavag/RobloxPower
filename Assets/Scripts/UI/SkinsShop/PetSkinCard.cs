using System;
using UnityEngine.EventSystems;

public class PetSkinCard : SkinCard, IPointerClickHandler
{
    public static event Action<SkinCard> PetCardClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        PetCardClicked?.Invoke(this);
    }
}
