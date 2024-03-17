using UnityEngine;

public class CanvasRotator : MonoBehaviour
{
    Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void LateUpdate()
    {
        RotateCanvas();
    }

    void RotateCanvas()
    {
        Vector3 cameraPosition = mainCamera.transform.position;

        GetComponent<Canvas>().transform.LookAt(cameraPosition);
        GetComponent<Canvas>().transform.Rotate(0, 180, 0);
    }
}
