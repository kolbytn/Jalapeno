using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayCycle : MonoBehaviour
{

    Light2D skyLight;
    float dayTime;
    float periodTime;

    // all time measured in minutes
    float sunriseLength = 0.25f;
    float sunnyLength = 1;
    float sunsetLength = 0.25f;
    float nightLength = 0.5f;
    float fullDayLength;

    float dayIntensity = 1;
    float nightIntensity = 0.1f;

    // determined by the length of the day
    float sunriseSpeed;
    float sunsetSpeed;

    // Start is called before the first frame update
    void Start()
    {
        skyLight = GetComponent<Light2D>();
        skyLight.intensity = nightIntensity;
        fullDayLength = sunriseLength + sunnyLength + sunsetLength + nightLength;
        sunriseSpeed = Mathf.Abs(dayIntensity-nightIntensity)/(sunriseLength*60);
        sunsetSpeed = Mathf.Abs(nightIntensity-dayIntensity)/(sunsetLength*60);
        Debug.Log(sunriseSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        dayTime += Time.deltaTime/60;

        if (dayTime > fullDayLength){
            dayTime -= fullDayLength;
        }

        if (dayTime < sunriseLength) {
            // it is sunrise
            // Debug.Log("Sunrise");
            skyLight.intensity += sunriseSpeed * Time.deltaTime;
            if (skyLight.intensity > dayIntensity) { 
                skyLight.intensity = dayIntensity;
            }
            // Debug.Log(skyLight.intensity.ToString());
        }
        else if (dayTime < sunriseLength + sunnyLength) {
            // it is sunny
            // Debug.Log("day");

            skyLight.intensity = dayIntensity;
        }
        else if (dayTime < sunriseLength + sunnyLength + sunsetLength) {
            // it is sunset
            // Debug.Log("sunset");

            skyLight.intensity -= sunsetSpeed * Time.deltaTime;
            if (skyLight.intensity < nightIntensity) { 
                skyLight.intensity = nightIntensity;
            }
        }
        else{
            // it is night
            // Debug.Log("night");

            skyLight.intensity = nightIntensity;
        }
    }

    public float GetLightIntensity() {
        return skyLight.intensity;
    }
}
