using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlowingLight : MonoBehaviour
{
    public Light2D light2D;

    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float speed = 0.7f;

    private bool isIncreasing = true;

    private void Update()
    {
        if (isIncreasing)
        {
            light2D.intensity += speed * Time.deltaTime;
            if (light2D.intensity >= maxIntensity)
            {
                isIncreasing = false;
            }
        }
        else
        {
            light2D.intensity -= speed * Time.deltaTime;
            if (light2D.intensity <= minIntensity)
            {
                isIncreasing = true;
            }
        }
    }
}
