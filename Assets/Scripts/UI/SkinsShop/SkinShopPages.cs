using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopPages : MonoBehaviour
{
    [SerializeField]
    private Button[] pageButtons;

    private void OnEnable()
    {
        foreach (var button in pageButtons)
        {
            //button.onClick.AddListener(OnSwapPage);
        }
    }
    private void OnDisable()
    {
        foreach (var button in pageButtons)
        {
            //button.onClick.RemoveAllListeners(OnSwapPage);
        }
    }

    void OnSwapPage()
    {

    }
}
