using DG.Tweening;
using UnityEngine;

public class ObjectCompression : MonoBehaviour
{
    [SerializeField]
    private bool needDeactivateObject;

    [Header("Object")]
    [SerializeField]
    private Transform objectTransform;
    [SerializeField]
    private Vector3 targetScale;
    private Vector3 startScale;
    [Header("Timers")]
    [SerializeField]
    private float breakInterval;
    [SerializeField]
    private float returnInterval;
    [SerializeField]
    private float compressionAnimationTime;
    [SerializeField]
    private Ease compressionEase;
    [SerializeField]
    private Ease uncompressionEase;
    private float breakTimer;
    private float returnTimer;
    bool isPlatfromCompressed;
    Tween compressTween;
    Tween uncompressTween;
    

    void Start()
    {
        startScale = objectTransform.localScale;
        compressTween = CompressTween(targetScale, compressionAnimationTime, compressionEase);
        compressTween.OnComplete(OnCompressedDone);
        uncompressTween = CompressTween(startScale, compressionAnimationTime, uncompressionEase);
        uncompressTween.OnComplete(OnUncompressedDone);
        ResetTimer(ref breakTimer, breakInterval);
        ResetTimer(ref returnTimer, returnInterval);
    }
    private void OnDestroy()
    {
        objectTransform.DOPause();
        objectTransform.DOKill();
    }

    void Update()
    {
        if(!isPlatfromCompressed)
        {
            breakTimer -= Time.deltaTime;
            if(breakTimer <= 0)
            {
                compressTween.Play();
            }
        }
        else
        {
            returnTimer -= Time.deltaTime;
            if(returnTimer <= 0)
            {
                if(needDeactivateObject)
                    objectTransform.gameObject.SetActive(true);
                uncompressTween.Play();
            }
        }
    }
  
    void OnCompressedDone()
    {
        isPlatfromCompressed = true;
        ResetTimer(ref returnTimer, returnInterval);
        if (needDeactivateObject)
            objectTransform.gameObject.SetActive(false);
        uncompressTween.Rewind();
    }
    void OnUncompressedDone()
    {
        isPlatfromCompressed = false;
        ResetTimer(ref breakTimer, breakInterval);
        compressTween.Rewind();
    }
    void ResetTimer(ref float timer, float interval)
    {
        timer = interval;
    }
    Tween CompressTween(Vector3 scale, float animTme, Ease ease)
    {
        Tween compressTween = objectTransform.DOScale(scale, animTme);
        compressTween.SetEase(ease);
        return compressTween;
    }

  
}
