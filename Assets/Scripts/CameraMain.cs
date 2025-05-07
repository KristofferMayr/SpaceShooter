using UnityEngine;

public class CameraResolutionSetter : MonoBehaviour
{
    void Start()
    {
        // Set screen resolution (in play mode, affects game view)
        Screen.SetResolution(1920, 1080, false); // false = windowed mode

        // Optional: Set camera aspect ratio directly
        Camera.main.aspect = 1920f / 1080f;

        Debug.Log("Camera resolution set to 1920x1080.");
    }
}
