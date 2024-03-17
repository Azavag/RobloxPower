using DG.Tweening;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{ 
    [SerializeField] 
    private PlayerController playerController;
    [SerializeField]
    private FadeScreen fadeScreen;
    [SerializeField] Transform spawnPoint;
    SoundController soundController;

    private void OnEnable()
    {
        DeadZone.PlayerDead += OnPlayerDead;
    }
    private void OnDisable()
    {
        DeadZone.PlayerDead -= OnPlayerDead;
    }
    private void Awake()
    {
        soundController = FindObjectOfType<SoundController>();
    }
    private void Start()
    {
        TransferPlayer();
    }

    void TransferPlayer()
    {
        playerController.transform.position = spawnPoint.position;
    }
    void UnblockPlayer()
    {
        playerController.BlockPlayersInput(false);
    }

    void OnPlayerDead()
    {
        soundController.Play("Death");
        RespawnPlayer();
    }
    public void FinishCourse()
    {
        soundController.Play("Finish");
        RespawnPlayer();
    }

    void RespawnPlayer()
    {
        playerController.BlockPlayersInput(true);
        fadeScreen.StartInFadeScreenTween();
        Invoke("TransferPlayer", fadeScreen.inFadeAnimDuration);
        Invoke("UnblockPlayer", fadeScreen.inFadeAnimDuration + fadeScreen.outFadeAnimDuration/3);        
    }   
}
