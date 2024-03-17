using ECM.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private BaseCharacterController characterController;
    bool isButtonHold;
    private void Awake()
    {
        characterController = FindObjectOfType<BaseCharacterController>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isButtonHold = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isButtonHold = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        characterController.jump = isButtonHold;
    }
}
