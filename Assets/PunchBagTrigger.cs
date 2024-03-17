using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PunchBagTrigger : MonoBehaviour
{
    PunchBagFocus punchBagFocus;
    [SerializeField]
    private PunchTrainZone punchTrainZone;

    [SerializeField]
    private ParticleSystem circleParticle;
    [SerializeField]
    private ParticleSystem fxParticle;
    [SerializeField]
    private MinMaxGradient startGradient;
    [SerializeField]
    private MinMaxGradient targetGradient;

    private void Awake()
    {
        punchBagFocus = transform.parent.GetComponent<PunchBagFocus>();
    }
    void Start()
    {
        ChangeParticleColor(startGradient.color);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeParticleColor(targetGradient.color);
            punchBagFocus.SetTrainingTransform(other.transform);
            punchBagFocus.StartTrain();
            punchTrainZone.FreezePlayerToTrainZone();
        }      
    }


    void ChangeParticleColor(Color gradient)
    {
        var circleColor = circleParticle.colorOverLifetime;
        circleColor.color = gradient;

        var fxColor = fxParticle.colorOverLifetime;
        fxColor.color = gradient;
    }

    public void ResetColor()
    {
        ChangeParticleColor(startGradient.color);
    }
}
