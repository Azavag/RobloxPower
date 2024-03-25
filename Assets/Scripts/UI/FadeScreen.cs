using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    [SerializeField]
    private Image blackImage;
    [SerializeField]
    private Color targetColor;
    private Color startColor;
    [SerializeField]
    public float inFadeAnimDuration;
    [SerializeField]
    public float outFadeAnimDuration;
    [SerializeField]
    private Ease inFadeAnimEase;
    [SerializeField]
    private Ease outFadeAnimEase;
    [SerializeField]
    private Ease startAnimEase;
    Tween inFadeScreenTween;
    Tween outFadeScreenTween;

    private float enterFadeDuration = 0.5f;
    private float exitFadeDuration = 0.6f;
    [SerializeField]
    private UINavigation uiNavigation;
    [SerializeField]
    private PlayerController playerController;

    void Start()
    {
        startColor = blackImage.color;
        inFadeScreenTween = ChangeColorTween(targetColor, inFadeAnimDuration, inFadeAnimEase);
        outFadeScreenTween = ChangeColorTween(startColor, outFadeAnimDuration, outFadeAnimEase);

       
        EnterLevelFadeOut();
    }
    private void OnDestroy()
    {
        inFadeScreenTween.Kill();
        outFadeScreenTween.Kill();
        blackImage.DOKill();
    }
    private void Update()
    {
        
    }
    public void StartInFadeScreenTween()
    {
        uiNavigation.ToggleFadeScreenCanvas(true);
        inFadeScreenTween.Rewind();
        inFadeScreenTween.OnComplete(() => { StartOutFadeScreenTween(); });
        inFadeScreenTween.Play();
    }
    public void StartOutFadeScreenTween()
    {
        outFadeScreenTween.Rewind();
        outFadeScreenTween.OnComplete(() => { uiNavigation.ToggleFadeScreenCanvas(false); });
        outFadeScreenTween.Play();
    }

    public Tween ChangeColorTween(Color targetColor, float duration, Ease ease)
    {
        Tween colorTween = blackImage.DOColor(targetColor, duration);
        colorTween.SetEase(ease);
        return colorTween;
    }

    public void EnterLevelFadeOut()
    {
        playerController.BlockPlayersInput(true);
        uiNavigation.ToggleFadeScreenCanvas(true);      
        blackImage.color = targetColor;
        Tween colorTween = ChangeColorTween(startColor, enterFadeDuration, startAnimEase);
        colorTween.OnComplete(delegate
        { 
            playerController.BlockPlayersInput(false);
            uiNavigation.ToggleFadeScreenCanvas(false);
        });
        colorTween.SetAutoKill(true);
        colorTween.Play();
    }
    public void ExitLevelFadeIn(TweenCallback callback)
    {
        playerController.BlockPlayersInput(true);
        uiNavigation.ToggleFadeScreenCanvas(true);
        Tween colorTween = ChangeColorTween(targetColor, exitFadeDuration, inFadeAnimEase);
        colorTween.OnComplete(callback);
        colorTween.SetAutoKill(true);
        colorTween.Play();
    }

    public float GetInFadeDuration()
    {
        return inFadeAnimDuration;
    }
    public float GetOutFadeDuration()
    {
        return outFadeAnimDuration;
    }

}
