using System;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [Header("Settings")]
    [Range(0f, 1f)] [SerializeField] private float time;
    [Range(0f, 1f)] [SerializeField] private float start = 0.4f;
    [SerializeField] private Vector3 noon;
    [SerializeField] private float length;
    [SerializeField] private float rate;

    [Header("Sun")]
    [SerializeField] private Light sun;
    [SerializeField] private Gradient sunColor;
    [SerializeField] private AnimationCurve sunIntensity;

    [Header("Moon")] [SerializeField] private Light moon;
    [SerializeField] private Gradient moonColor;
    [SerializeField] private AnimationCurve moonIntensity;

    [Header("Skybox")]
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private AnimationCurve skyboxIntensity;
    private static readonly int CubemapTransition = Shader.PropertyToID("_CubemapTransition");
    
    [Header("Other lighting")]
    [SerializeField] private AnimationCurve lightIntensityMultiplier;
    [SerializeField] private AnimationCurve reflectionIntensityMultiplier;

    private void Start()
    {
        time = start;
        rate = 1f / length;
    }

    private void Update()
    {
        { // increment current time
            time += rate * Time.deltaTime;
            if (time > 1) time = 0;
        }

        { // light rotation
            sun.transform.eulerAngles = (time - 0.25f) * noon * 4f;
            moon.transform.eulerAngles = (time - 0.75f) * noon * 4f;
        }

        { // light intensity
            sun.intensity = sunIntensity.Evaluate(time);
            moon.intensity = moonIntensity.Evaluate(time);
        }

        { // change colors
            sun.color = sunColor.Evaluate(time);
            moon.color = moonColor.Evaluate(time);
        }

        { // enable / disable sun and moon
            // for sun
            if (sun.intensity == 0f && sun.gameObject.activeInHierarchy)
                sun.gameObject.SetActive(false);
            else if (sun.intensity > 0f && !sun.gameObject.activeInHierarchy)
                sun.gameObject.SetActive(true);
                
            
            // for moon
            if (moon.intensity == 0f && moon.gameObject.activeInHierarchy)
                moon.gameObject.SetActive(false);
            else if (moon.intensity > 0f && !moon.gameObject.activeInHierarchy)
                moon.gameObject.SetActive(true);
        }

        { // lighting and reflection intensity
            RenderSettings.ambientIntensity = lightIntensityMultiplier.Evaluate(time);
            RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
        }

        skyboxMaterial.SetFloat(CubemapTransition, skyboxIntensity.Evaluate(time));
    }
}