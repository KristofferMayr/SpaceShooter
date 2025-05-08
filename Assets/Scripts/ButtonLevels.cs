using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AlphaButtonRaycaster : MonoBehaviour, ICanvasRaycastFilter
{
    public float alphaThreshold = 0.1f;

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = alphaThreshold;
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return image.IsRaycastLocationValid(sp, eventCamera);
    }
}
