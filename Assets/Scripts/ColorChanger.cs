using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    public Slider redSlider, greenSlider, blueSlider;
    float alphaVal;
    [SerializeField] Material objectMaterial;
    public void Initialize()
    {
        Color currentColor = objectMaterial.GetColor("_BaseColor");
        redSlider.value = currentColor.r;
        greenSlider.value = currentColor.g;
        blueSlider.value = currentColor.b;
        alphaVal = currentColor.a;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Color newColor = new Color(redSlider.value, greenSlider.value, blueSlider.value, alphaVal);
        objectMaterial.SetColor("_BaseColor", newColor);
    }
}
