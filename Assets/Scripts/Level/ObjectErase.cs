using UnityEngine;
using DG.Tweening;

public class ObjectErase : MonoBehaviour
{
    [Header("Object")]
    [SerializeField]
    private Transform objectTransform;
    private Material objectMaterial;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    private Color startColor;
    [SerializeField]
    private Color endColor;

    [Header("Timers")]
    [SerializeField]
    private float eraseInterval;
    [SerializeField]
    private float returnInterval;
    [SerializeField]
    private float eraseAnimationTime;

    private float timer;
    bool isObjectErase;
    bool isObjectReturn;

    [SerializeField]
    private bool ifNeedTouch;

    //[SerializeField]
    //private BotsSystem botsSystem;

    private void Awake()
    {
        meshRenderer = objectTransform.GetComponent<MeshRenderer>();
        objectMaterial = meshRenderer.material;
        startColor = objectMaterial.color;
        boxCollider = objectTransform.GetComponent<BoxCollider>();
        //botsSystem = FindObjectOfType<BotsSystem>();
    }
    void Start()
    {
        if (!ifNeedTouch)
            isObjectErase = true;
        ResetTimer(eraseInterval);
    }
    private void OnDestroy()
    {
        objectTransform.DOPause();
        objectTransform.DOKill();
    }
    void Update()
    {
        if (isObjectErase)
            EraseObjectTimer();
        if(isObjectReturn)
            ReturnObjectTimer();
    }
    void EraseObjectTimer()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            EraseObject();
            ResetTimer(returnInterval);
        }
    }
    void ReturnObjectTimer()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            ReturnObject();
            ResetTimer(eraseInterval);           
        }
    }
  
    void EraseObject()
    {
        isObjectErase = false;
        Tween colorTween = ChangeObjectColorTween(endColor, eraseAnimationTime);
        colorTween.SetAutoKill(true);
        colorTween.OnComplete(DeactivateObject);
        colorTween.Play();              
    }
    void DeactivateObject()
    {
        if (meshRenderer == null || boxCollider == null)
            return;
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        //botsSystem.BakeSurface();
        isObjectReturn = true;
    }
    void ReturnObject()
    {       
        Tween colorTween = ChangeObjectColorTween(startColor, 0f);
        colorTween.OnComplete(ActivateObject);
        colorTween.SetAutoKill();
        colorTween.Play();               
    }
    void ActivateObject()
    {
        boxCollider.enabled = true;
        meshRenderer.enabled = true;
        //botsSystem.BakeSurface();
        isObjectReturn = false;
        if (!ifNeedTouch)
            isObjectErase = true;
    }
    Tween ChangeObjectColorTween(Color colorValue, float animTime)
    {
        Tween colorTween = objectMaterial.DOColor(colorValue, animTime);
        colorTween.SetAutoKill();
        colorTween.SetEase(Ease.Linear);
        return colorTween;
    }

    void ResetTimer(float interval)
    {
        timer = interval;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isObjectErase = true;
        }
    }

    

}
