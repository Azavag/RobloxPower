using DG.Tweening;
using TMPro;
using UnityEngine;

public class TrainHintText : MonoBehaviour
{
    [SerializeField]
    private Transform hintText;

    [SerializeField]
    private float animDuration;
    [SerializeField]
    private float targetScale;
    [SerializeField]
    private Ease animEase;

    void Start()
    {
        hintText.DOScale(targetScale, animDuration)
            .SetEase(animEase)
            .SetLoops(-1, LoopType.Yoyo)
            .Play();
    }

    private void OnDestroy()
    {
        hintText.DOKill();
    }


}
