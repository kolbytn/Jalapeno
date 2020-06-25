using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum DaySection
{
    sunrise,
    daytime,
    sunset,
    night
}

public class DayCycle : MonoBehaviour
{

    Light2D skyLight;
    float dayTime;
    public int dayCount{
        get;
        private set;
    }
    public DaySection daySection {
        get;
        private set;
    }

    // all time measured in minutes
    float sunriseLength = 2f;
    float sunnyLength = 4f;
    float sunsetLength = 2f;
    float nightLength = 3f;
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
        skyLight.intensity = dayIntensity;
        fullDayLength = sunriseLength + sunnyLength + sunsetLength + nightLength;
        sunriseSpeed = Mathf.Abs(dayIntensity-nightIntensity)/(sunriseLength*60);
        sunsetSpeed = Mathf.Abs(nightIntensity-dayIntensity)/(sunsetLength*60);
        dayTime = sunriseLength;
    }

    // Update is called once per frame
    void Update()
    {
        dayTime += Time.deltaTime/60;

        if (dayTime > fullDayLength){
            dayTime -= fullDayLength;
        }

        DaySection section = GetDaySection();
        if (daySection == DaySection.night && section == DaySection.sunrise) {
            dayCount++;
            Debug.Log(dayCount.ToString());
        }
        daySection = section;
        
        if (daySection == DaySection.sunrise) {
            skyLight.intensity += sunriseSpeed * Time.deltaTime;
            if (skyLight.intensity > dayIntensity) { 
                skyLight.intensity = dayIntensity;
            }
        }
        else if (daySection == DaySection.daytime) {
            skyLight.intensity = dayIntensity;
        }
        else if (daySection == DaySection.sunset) {
            skyLight.intensity -= sunsetSpeed * Time.deltaTime;
            if (skyLight.intensity < nightIntensity) { 
                skyLight.intensity = nightIntensity;
            }
        }
        else{
            skyLight.intensity = nightIntensity;
        }
    }

    public float GetLightIntensity() {
        return skyLight.intensity;
    }

    // returns an enum integer representing what part of the day it is
    public DaySection GetDaySection() {
        if (dayTime < sunriseLength) {
            return DaySection.sunrise;
        }
        else if (dayTime < sunriseLength + sunnyLength) {
            return DaySection.daytime;
        }
        else if (dayTime < sunriseLength + sunnyLength + sunsetLength) {
            return DaySection.sunset;
        }
        else{
            return DaySection.night;
        }
    }
}
