using System;
using UnityEngine;

public class PunchBagsController : MonoBehaviour
{
    [SerializeField]
    private PunchbagCardControl[] cards;
    [SerializeField]
    private PunchbagScriptableObject[] scriptableObjects;

    [SerializeField]
    private Transform[] punchbagsTransforms;
    [SerializeField]
    private int currentPunchbagNumber;
    [SerializeField]
    private ParticleSystem upgradeParticles;

    [SerializeField]
    private PunchbagsShop punchbagsShop;
    private PunchTrainZone punchTrainZone;

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
        punchTrainZone = GetComponent<PunchTrainZone>();
        Initialization();
    }
 

    void Initialization()
    {
        currentPunchbagNumber = Bank.Instance.playerInfo.currentPunchBagNumber;

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].punchbagScriptable = scriptableObjects[i];
        }
        punchbagsShop.GetCardsData(cards, scriptableObjects);
        for (int i = 0; i< punchbagsTransforms.Length; i++)
        {
            TogglePunchBag(i, false);
        }
        OnPunchbagSelected(currentPunchbagNumber);

    }

    private void OnPunchbagSelected(int id)
    {
        TogglePunchBag(currentPunchbagNumber, false);
        TogglePunchBag(id, true);
        upgradeParticles.Play();

        currentPunchbagNumber = id;
        Bank.Instance.playerInfo.currentPunchBagNumber = currentPunchbagNumber;
        punchTrainZone.OnPunchbagSelected(
            scriptableObjects[id].punchbagStats.maxStreak, scriptableObjects[id].punchbagStats.maxClicks);
        //звук
    }

    void TogglePunchBag(int index, bool state)
    {
        punchbagsTransforms[index].gameObject.SetActive(state);
    }
}
