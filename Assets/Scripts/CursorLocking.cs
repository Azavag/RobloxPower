using UnityEngine;
using Cursor = UnityEngine.Cursor;

public static class CursorLocking
{       
    public static void LockCursor(bool state)
    {
        if (IsMobileController.IsMobile)
            return;

        Cursor.visible = !state;
        if(state)
            Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.None;
    }
}

static public class IsMobileController
{
    public static bool IsMobile { get; set; }
}
