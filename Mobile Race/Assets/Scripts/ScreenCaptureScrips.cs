using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCaptureScrips : MonoBehaviour
{
    private void Awake()
    {
        ScreenCapture.CaptureScreenshot("Track2_Image.png");
    }
}
