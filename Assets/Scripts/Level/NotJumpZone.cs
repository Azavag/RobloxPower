using System;
using UnityEngine;

public class NotJumpZone : MonoBehaviour
{
    public static event Action EnterNotJumpZone;
    public static event Action ExitNotJumpZone;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnterNotJumpZone.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExitNotJumpZone.Invoke();
        }
    }
}
