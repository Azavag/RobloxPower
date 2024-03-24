using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIcon : MonoBehaviour
{
    [SerializeField]
    private Image crossImage;
    private Sequence scaleCrossSequence;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private Image faceImage;

    [SerializeField]
    private Image blackWhiteImage;

    float faceImageFadeDuration = 0.3f;

    private void Start()
    {
        scaleCrossSequence = DOTween.Sequence();
        scaleCrossSequence.Append(crossImage.transform.DOScale(2.2f, 0.25f));
        scaleCrossSequence.Append(crossImage.transform.DOScale(1, 0.55f));
        scaleCrossSequence.Append(faceImage.DOFade(0, faceImageFadeDuration));
    }

    public void InitializeIcon(string name, Sprite faceSprite, Sprite blackWhiteFaceSprite)
    {
        nameText.text = name;
        faceImage.sprite = faceSprite;
        blackWhiteImage.sprite = blackWhiteFaceSprite;
    }
    public void SetClosed(bool close)
    {
        ToggleCrossImage(close);
    }
    public void AnimateCross()
    {
        SetClosed(true);       
        scaleCrossSequence.Play();
        scaleCrossSequence.OnComplete(SwitchImages);
    }

    void SwitchImages()
    {
        faceImage.gameObject.SetActive(false);
        blackWhiteImage.gameObject.SetActive(true);
    }
    void ToggleCrossImage(bool state)
    {
        crossImage.gameObject.SetActive(state);
    }

}
