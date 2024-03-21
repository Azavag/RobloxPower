using System;
using UnityEngine.EventSystems;

public class AccessoriesSkinCard : SkinCard, IPointerClickHandler
{
    public static event Action<SkinCard> �ccessoriesCardClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        �ccessoriesCardClicked?.Invoke(this);
    }
}
