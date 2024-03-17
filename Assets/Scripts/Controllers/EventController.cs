using System;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public static event Action ballDeactivated;

    public static void InvokeBallDeactivated()
    {
        ballDeactivated?.Invoke();
    }
}
