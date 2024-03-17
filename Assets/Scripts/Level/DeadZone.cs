using System;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public static event Action PlayerDead;
    public float collisionCheckDistance;
    private Rigidbody rb;
    public bool isStatic;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (isStatic)
            return;
        RaycastHit hit;
        if (rb.SweepTest(transform.forward, out hit, collisionCheckDistance))
        {
            if (hit.collider.transform.CompareTag("Player"))
            {
                PlayerDead?.Invoke();
            }
            //if (hit.transform.CompareTag("Bot"))
            //{
            //    hit.transform.GetComponent<BotController>().ReturnToSpawn();
            //}
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isStatic)
            return;
        if (other.CompareTag("Player"))
        {
            PlayerDead?.Invoke();
        }
        //if (other.CompareTag("Bot"))
        //{
        //    other.GetComponent<BotController>().ReturnToSpawn();
        //}
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isStatic)
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerDead?.Invoke();
        }
    }






}
