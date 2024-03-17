using UnityEngine;

public class BotsSystem : MonoBehaviour
{
    [SerializeField]
    private Transform[] destinationPoints;

    public Transform GetRandomDestinationPoint()
    {
        int randomNumber = Random.Range(0, destinationPoints.Length);       
        return destinationPoints[randomNumber];
    }
}
