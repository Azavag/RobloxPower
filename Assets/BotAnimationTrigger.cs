using TMPro;
using UnityEngine;

public class BotAnimationTrigger : MonoBehaviour
{
    bool isTraining;
    Transform botTransform;
    Vector3 targetPosition;

    private void Start()
    {
        targetPosition = transform.position;
        targetPosition.y = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bot"))
        {       
            isTraining = true;
            botTransform = other.transform;
           
            other.GetComponent<Animator>().SetBool("isTrain", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            other.GetComponent<Animator>().SetBool("isTrain", false);
            isTraining = false;
        }
    }
    private void LateUpdate()
    {
        if(isTraining)
        {
            botTransform.transform.LookAt(targetPosition);
        }
    }
}
