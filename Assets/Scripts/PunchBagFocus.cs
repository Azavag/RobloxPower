using UnityEngine;

public class PunchBagFocus : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem gainPowerPartcles;
    public static bool IsTraining;
    private Transform trainingTransform;
    private PunchbagAnimation punchbagAnimation;

    private void Awake()
    {
        punchbagAnimation = GetComponentInChildren<PunchbagAnimation>();
    }
    private void Start()
    {
        IsTraining = false;
    }
    public void StartTrain()
    {
        IsTraining = true;        
    }

    public void EndTrain()
    {
        IsTraining = false;
    }

    private void Update()
    {
        if(IsTraining)
        {
            trainingTransform.LookAt(transform.position);            
        }
    }

    public void SetTrainingTransform(Transform transform)
    {
        trainingTransform = transform;
    }

    public void OnClickTrainButton()
    {
        punchbagAnimation.PlayHitAnimation();
        gainPowerPartcles.Play();
        //Звук
    }
}
