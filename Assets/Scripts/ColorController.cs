using UnityEngine;

public class ColorController : MonoBehaviour
{
    public void SetDefaultColor(Renderer renderer, Color color)
    {
        renderer.material.color = color;
    }

    public void ChangingColorOfCube(Renderer renderer)
    {
        renderer.material.color = Random.ColorHSV();
    }
}
