using DG.Tweening;
using UnityEngine;

public class ArenaTextAnimation : MonoBehaviour
{

    [SerializeField]
    private GameObject fightText;
   
    [Header("MoveTween")]
    Tween moveTween;
    [SerializeField]
    private float moveDuration;
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private Transform endPoint;
    [SerializeField]
    private Ease moveEase;
    [Header("ScaleTween")]
    Tween scaleTween;
    [SerializeField]
    private Vector3 startScale;
    [SerializeField]
    private float endScale;
    [SerializeField]
    private Ease scaleEase;
    Sequence entranceSequence;
    private float delayDuration = 0.35f;
    private float exitInterval = 0.15f;

    SoundController soundController;

    private void OnDestroy()
    {
        entranceSequence.Kill();
    }
    private void OnDisable()
    {
       
    }

    private void Awake()
    {
        soundController = FindObjectOfType<SoundController>();
    }
    void Start()
    {        
        fightText.transform.position = startPoint.position;
        fightText.transform.localScale = startScale;
        moveTween = fightText.transform.DOMove(endPoint.position, moveDuration)
            .SetEase(moveEase);
        scaleTween = fightText.transform.DOScale(endScale, moveDuration)
            .SetEase(scaleEase);
        entranceSequence = DOTween.Sequence();
        SetupSequence();
        ActivateText(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            EntranceAnimation();
        }
    }

    public void EntranceAnimation()
    {      
        ActivateText(true);      
        entranceSequence.Restart();
    }

    void SetupSequence()
    {              
        entranceSequence.PrependCallback(() => { soundController.Play("Reberb"); });
        entranceSequence.Append(moveTween);
        entranceSequence.Join(scaleTween);
        entranceSequence.AppendCallback(() => { soundController.Play("ReadyFight"); });
        entranceSequence.AppendInterval(delayDuration);
        entranceSequence.Append(fightText.transform.DOScaleY(0, exitInterval));
        entranceSequence.Join(fightText.transform.DOScaleX(20f, exitInterval));
        entranceSequence.OnComplete(() => ActivateText(false));
    }

    void ActivateText(bool state)
    {
        fightText.SetActive(state);
    }

    public float GetFullAnimationDuration()
    {
        return moveDuration + delayDuration + exitInterval;
    }
}
