using UnityEngine;

public class ArenaFightTrigger : MonoBehaviour
{
    ArenaFight arenaFight;
    [SerializeField]
    private Transform playerPoint;
    [SerializeField]
    private Transform triggerFx;
    private void Awake()
    {
        arenaFight = GetComponentInParent<ArenaFight>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            arenaFight.StartFight();
            ToggleTriggerFX(false);
            MovePlayer(other.transform);
        }
    }

    void MovePlayer(Transform playerTransform)
    {
        playerTransform.transform.position = playerPoint.position;
        playerTransform.transform.localRotation = playerPoint.rotation;
    }

    public void ToggleTriggerFX(bool state)
    {
        triggerFx.gameObject.SetActive(state);
    }

}
