using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] float pulseSpeed = 6f;
    [SerializeField] Light light;
    [SerializeField] List<Color> colors;
    int currentColorIndex = 0;
    float maxIntensity, minIntensity;

    bool dimming = false;
    void Start()
    {
        if (!light)
            light = GetComponent<Light>();

        light.color = colors[currentColorIndex];
        maxIntensity = light.intensity;
        minIntensity = maxIntensity / 5;

        dimming = Random.value > 0.5f;

        if (!dimming)
            light.intensity = minIntensity;

        StartCoroutine(PulseLight());
    }

    void Update()
    {
        if(PlayerController.instance.transform.position.z - transform.position.z >= 40f)
        {
            transform.position = new Vector3(0, 0, transform.position.z + 100);

            ++currentColorIndex;
            if (currentColorIndex >= colors.Count)
                currentColorIndex = 0;

            light.color = colors[currentColorIndex];
        }
    }

    IEnumerator PulseLight()
    {
        float targetIntensity;

        if (dimming)
            targetIntensity = minIntensity;
        else
            targetIntensity = maxIntensity;

        while (isActiveAndEnabled)
        {
            light.intensity = Mathf.MoveTowards(light.intensity, targetIntensity, Time.deltaTime * pulseSpeed);

            if (light.intensity <= minIntensity + 0.5f)
                targetIntensity = maxIntensity;

            if (light.intensity >= maxIntensity - 0.5f)
                targetIntensity = minIntensity;


            yield return null;
        }

        yield break;
    }
}
