using UnityEngine;

public class ResolutionScaler2D : MonoBehaviour
{
    // Make these public to allow customization in the Unity Editor
    [Tooltip("Initial scale at the base resolution.")]
    public Vector3 baseScale = new Vector3(1f, 1f, 1f);

    private float baseWidth = 917f;  // Your specified base width
    private float baseHeight = 407f;  // Your specified base height

    void Start()
    {
        ScaleWithResolution();
    }

    void Update()
    {
        ScaleWithResolution();
    }

    private void ScaleWithResolution()
    {
        // Calculate the current width and height ratios based on the current and base resolutions
        float widthRatio = Screen.width / baseWidth;
        float heightRatio = Screen.height / baseHeight;

        // Calculate the new scale by multiplying each component of the baseScale by the respective ratio
        Vector3 newScale = new Vector3(baseScale.x * widthRatio, baseScale.y * heightRatio, baseScale.z);
        transform.localScale = newScale;
    }
}
