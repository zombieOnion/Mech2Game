using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSettings : MonoBehaviour
{
    public Camera PilotCamera;
    public Camera EWOCamera;

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.f1Key.wasPressedThisFrame)
            SetCameraToFullscreen(0, FullscreenRect, 1, FullscreenRect);
        if(Keyboard.current.f2Key.wasPressedThisFrame)
            SetCameraToFullscreen(1, FullscreenRect, 0, FullscreenRect);
        if(Keyboard.current.f3Key.wasPressedThisFrame)
            SetCameraToFullscreen(0, new Rect(0, 0, 0.5f, 1), 0, new Rect(0.5f, 0, 0.5f, 1));
    }
    private Rect FullscreenRect => new Rect(0, 0, 1, 1);

    private void SetCameraToFullscreen(int pilotDisplay, Rect pilotRect, int ewoDisplay, Rect ewoRect) {
        PilotCamera.targetDisplay = pilotDisplay;
        EWOCamera.targetDisplay = ewoDisplay;
        PilotCamera.rect = pilotRect;
        EWOCamera.rect = ewoRect;
    }

}
