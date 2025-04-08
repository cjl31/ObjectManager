using UnityEngine;

public class ColorChangeableObject : MonoBehaviour
{
    private Renderer objectRenderer;
    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    public void SetTransparency(float alpha)
    {
        Color color = objectRenderer.material.color;
        color.a = alpha;
        objectRenderer.material.color = color;
    }
    public void SetColor(Color _color)
    {
        Color color = objectRenderer.material.color;
        color.r = _color.r;
        color.g = _color.g;
        color.b = _color.b;
        objectRenderer.material.color = color;
    }
    
}
