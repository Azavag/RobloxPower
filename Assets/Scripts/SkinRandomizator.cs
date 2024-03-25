using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinRandomizator : MonoBehaviour
{
    [SerializeField]
    public Transform hairComponent;
    [SerializeField]
    private Transform hatComponent;
    [SerializeField]
    private Transform bagComponent;
    [SerializeField]
    private Transform faceAttachComponent;

    public void RandomHat()
    {
        DeactivateAllHeadAttaches();
        int rangomHatNumber = Random.Range(0, hatComponent.childCount);
        hatComponent.GetChild(rangomHatNumber).gameObject.SetActive(true);
    }
    public void RandomHair()
    {
        DeactivateAllHeadAttaches();
        int rangomHairNumber = Random.Range(0, hairComponent.childCount);
        hairComponent.GetChild(rangomHairNumber).gameObject.SetActive(true);
    }
    public void RandomAttach()
    {
        foreach (Transform attach in faceAttachComponent)
        {
            attach.gameObject.SetActive(false);
        }
        int rangomAttachNumber = Random.Range(0, faceAttachComponent.childCount);
        faceAttachComponent.GetChild(rangomAttachNumber).gameObject.SetActive(true);
    }
    public void RandomBag()
    {
        foreach (Transform bag in bagComponent)
        {
            bag.gameObject.SetActive(false);
        }
        int rangomBagNumber = Random.Range(0, bagComponent.childCount);
        bagComponent.GetChild(rangomBagNumber).gameObject.SetActive(true);
    }

    void DeactivateAllHeadAttaches()
    {
        foreach (Transform hair in hairComponent)
        {
            hair.gameObject.SetActive(false);
        }
        foreach (Transform hat in hatComponent)
        {
            hat.gameObject.SetActive(false);
        }
    }
}
