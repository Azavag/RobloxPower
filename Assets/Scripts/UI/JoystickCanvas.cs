using UnityEngine;
using UnityEngine.UI;

public class JoystickCanvas : MonoBehaviour
{
    [SerializeField]
    private Joystick moveJoystick;
    [SerializeField]
    private Joystick cameraJoystick;
    [SerializeField]
    private Transform moveJoystickHandle;
  private void OnDisable()
    {
        moveJoystick.input = Vector2.zero;
        cameraJoystick.input = Vector2.zero;
        moveJoystickHandle.localPosition = Vector2.zero;
    }
}
