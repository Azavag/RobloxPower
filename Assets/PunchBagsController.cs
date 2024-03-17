using System;
using UnityEngine;

public class PunchBagsController : MonoBehaviour
{
    [SerializeField]
    private Transform[] punchbagsTransforms;
    [SerializeField]
    private int currentPunchbagNumber;
    private PunchTrainZone punchTrainZone;
    [SerializeField]
    private ParticleSystem upgradeParticles;

    private void OnEnable()
    {
        PunchbagsShop.PunchbagSelected += OnPunchbagSelected;
    }
    private void OnDisable()
    {
        PunchbagsShop.PunchbagSelected -= OnPunchbagSelected;
    }
   
    private void Awake()
    {

        Initialization();
    }
    void Start()
    {
        
        
    }

    void TogglePunchBag(int index, bool state)
    {
        punchbagsTransforms[index].gameObject.SetActive(state);
    }

    void Initialization()
    {
        currentPunchbagNumber = Bank.Instance.playerInfo.currentPunchBagNumber;
        for (int i = 0; i< punchbagsTransforms.Length; i++)
        {
            TogglePunchBag(i, false);
        }
        TogglePunchBag(currentPunchbagNumber, true);
    }

    private void OnPunchbagSelected(int id)
    {
        TogglePunchBag(currentPunchbagNumber, false);
        TogglePunchBag(id, true);
        upgradeParticles.Play();
        currentPunchbagNumber = id;
        //звук
        Bank.Instance.playerInfo.currentPunchBagNumber = currentPunchbagNumber;
    }
}
